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
    [Authorize(Roles = Role.DI_SPECIALIST + ", " + Role.VIEWER)]
    public class RequestsController : Controller
    {
        private DREAMContext db = new DREAMContext();

        //private SearchIndex SearchIndex = new SearchIndex();

        //
        // GET: /Requests/

        public ActionResult Index()
        {
            /*
             List<RequestViewModel> requests = new List<RequestViewModel>();

            foreach (Request request in db.Requests.ToList())
            {
                RequestViewModel rv = RequestViewModel.CreateFromRequest(request);
                rv.CreatedBy = FindUsernameFromID(request.CreatedBy);
                rv.ClosedBy = FindUsernameFromID(request.ClosedBy);
                requests.Add(rv);
            }
            */

            return View(db.Requests.ToList());
            //return View(requests);
        }

        //
        // GET: /Requests/Add

        [HttpGet]
        [Authorize(Roles=Role.DI_SPECIALIST)]
        public ActionResult Add()
        {
            Request request = new Request();
            request.Caller = new Caller();
            request.Patient = new Patient();
            RequestViewModel rv = RequestViewModel.CreateFromRequest(request);

            PopulateDropDownLists(false, false);
            ViewBag.QuestionTypeList = BuildQuestionTypeDropdownList();
            ViewBag.TumourGroupList = BuildTumourGroupDropdownList();
            ViewBag.ReferenceTypeList = BuildReferenceTypeDropdownList();
            ViewBag.ProbabilityList = BuildProbabilityDropdownList();
            ViewBag.SeverityList = BuildSeverityDropdownList();
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
                request.Enabled = true;

                rv.MapToRequest(request);

                Patient patient = null;
                if (rv.PatientAgencyID != "0")
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

                request.Caller.Type = db.RequesterTypes.SingleOrDefault(rt => rt.ID == rv.RequesterTypeID);
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
                //SearchIndex.AddOrUpdateIndex(request);
                db.Logs.Add(Log.Create(request, Membership.GetUser()));
                db.SaveChanges();

                addQuestions(request, rv);
                db.SaveChanges();

                return RedirectToAction("Edit", new RouteValueDictionary(new { id = request.ID }));
            }
            else
            {
                ModelState.AddModelError(ModelState.ToString(), "The add request failed!");
            }

            PopulateDropDownLists(false, false);
            ViewBag.QuestionTypeList = BuildQuestionTypeDropdownList();
            ViewBag.TumourGroupList = BuildTumourGroupDropdownList();
            ViewBag.ReferenceTypeList = BuildReferenceTypeDropdownList();
            ViewBag.ProbabilityList = BuildProbabilityDropdownList();
            ViewBag.SeverityList = BuildSeverityDropdownList();

            return View(rv);
        }

        //
        // GET: /Requests/Search

        [HttpGet]
        [Authorize(Roles = Role.DI_SPECIALIST + ", " + Role.VIEWER)]
        public ActionResult Search()
        {
            SearchViewModel sm = new SearchViewModel();
            return View(sm);
        }

        //
        // POST: /Requests/Search

        [HttpPost]
        public ActionResult Search(SearchViewModel search)
        {
            ISet<Request> requests = new HashSet<Request>();
            // Split based on whitespace
            string[] keywords = search.Query.Split(null);
            IEnumerable<Keyword> matched = db.Keywords.Where(k => keywords.Contains(k.KeywordText))
                .Include(k => k.AssociatedQuestions.Select(c => c.Request)).ToList();
            foreach (var k in matched)
            {
                List<int> ints = k.AssociatedQuestions.Select(q => q.Request.ID).ToList();
                requests.UnionWith(ints.Select(id => FindRequest(id)));
            }
            //IEnumerable<Request> requests = db.Requests.Where(request => request.Patient.FirstName.Equals(search.Query));
            search.Results = new List<RequestViewModel>(requests.Select(r => RequestViewModel.CreateFromRequest(r)));
            search.Executed = true;
            return View(search);
        }

        //
        // GET: /Requests/ViewRequest/5

        // TODO Rename to View - seems redundant to have Requests/ViewRequest/5
        // ^ Don't do that, terrible, terrible, terrible things will happen.
        [HttpGet]
        [Authorize(Roles=Role.DI_SPECIALIST + ", " + Role.VIEWER)]
        public ActionResult ViewRequest(int id = 0)
        {
            Request request = FindRequest(id);
            ViewBag.QuestionTypeList = BuildQuestionTypeDropdownList();
            ViewBag.TumourGroupList = BuildTumourGroupDropdownList();
            ViewBag.ProbabilityList = BuildProbabilityDropdownList();
            ViewBag.SeverityList = BuildSeverityDropdownList();
            ViewBag.ReferenceTypeList = BuildReferenceTypeDropdownList();

            if (request == null)
            {
                return HttpNotFound();
            }

            if (isLocked(request))
            {
                return View();
            }

            RequestViewModel rv = RequestViewModel.CreateFromRequest(request);

            db.Logs.Add(Log.View(request, Membership.GetUser()));
            db.SaveChanges();
            return View(rv);
        }

        //
        // POST: /Requests/ViewRequest/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewRequest(RequestViewModel rv)
        {
            Request request = FindRequest(rv.RequestID);
            if (isLocked(request, true))
            {
                return View();
            }

            else if (request.Lock() != null)
            {
                return RedirectToAction("Edit", request.ID);
            }

            ModelState.AddModelError("", "View Request failed!");
            return View(rv);
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
        [Authorize(Roles = Role.DI_SPECIALIST)]
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

            PopulateDropDownLists(request.CompletionTime != null);

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

            // Why is this here? Of course the request is locked. How is someone editing it if it isn't?
            // TODO Make the whole request locking system work properly
            if (isLocked(request, true))
            {
                ViewBag.IsLocked = true;
                return View();
            }

            if (ModelState.IsValid)
            {
                rv.MapToRequest(request);
                rv.MapToRequestPatient(request);

                if (request.CompletionTime == null && rv.Status == "Closed")
                {
                    request.CompletionTime = DateTime.UtcNow;
                    request.ClosedBy = (Guid)Membership.GetUser().ProviderUserKey;
                    db.Logs.Add(Log.Close(request, Membership.GetUser()));
                }

                addQuestions(request, rv);
                
                request.Caller.Type = db.RequesterTypes.SingleOrDefault(rt => rt.ID == rv.RequesterTypeID);
                request.Caller.Region = db.Regions.SingleOrDefault(reg => reg.ID == rv.CallerRegionID);

                //SearchIndex.AddOrUpdateIndex(request);

                db.Logs.Add(Log.Edit(request, Membership.GetUser()));

                db.SaveChanges();

                //request.Unlock();
                return RedirectToAction("Index");
            }

            // TODO Add drop down lists to ViewBag
            return View(rv);
        }

        private void addQuestions(Request request, RequestViewModel rv)
        {
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

                if (question == null)
                    continue;

                qv.MapToQuestion(question);
                question.QuestionType = db.QuestionTypes.SingleOrDefault(qt => qt.ID == qv.QuestionTypeID);
                question.TumourGroup = db.TumourGroups.SingleOrDefault(tg => tg.ID == qv.TumourGroupID);

                question.Keywords.Clear();
                ISet<string> newKeywords = new HashSet<string>(qv.Keywords.Select(k => k.Keyword).ToArray());
                string[] keywordArray = newKeywords.ToArray();
                IEnumerable<Keyword> keywords = db.Keywords.Where(k => keywordArray.Contains(k.KeywordText));
                foreach (Keyword k in keywords)
                {
                    newKeywords.Remove(k.KeywordText);
                    question.Keywords.Add(k);
                }
                foreach (string s in newKeywords)
                {
                    Keyword k = db.Keywords.Create();
                    k.KeywordText = s;
                    k.Enabled = true;
                    question.Keywords.Add(k);
                }

                foreach (var refv in qv.References)
                {
                    Reference reference = null;
                    if (refv.ReferenceID != 0 && refv.Delete == false)
                    {
                        reference = question.References.SingleOrDefault(r => r.ID == refv.ReferenceID);
                    }
                    else if (refv.ReferenceID != 0 && refv.Delete == true)
                    {
                        reference = question.References.SingleOrDefault(r => r.ID == refv.ReferenceID);
                        question.References.Remove(reference);
                        db.References.Remove(reference);
                    }
                    else if (refv.ReferenceID == 0 && refv.Delete == false)
                    {
                        reference = db.References.Create();
                        question.References.Add(reference);
                    }

                    if (reference != null)
                    {
                        refv.MapToReference(reference);
                    }
                }
            }
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
        [Authorize(Roles = Role.DI_SPECIALIST)]
        [Authorize(Roles = Role.VIEWER)]
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
                
                Regex regexText2 = new Regex("REQUEST TYPE GOES HERE");
                if (req.Caller.Type != null)
                    docText = regexText2.Replace(docText, req.Caller.Type.FullName);
                else docText = regexText2.Replace(docText, "none");

                Regex regexText3 = new Regex("CALLERFIRSTNAME GOES HERE");
                if (req.Caller != null && req.Caller.FirstName != null)
                    docText = regexText3.Replace(docText, req.Caller.FirstName);
                else if (req.Caller == null) 
                    docText = regexText3.Replace(docText, "No caller data.");
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
                    docText = regexText8.Replace(docText, "Patient " + req.Patient.FirstName);
                else if (req.Patient == null)
                    docText = regexText3.Replace(docText, "No patient data.");
                else docText = regexText3.Replace(docText, "");

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
                if (req.Questions != null) 
                {
                    int qIndex = 0;
                    foreach (Question q in req.Questions)
                    {
                        wordDoc.MainDocumentPart.Document.Body.AppendChild(
                                new Paragraph(
                                    new Run(
                                        new Text(""))));
                        Paragraph para1 = wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph());
                        Run run1 = para1.AppendChild(new Run());
                        RunProperties runProperties1 = run1.AppendChild(new RunProperties());
                        Bold bold1 = new Bold();
                        bold1.Val = OnOffValue.FromBoolean(true);
                        runProperties1.AppendChild(bold1);
                        run1.AppendChild(new Text("Question" + qIndex));
                        Run run3 = para1.AppendChild(new Run(new Text(" - Probability " + q.Probability.ToString() + ", Severity " + q.Severity.ToString())));

                        if (q.QuestionType != null)
                        {
                            Paragraph para = wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph());
                            Run run = para.AppendChild(new Run());
                            RunProperties runProperties = run.AppendChild(new RunProperties());
                            Italic ital = new Italic();
                            ital.Val = OnOffValue.FromBoolean(true);
                            runProperties.AppendChild(ital);
                            run.AppendChild(new Text("Type"));
                            Run run2 = para.AppendChild(new Run(new Text(": " + q.QuestionType.FullName)));
                        }
                        if (q.TumourGroup != null)
                        {
                            Paragraph para = wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph());
                            Run run = para.AppendChild(new Run());
                            RunProperties runProperties = run.AppendChild(new RunProperties());
                            Italic ital = new Italic();
                            ital.Val = OnOffValue.FromBoolean(true);
                            runProperties.AppendChild(ital);
                            run.AppendChild(new Text("Tumour Group"));
                            Run run2 = para.AppendChild(new Run(new Text(": " + q.TumourGroup.FullName)));
                        }
                        if (q.Keywords != null)
                        {
                            String kWords = "";
                            int i = 0;
                            foreach (Keyword k in q.Keywords)
                            {
                                if (k.KeywordText != null && k.KeywordText.Trim() != "")
                                {
                                    if (i == 0) kWords = k.KeywordText;
                                    else kWords = kWords + ", " + k.KeywordText;
                                    i++;
                                }
                            }
                            Paragraph para = wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph());
                            Run run = para.AppendChild(new Run());
                            RunProperties runProperties = run.AppendChild(new RunProperties());
                            Italic ital = new Italic();
                            ital.Val = OnOffValue.FromBoolean(true);
                            runProperties.AppendChild(ital);
                            run.AppendChild(new Text("Key Words"));
                            Run run2 = para.AppendChild(new Run(new Text(": " + kWords)));
                        }
                        if (q.QuestionText != null)
                        {
                            wordDoc.MainDocumentPart.Document.Body.AppendChild(
                                new Paragraph(
                                    new Run(
                                        new Text(""))));
                            wordDoc.MainDocumentPart.Document.Body.AppendChild(
                                new Paragraph(
                                    new Run(
                                        new Text(q.QuestionText))));
                        }
                        if (q.Response != null)
                        {
                            wordDoc.MainDocumentPart.Document.Body.AppendChild(
                                   new Paragraph(
                                       new Run(
                                           new Text(""))));

                            Paragraph para = wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph());
                            Run run = para.AppendChild(new Run());
                            RunProperties runProperties = run.AppendChild(new RunProperties());
                            Bold bold = new Bold();
                            bold.Val = OnOffValue.FromBoolean(true);
                            runProperties.AppendChild(bold);
                            run.AppendChild(new Text("Response" + qIndex));
                            wordDoc.MainDocumentPart.Document.Body.AppendChild(
                                new Paragraph(
                                    new Run(
                                        new Text(q.Response))));
                        }
                        if (q.SpecialNotes != null)
                        {
                            wordDoc.MainDocumentPart.Document.Body.AppendChild(
                                   new Paragraph(
                                       new Run(
                                           new Text(""))));
                            Paragraph para = wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph());
                            Run run = para.AppendChild(new Run());
                            RunProperties runProperties = run.AppendChild(new RunProperties());
                            Bold bold = new Bold();
                            bold.Val = OnOffValue.FromBoolean(true);
                            runProperties.AppendChild(bold);
                            run.AppendChild(new Text("Special Notes" + qIndex));
                            wordDoc.MainDocumentPart.Document.Body.AppendChild(
                                new Paragraph(
                                    new Run(
                                        new Text(q.SpecialNotes))));
                        }
                        if (q.References != null)
                        {
                            wordDoc.MainDocumentPart.Document.Body.AppendChild(
                                   new Paragraph(
                                       new Run(
                                           new Text(""))));
                            Paragraph para = wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph());
                            Run run = para.AppendChild(new Run());
                            RunProperties runProperties = run.AppendChild(new RunProperties());
                            Bold bold = new Bold();
                            bold.Val = OnOffValue.FromBoolean(true);
                            runProperties.AppendChild(bold);
                            run.AppendChild(new Text("References: "));
                            int i = 1;
                            foreach (Reference r in q.References)
                            {
                                if (r.Value != null && r.Value.Trim() != "")
                                {
                                    wordDoc.MainDocumentPart.Document.Body.AppendChild(
                                        new Paragraph(
                                            new Run(
                                                new Text(i + ". " + r.Value))));
                                    i++;
                                }
                            }
                        }
                        qIndex++;
                    }
                }

            }
            return mem;
        }

        #region Helper Methods

        private Request FindRequest(int id)
        {
            return db.Requests.Include(r => r.Caller)
                              .Include(r => r.Patient)
                              .Include(r => r.Caller.Type)
                              .Include(r => r.Caller.Region)
                              .Include(p => p.Questions.Select(c => c.Keywords))
                              .Include(p => p.Questions.Select(c => c.References))
                              .Include(p => p.Questions.Select(c => c.QuestionType))
                              .Include(p => p.Questions.Select(c => c.TumourGroup))
                              .Single(r => r.ID == id && r.Enabled == true);
        }

        #region DropDown Lists
        private void PopulateDropDownLists(bool closed, bool questionDropDowns = true)
        {
            ViewBag.RequesterTypeList = BuildRequesterTypeDropdownList();
            ViewBag.RegionList = BuildRegionDropdownList();
            ViewBag.GenderList = BuildGenderDropdownList();
            ViewBag.StatusList = BuildStatusDropdownList(closed);

            if (questionDropDowns)
            {
                ViewBag.QuestionTypeList = BuildQuestionTypeDropdownList();
                ViewBag.TumourGroupList = BuildTumourGroupDropdownList();
                ViewBag.ReferenceTypeList = BuildReferenceTypeDropdownList();
                ViewBag.ProbabilityList = BuildProbabilityDropdownList();
                ViewBag.SeverityList = BuildSeverityDropdownList();
            }
        }

        private IEnumerable<SelectListItem> BuildGenderDropdownList()
        {
            List<SelectListItem> genders = new List<SelectListItem>();

            genders.Add(new SelectListItem { Text = "Unknown", Value = Gender.UNKNOWN.ToString() });
            genders.Add(new SelectListItem { Text = "Male", Value = Gender.MALE.ToString() });
            genders.Add(new SelectListItem { Text = "Female", Value = Gender.FEMALE.ToString() });

            return genders;
        }

        private IEnumerable<SelectListItem> BuildReferenceTypeDropdownList()
        {
            List<SelectListItem> refTypes = new List<SelectListItem>();

            refTypes.Add(new SelectListItem { Text = "Text", Value = ReferenceType.TEXT.ToString() });
            refTypes.Add(new SelectListItem { Text = "URL", Value = ReferenceType.URL.ToString() });
            refTypes.Add(new SelectListItem { Text = "Request", Value = ReferenceType.REQUEST.ToString() });
            refTypes.Add(new SelectListItem { Text = "File", Value = ReferenceType.FILE.ToString() });

            return refTypes;
        }

        private IEnumerable<SelectListItem> BuildProbabilityDropdownList()
        {
            List<SelectListItem> selectItems = new List<SelectListItem>();

            selectItems.Add(new SelectListItem { Text = "Possible", Value = Probability.Possible.ToString() });
            selectItems.Add(new SelectListItem { Text = "Probable", Value = Probability.Probable.ToString() });
            selectItems.Add(new SelectListItem { Text = "Unlikely", Value = Probability.Unlikely.ToString() });

            return selectItems;
        }

        private IEnumerable<SelectListItem> BuildSeverityDropdownList()
        {
            List<SelectListItem> selectItems = new List<SelectListItem>();

            selectItems.Add(new SelectListItem { Text = "Major", Value = Severity.Major.ToString() });
            selectItems.Add(new SelectListItem { Text = "Moderate", Value = Severity.Moderate.ToString() });
            selectItems.Add(new SelectListItem { Text = "Minor", Value = Severity.Minor.ToString() });

            return selectItems;
        }

        private IEnumerable<SelectListItem> BuildStatusDropdownList(bool closed)
        {
            List<SelectListItem> selectItems = new List<SelectListItem>();

            if (!closed)
                selectItems.Add(new SelectListItem { Text = "In Progress", Value = "Open" });
            selectItems.Add(new SelectListItem { Text = "Closed", Value = "Closed" });

            return selectItems;
        }

        private IEnumerable<SelectListItem> BuildRequesterTypeDropdownList()
        {
            return BuildTypedDropdownList<RequesterType>(db.RequesterTypes);
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
            DREAM.Models.Lock requestLock = findRequestLock(request.ID);
            MembershipUser lockingUser = getUserFromLock(requestLock);
            MembershipUser currentUser = Membership.GetUser();

            return requestLock != null && currentUser.UserName != lockingUser.UserName;
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
            return db.Locks.SingleOrDefault(l => l.RequestID == requestID);
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
            return requestLock != null ? Membership.GetUser(requestLock.UserID) : null;
        }

        // Find and return the username belonging to the given user ID.
        //
        // Arguments:
        //      userID -- The user ID.
        //
        // Returns:
        //      The username of the given user ID or null if none could be found.
        public string FindUsernameFromID(Guid userID)
        {
            MembershipUser user = Membership.GetUser(userID);

            return user != null ? user.UserName : null;
        }

        #endregion
    }
}
﻿