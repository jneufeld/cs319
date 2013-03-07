using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        // GET: /Requests/Details/5

        public ActionResult Details(int id = 0)
        {
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        //
        // GET: /Requests/Add

        public ActionResult Add()
        {
            ViewBag.RequestTypeList = BuildRequestTypeDropdownList();
            ViewBag.RegionList = BuildRegionDropdownList();
            ViewBag.GenderList = BuildGenderDropdownList();
            return View();
        }

        //
        // POST: /Requests/Add

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Request request)
        {
            ViewBag.RequestTypeList = BuildRequestTypeDropdownList();
            ViewBag.RegionList = BuildRegionDropdownList();
            ViewBag.GenderList = BuildGenderDropdownList();

            if (ModelState.IsValid)
            {
                request.CreationTime = DateTime.Now;
                request.CompletionTime = null;
                request.CreatedBy = (Guid)Membership.GetUser().ProviderUserKey;
                request.ClosedBy = Guid.Empty;
                request.Caller.RequestID = request.ID;

                db.Requests.Add(request);
                db.Logs.Add(Log.Create(request, Membership.GetUser()));
                db.SaveChanges();

                return RedirectToAction("Add", request.ID);
            }
            else
            {
                ModelState.AddModelError(ModelState.ToString(), "The add request failed!");
            }

            return View(request);
        }

        //
        // GET: /Requests/ViewRequest/5

        public ActionResult ViewRequest(int id = 0)
        {
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }

            if (isLocked(request))
            {
                return View();
            }

            Log.Create(request, Membership.GetUser());
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

            ViewBag.Questions = new List<Question>(request.Questions);

            ViewBag.RequestTypeList = BuildRequestTypeDropdownList();
            ViewBag.RegionList = BuildRegionDropdownList();
            ViewBag.GenderList = BuildGenderDropdownList();

            ViewBag.CreatedByUsername = FindUsernameFromID(request.CreatedBy);
            ViewBag.ClosedByUsername = FindUsernameFromID(request.ClosedBy);

            return View(request);
        }

        //
        // POST: /Requests/Edit/5

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

        public IEnumerable<SelectListItem> BuildGenderDropdownList()
        {
            List<SelectListItem> requestTypes = new List<SelectListItem>();

            requestTypes.Add(new SelectListItem { Text = "Male", Value = Gender.MALE.ToString() });
            requestTypes.Add(new SelectListItem { Text = "Female", Value = Gender.FEMALE.ToString() });
            requestTypes.Add(new SelectListItem { Text = "Unknown", Value = Gender.UNKNOWN.ToString() });

            return requestTypes;
        }

        public IEnumerable<SelectListItem> BuildRequestTypeDropdownList()
        {
            return BuildTypedDropdownList<RequestType>(db.RequestTypes);
        }

        public IEnumerable<SelectListItem> BuildRegionDropdownList()
        {
            return BuildTypedDropdownList<Region>(db.Regions);
        }

        public IEnumerable<SelectListItem> BuildTypedDropdownList<T>(DbSet<T> dbSet) where T : DropDown
        {
            IEnumerable<T> ts = dbSet.AsEnumerable<T>();
            IEnumerable<SelectListItem> list = ts.Select(r => new SelectListItem
            {
                Value = r.ID.ToString(),
                Text = r.FullName,
            });
            return list;
        }

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
        private String FindUsernameFromID(Guid userID)
        {
            String returnValue;
            MembershipUser user = Membership.GetUser(userID);

            if (user == null)
            {
                returnValue = null;
            }
            else
            {
                returnValue = user.UserName;
            }

            return returnValue;
        }
    }
}