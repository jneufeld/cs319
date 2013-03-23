using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DREAM.Models;
namespace DREAM.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private DREAMContext db = new DREAMContext();

        //
        // GET: /Requests/

        public ActionResult Index()
        {
            return View(db.Requests.ToList());
        }

        //
        // GET: /Requests/Add

        [HttpGet]
        public ActionResult Add()
        {
            Request request = new Request();
            request.Caller = new Caller();
            request.Patient = new Patient();
            RequestViewModel rv = RequestViewModel.CreateFromRequest(request);
            ViewBag.RequestTypeList = BuildRequestTypeDropdownList();
            ViewBag.RegionList = BuildRegionDropdownList();
            ViewBag.GenderList = BuildGenderDropdownList();
            return View(rv);
        }

        //
        // POST: /Requests/Add

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(RequestViewModel rv)
        {
            if (ModelState.IsValid)
            {
                Request request = db.Requests.Create();
                request.Caller = db.Callers.Create();
                request.Patient = db.Patients.Create();

                rv.MapToRequest(request);

                Patient patient = null;
                if (rv.PatientAgencyID != 0)
                {
                    patient = db.Patients.SingleOrDefault(p => p.AgencyID == rv.PatientAgencyID);
                    if (patient != null)
                        request.Patient = patient;
                }
                if (patient == null)
                {
                    request.Patient = db.Patients.Create();
                    rv.MapToRequestPatient(request);
                }

                request.Type = db.RequestTypes.SingleOrDefault(rt => rt.ID == rv.RequestTypeID);
                request.Caller.Region = db.Regions.SingleOrDefault(reg => reg.ID == rv.CallerRegionID);

                request.CreationTime = DateTime.UtcNow;
                request.CompletionTime = null;
                request.CreatedBy = (Guid) Membership.GetUser().ProviderUserKey;
                request.ClosedBy = Guid.Empty;

                // TODO Probably wrap this whole thing in a transaction
                db.Requests.Add(request);
                db.Callers.Add(request.Caller);
                if (request.Patient.ID == 0)
                    db.Patients.Add(request.Patient);
                db.Logs.Add(Log.Create(request, Membership.GetUser()));
                db.SaveChanges();

                return RedirectToAction("Edit", new RouteValueDictionary(new { Id = request.ID }));
            }
            else
            {
                ModelState.AddModelError(ModelState.ToString(), "The add request failed!");
            }

            ViewBag.RequestTypeList = BuildRequestTypeDropdownList();
            ViewBag.RegionList = BuildRegionDropdownList();
            ViewBag.GenderList = BuildGenderDropdownList();

            return View(rv);
        }

        //
        // GET: /Requests/Search

        [HttpGet]
        public ActionResult Search()
        {
            SearchModel sm = new SearchModel();
            return View(sm);
        }

        //
        // POST: /Requests/Search

        [HttpPost]
        public ActionResult Search(SearchModel search)
        {
            search.results = db.Requests.Where(request => request.Patient.FirstName.Equals(search.query));
            search.executed = true;
            return View(search);
        }

        //
        // GET: /Requests/ViewRequest/5

        [HttpGet]
        public ActionResult ViewRequest(int id = 0)
        {
            Request request = FindRequest(id);

            if (request.CreatedBy != null)
                ViewBag.CreatedBy = Membership.GetUser(request.CreatedBy).UserName;

            if (request.ClosedBy != Guid.Empty)
                ViewBag.ClosedBy = Membership.GetUser(request.ClosedBy).UserName;

            if (request == null)
            {
                return HttpNotFound();
            }

            if (isLocked(request))
            {
                return View();
            }

            db.Logs.Add(Log.View(request, Membership.GetUser()));
            db.SaveChanges();
            return View(request);
        }

        //
        // POST: /Requests/ViewRequest/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewRequest(Request request)
        {

            if (isLocked(request, true))
            {
                return View();
            }

            else if (request.Lock() != null)
            {
                return RedirectToAction("Edit", request.ID);
            }

            ModelState.AddModelError("", "View Request failed!");
            return View(request);
        }

        // Find a request and return a view for editing it. If no request exists, we send a HTTP 404 error.
        // If the request is locked, return an empty view (for now).
        //
        // This method is invoked for GET requests on /requests/edit/<INT>.
        //
        // Arguments:
        //      id -- The ID of the request to be edited.
        //
        // Returns:
        //      The view for editing the request, or a 404 if the request doesn't exist.
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Request request = FindRequest(id);

            if (request == null)
            {
                return HttpNotFound();
            }

            ViewBag.IsLocked = false;
            if (isLocked(request, true))
            {
                ViewBag.IsLocked = true;
                return View();
            }

            RequestViewModel rv = RequestViewModel.CreateFromRequest(request);

            rv.CreatedBy = FindUsernameFromID(request.CreatedBy);
            rv.ClosedBy = FindUsernameFromID(request.ClosedBy);

            ViewBag.RequestTypeList = BuildRequestTypeDropdownList();
            ViewBag.RegionList = BuildRegionDropdownList();
            ViewBag.QuestionTypeList = BuildQuestionTypeDropdownList();
            ViewBag.TumourGroupList = BuildTumourGroupDropdownList();
            ViewBag.GenderList = BuildGenderDropdownList();

            return View(rv);
        }

        //
        // POST: /Requests/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RequestViewModel rv)
        {
            Request request = FindRequest(rv.RequestID);
            ViewBag.IsLocked = false;

            if (request == null)
            {
                return HttpNotFound();
            }

            if (isLocked(request, true))
            {
                ViewBag.IsLocked = true;
                return View();
            }

            if (ModelState.IsValid)
            {
                rv.MapToRequest(request);
                rv.MapToRequestPatient(request);

                if (rv.Questions == null)
                    rv.Questions = new List<QuestionViewModel>();
                foreach (var qv in rv.Questions)
                {
                    Question question = null;
                    if (qv.QuestionID != 0 && qv.Delete == false)
                    {
                        question = request.Questions.SingleOrDefault(q => q.ID == qv.QuestionID);
                    }
                    else if (qv.QuestionID != 0 && qv.Delete == true)
                    {
                        // Delete question
                    }
                    else if (qv.QuestionID == 0 && qv.Delete == false)
                    {
                        question = db.Questions.Create();
                        request.Questions.Add(question);
                    }

                    if (question != null)
                    {
                        qv.MapToQuestion(question);
                        question.QuestionType = db.QuestionTypes.SingleOrDefault(qt => qt.ID == qv.QuestionTypeID);
                        question.TumourGroup = db.TumourGroups.SingleOrDefault(tg => tg.ID == qv.TumourGroupID);
                    }
                }

                request.Type = db.RequestTypes.SingleOrDefault(rt => rt.ID == rv.RequestTypeID);
                request.Caller.Region = db.Regions.SingleOrDefault(reg => reg.ID == rv.CallerRegionID);

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            //request.Unlock();
            return View(rv);
        }

        [HttpGet]
        public ActionResult QuestionEntryItem(int id)
        {
            ViewBag.QuestionTypeList = BuildQuestionTypeDropdownList();
            ViewBag.TumourGroupList = BuildTumourGroupDropdownList();
            QuestionViewModel qv = new QuestionViewModel
            {
                Index = id,
            };
            return PartialView("QuestionEntry", qv);
        }

        //
        // GET: /Requests/Delete/5

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Request request = FindRequest(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        //
        // POST: /Requests/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Requests/Export
        [HttpGet]
        public ActionResult Export(int reqId)
        {
            Request request = FindRequest(reqId);
            if (isLocked(request))
            {
                return View();
            }

            MemoryStream ms = ExportDoc(Server.MapPath(@"~/") + "/Templates/Export_Report.docx", request);
            return File(ms.ToArray(), "application/ms-word", "Request" + reqId + ".docx");
        }

        // Need to add null checks for all attributes (since many can be null)
        // Need a better Template?
        // Need a loop to create Questions for the number of questions in the request.
        public MemoryStream ExportDoc(string docName, Request req)
        {
            byte[] byteArray = System.IO.File.ReadAllBytes(docName);
            MemoryStream mem = new MemoryStream();
            mem.Write(byteArray, 0, (int)byteArray.Length);
            using (WordprocessingDocument wordDoc =
                WordprocessingDocument.Open(mem, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                Regex regexText1 = new Regex("REQUESTID GOES HERE");
                docText = regexText1.Replace(docText, req.ID.ToString());
                /*
                Regex regexText2 = new Regex("REQUEST TYPE GOES HERE");
                docText = regexText2.Replace(docText, "Nurse");

                Regex regexText3 = new Regex("CALLERFIRSTNAME GOES HERE");
                docText = regexText3.Replace(docText, "Bob");

                Regex regexText4 = new Regex("CALLERLASTNAME GOES HERE");
                docText = regexText4.Replace(docText, "Marley");

                Regex regexText5 = new Regex("CALLERPHONENUMBER GOES HERE");
                docText = regexText5.Replace(docText, "555-555-5555");

                Regex regexText6 = new Regex("CALLEREMAIL GOES HERE");
                docText = regexText6.Replace(docText, "bobmarley@email.com");

                Regex regexText7 = new Regex("CALLERREGION GOES HERE");
                docText = regexText7.Replace(docText, "Yukon");

                Regex regexText8 = new Regex("PATIENTFIRSTNAME GOES HERE");
                docText = regexText8.Replace(docText, "Jim");

                Regex regexText9 = new Regex("PATIENTLASTNAME GOES HERE");
                docText = regexText9.Replace(docText, "Marley");

                Regex regexText10 = new Regex("PATIENTAGENCYID GOES HERE");
                docText = regexText10.Replace(docText, "4");

                Regex regexText11 = new Regex("PATIENTGENDER GOES HERE");
                docText = regexText11.Replace(docText, "Male");

                Regex regexText12 = new Regex("PATIENTAGE GOES HERE");
                docText = regexText12.Replace(docText, "45");
                */
                Regex regexText2 = new Regex("REQUEST TYPE GOES HERE");
                if (req.Type != null)
                    docText = regexText2.Replace(docText, req.Type.FullName);
                else docText = regexText2.Replace(docText, "none");

                Regex regexText3 = new Regex("CALLERFIRSTNAME GOES HERE");
                if (req.Caller != null && req.Caller.FirstName != null)
                    docText = regexText3.Replace(docText, req.Caller.FirstName);
                else docText = regexText3.Replace(docText, "");

                Regex regexText4 = new Regex("CALLERLASTNAME GOES HERE");
                if (req.Caller != null && req.Caller.LastName != null)
                    docText = regexText4.Replace(docText, req.Caller.LastName);
                else docText = regexText4.Replace(docText, "");

                Regex regexText5 = new Regex("CALLERPHONENUMBER GOES HERE");
                if (req.Caller != null && req.Caller.PhoneNumber != null)
                    docText = regexText5.Replace(docText, req.Caller.PhoneNumber);
                else docText = regexText5.Replace(docText, "");

                Regex regexText6 = new Regex("CALLEREMAIL GOES HERE");
                if (req.Caller != null && req.Caller.Email != null)
                    docText = regexText6.Replace(docText, req.Caller.Email);
                else docText = regexText6.Replace(docText, "");

                Regex regexText7 = new Regex("CALLERREGION GOES HERE");
                if (req.Caller != null && req.Caller.Region != null)
                    docText = regexText7.Replace(docText, req.Caller.Region.FullName);
                else docText = regexText7.Replace(docText, "");

                Regex regexText8 = new Regex("PATIENTFIRSTNAME GOES HERE");
                if (req.Patient != null && req.Patient.FirstName != null)
                    docText = regexText8.Replace(docText, req.Patient.FirstName);
                else docText = regexText8.Replace(docText, "");

                Regex regexText9 = new Regex("PATIENTLASTNAME GOES HERE");
                if (req.Patient != null && req.Patient.LastName != null)
                    docText = regexText9.Replace(docText, req.Patient.LastName);
                else docText = regexText9.Replace(docText, "");

                Regex regexText10 = new Regex("PATIENTAGENCYID GOES HERE");
                if (req.Patient != null)
                    docText = regexText10.Replace(docText, req.Patient.AgencyID.ToString());
                else docText = regexText10.Replace(docText, "");

                Regex regexText11 = new Regex("PATIENTGENDER GOES HERE");
                if (req.Patient != null && (req.Patient.Gender == 1 || req.Patient.Gender == 2))
                {
                    if (req.Patient.Gender == 1)
                        docText = regexText11.Replace(docText, "Male");
                    else docText = regexText11.Replace(docText, "Female");
                }
                else docText = regexText11.Replace(docText, "");

                Regex regexText12 = new Regex("PATIENTAGE GOES HERE");
                if (req.Patient != null && req.Patient.Age > 0)
                    docText = regexText12.Replace(docText, req.Patient.Age.ToString());
                else docText = regexText12.Replace(docText, "");
                
                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

                wordDoc.MainDocumentPart.Document.Body.InsertAt(
                    new Paragraph(
                        new Run(
                            new Text("Question1"))), 16);

            }
            return mem;
        }

        #region Helper Methods

        private Request FindRequest(int id)
        {
            return db.Requests.Include(r => r.Caller)
                              .Include(r => r.Patient)
                              .Include(r => r.Type)
                              .Include(r => r.Caller.Region)
                              .Include(p => p.Questions.Select(c => c.Keywords))
                              .Include(p => p.Questions.Select(c => c.Reference))
                              .Include(p => p.Questions.Select(c => c.QuestionType))
                              .Include(p => p.Questions.Select(c => c.TumourGroup))
                              .Single(r => r.ID == id);
        }

        #region DropDown Lists
        private IEnumerable<SelectListItem> BuildGenderDropdownList()
        {
            List<SelectListItem> genders = new List<SelectListItem>();

            genders.Add(new SelectListItem { Text = "Unknown", Value = Gender.UNKNOWN.ToString() });
            genders.Add(new SelectListItem { Text = "Male", Value = Gender.MALE.ToString() });
            genders.Add(new SelectListItem { Text = "Female", Value = Gender.FEMALE.ToString() });

            return genders;
        }

        private IEnumerable<SelectListItem> BuildRequestTypeDropdownList()
        {
            return BuildTypedDropdownList<RequestType>(db.RequestTypes);
        }

        private IEnumerable<SelectListItem> BuildRegionDropdownList()
        {
            return BuildTypedDropdownList<Region>(db.Regions);
        }

        private IEnumerable<SelectListItem> BuildQuestionTypeDropdownList()
        {
            return BuildTypedDropdownList<QuestionType>(db.QuestionTypes);
        }

        private IEnumerable<SelectListItem> BuildTumourGroupDropdownList()
        {
            return BuildTypedDropdownList<TumourGroup>(db.TumourGroups);
        }

        private IEnumerable<SelectListItem> BuildTypedDropdownList<T>(DbSet<T> dbSet) where T : DropDown
        {
            IEnumerable<T> ts = dbSet.AsEnumerable<T>();
            IEnumerable<SelectListItem> list = ts.Select(r => new SelectListItem
            {
                Value = r.ID.ToString(),
                Text = r.FullName,
            });
            return list;
        }
        #endregion

        // Check if the given request is currently locked, returning true or false.
        //
        // Arguments:
        //      request   -- The current Request object the user is viewing.
        //      isEditing -- Set of the operation is editing the Request itself.
        //
        // Returns:
        //      True if the given Request is currently locked, else false.
        private bool isLocked(Request request, bool isEditing = false)
        {
            bool returnValue = false;
            DREAM.Models.Lock requestLock = findRequestLock(request.ID);
            MembershipUser lockingUser = getUserFromLock(requestLock);
            MembershipUser currentUser = Membership.GetUser();

            if ((requestLock != null) && (!currentUser.UserName.Equals(lockingUser.UserName)))
            {
                ViewBag.LockingUser = lockingUser;
                returnValue = true;
            }
            else if (requestLock != null)
            {
                returnValue = false;
            }
            else
            {
                returnValue = false;
            }

            return returnValue;
        }

        // Find and return the Lock for the given request. If there is no Lock, null is returned.
        //
        // Arguments:
        //      requestID -- The ID (primary key) of the request.
        //
        // Returns:
        //      The Lock holding the given Request or null if there is no lock.
        private DREAM.Models.Lock findRequestLock(int requestID)
        {
            DREAM.Models.Lock returnValue = null;

            List<DREAM.Models.Lock> locks = db.Locks.ToList();
            foreach (DREAM.Models.Lock requestLock in locks)
            {
                if (requestLock.RequestID == requestID)
                {
                    returnValue = requestLock;
                    break;
                }
            }

            return returnValue;
        }

        // Find and return the MembershipUser associated with a Lock.
        //
        // Arguments:
        //      requestLock -- The Lock to find the user object from (may be null).
        //
        // Returns:
        //      The MembershipUser object of the user holding the Lock or null.
        private MembershipUser getUserFromLock(DREAM.Models.Lock requestLock)
        {
            MembershipUser returnValue = null;

            if (requestLock != null)
            {
                returnValue = Membership.GetUser(requestLock.UserID);
            }

            return returnValue;
        }

        // Find and return the username belonging to the given user ID.
        //
        // Arguments:
        //      userID -- The user ID.
        //
        // Returns:
        //      The username of the given user ID or null if none could be found.
        private string FindUsernameFromID(Guid userID)
        {
            MembershipUser user = Membership.GetUser(userID);

            return user != null ? user.UserName : null;
        }

        #endregion
    }
}
﻿