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
    }
}