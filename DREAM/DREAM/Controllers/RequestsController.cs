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
            return View();
        }

        //
        // POST: /Requests/Add

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Request request)
        {
            if (ModelState.IsValid)
            {
                request.CreationTime = DateTime.Now;
                request.CompletionTime = null;
                request.CreatedBy = (Guid)Membership.GetUser().ProviderUserKey;

                db.Requests.Add(request);
                db.Logs.Add(Log.Create(request, Membership.GetUser()));
                db.SaveChanges();

                return RedirectToAction("Add", request);
            }
            else
            {
                ModelState.AddModelError("", "Add request failed!");
            }

            return View(request);
        }

        //
        // GET: /Requests/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
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
            else if (isEditing)
            {
                ViewBag.NoLock = true;
                returnValue = true;
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
    }
}