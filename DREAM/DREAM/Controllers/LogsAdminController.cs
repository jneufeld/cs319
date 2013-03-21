using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DREAM.Models;

namespace DREAM.Controllers
{
    public class LogsAdminController : Controller
    {
        private DREAMContext db = new DREAMContext();

        //
        // GET: /LogsAdmin/

        public ActionResult Index(Guid? user, int? request, LogAction? action, DateTime? before, DateTime? after, int page = 1)
        {
            var logs = db.Logs.OrderByDescending(m => m.ID).ToPagedList(page, 2);
            ViewBag.logsPage = logs;
            return View(logs);
        }
    }
}