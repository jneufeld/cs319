using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
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

        public ActionResult Add()
        {
            Request request = new Request();
            request.Caller = new Caller();
            request.Patient = new Patient();
            RequestViewModel rv = RequestViewModel.CreateFromRequest(request);
            ViewBag.RequestTypeList = BuildRequestTypeDropdownList();
            ViewBag.RegionList = BuildRegionDropdownList();
            ViewBag.GenderList = BuildGenderDropdownList();
            rv.RequestTypeDropDownList = BuildRequestTypeDropdownList();
            rv.RegionDropDownList = BuildRegionDropdownList();
            rv.GenderDropDownList = BuildGenderDropdownList();
            return View(rv);
        }

        //
        // POST: /Requests/Add

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(RequestViewModel rv)
        {
            ViewBag.RequestTypeList = BuildRequestTypeDropdownList();
            ViewBag.RegionList = BuildRegionDropdownList();
            ViewBag.GenderList = BuildGenderDropdownList();

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

                request.Type = db.RequestTypes.Single(rt => rt.ID == request.Type.ID);
                request.Caller.Region = db.Regions.Single(reg => reg.ID == request.Caller.Region.ID);

                request.CreationTime = DateTime.UtcNow;
                request.CompletionTime = null;
                request.CreatedBy = (Guid) Membership.GetUser().ProviderUserKey;
                request.ClosedBy = Guid.Empty;

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

            return View(rv);
        }

        //
        // GET: /Requests/ViewRequest/5

        public ActionResult ViewRequest(int id = 0)
        {
            Request request = FindRequest(id);
            if (request == null)
            {
                return HttpNotFound();
            }

            if (isLocked(request))
            {
                return View();
            }

            Log.View(request, Membership.GetUser());
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

            // Need Lock() method in Request for this check to work
            //else if (request.Lock())
            //{
            return RedirectToAction("Edit", request.ID);
            //}

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
        public ActionResult Edit(int id = 0)
        {
            Request request = FindRequest(id);
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

            ViewBag.Questions = new List<Question>(request.Questions);

            ViewBag.RequestTypeList = BuildRequestTypeDropdownList();
            ViewBag.RegionList = BuildRegionDropdownList();
            ViewBag.QuestionTypeList = BuildQuestionTypeDropdownList();
            ViewBag.TumourGroupList = BuildTumourGroupDropdownList();
            ViewBag.GenderList = BuildGenderDropdownList();

            ViewBag.CreatedByUsername = FindUsernameFromID(request.CreatedBy);
            ViewBag.ClosedByUsername = FindUsernameFromID(request.ClosedBy);

            return View(request);
        }

        //
        // POST: /Requests/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            Request request = db.Requests.Find(id);
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

            //db.Requests.Attach(request);
            //db.Entry(request).State = EntityState.Modified;
            //TryUpdateModel<Request>(request, formCollection;

            if (ModelState.IsValid)
            {
                UpdateModel<Request>(request, formCollection);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            //request.Unlock();
            return View(request);
        }

        /*
        
        Old version...
        
        [HttpPost]
        public ActionResult Edit(Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(request);
        }
        
         */

        //
        // GET: /Requests/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Request request = db.Requests.Find(id);
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

        #region Helper Methods

        private Request FindRequest(int id)
        {
            return db.Requests.Include(r => r.Caller)
                              .Include(r => r.Patient)
                              .Include(r => r.Type)
                              .Include(r => r.Caller.Region)
                              .Include(p => p.Questions.Select(c => c.Keywords))
                              .Include(p => p.Questions.Select(c => c.Reference))
                              .Single(r => r.ID == id);
        }

        #region DropDown Lists
        private IEnumerable<SelectListItem> BuildGenderDropdownList()
        {
            List<SelectListItem> requestTypes = new List<SelectListItem>();

            requestTypes.Add(new SelectListItem { Text = "Male", Value = Gender.MALE.ToString() });
            requestTypes.Add(new SelectListItem { Text = "Female", Value = Gender.FEMALE.ToString() });
            requestTypes.Add(new SelectListItem { Text = "Unknown", Value = Gender.UNKNOWN.ToString() });

            return requestTypes;
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
                Value = r.StringID,
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
            Lock requestLock = findRequestLock(request.ID);
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
        private Lock findRequestLock(int requestID)
        {
            Lock returnValue = null;

            List<Lock> locks = db.Locks.ToList();
            foreach (Lock requestLock in locks)
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
        private MembershipUser getUserFromLock(Lock requestLock)
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
