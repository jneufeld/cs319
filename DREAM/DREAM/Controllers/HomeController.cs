using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DREAM.Models;

namespace DREAM.Controllers
{
    public class HomeController : Controller
    {
        private DREAMContext db = new DREAMContext();

        public ActionResult Index()
        {
            if (!IsCurrentUserAuthorized())
            {
                ViewBag.Message = "Please login to start using the system.";
                return View("GuestIndex");
            }

            HomeViewModel hv = new HomeViewModel();

            hv.OpenRequests = db.Requests
                .Where(r => r.CompletionTime == null)
                .OrderByDescending(r => r.CreationTime)
                .Take(10)
                .Include(r => r.Caller)
                .Include(r => r.Patient)
                .Include(r => r.Caller.Type)
                .Include(r => r.Caller.Region)
                .ToList()
                .Select(r => RequestViewModel.CreateFromRequest(r))
                .ToList();

            hv.RecentRequests = db.Requests
                .OrderByDescending(r => r.CreationTime)
                .Take(10)
                .Include(r => r.Caller)
                .Include(r => r.Patient)
                .Include(r => r.Caller.Type)
                .Include(r => r.Caller.Region)
                .ToList()
                .Select(r => RequestViewModel.CreateFromRequest(r))
                .ToList();

            if (User.IsInRole(Role.VIEWER) && !User.IsInRole(Role.DI_SPECIALIST))
            {
                hv.RecentRequests = hv.RecentRequests.Where(rv => !String.IsNullOrEmpty(rv.CompletionTime)).ToList();
            }

            ViewBag.Message = "Home Page";
            return View(hv);
        }

        public ActionResult About()
        {
            ViewBag.Message = "DREAM";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page";

            return View();
        }

        [Authorize(Roles=Role.ADMIN)]
        public ActionResult Admin()
        {
            ViewBag.Message = "Administration Panel";

            return View();
        }

        public ActionResult PageNotFound(String errorPath)
        {
            ViewBag.Message = "Sorry, the page you requested does not exist.";

            return View();
        }

        public ActionResult InvalidPermissions(String errorPath)
        {
            ViewBag.Message = "You do not have permission to view this page.";

            return View();
        }

        private bool IsCurrentUserAuthorized()
        {
            return User.IsInRole(Role.ADMIN) ||
                   User.IsInRole(Role.REPORTER) ||
                   User.IsInRole(Role.DI_SPECIALIST) ||
                   User.IsInRole(Role.VIEWER);
        }
    }
}
