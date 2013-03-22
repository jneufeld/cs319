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

        public ActionResult Index(String request = "null", int page = 1)
        {
            LogFilterModel lfm = new LogFilterModel();

            ContentResult req = new ContentResult { Content = request, ContentType = "application/json" };
            if (!req.Content.Equals("null"))
            {
                lfm.RequestID = Convert.ToInt32(req.Content);
                lfm.Logs = lfm.Logs.Where(log => log.RequestID == lfm.RequestID);
            }
            lfm.PagedLogs = lfm.Logs.OrderByDescending(m => m.ID).ToPagedList(page, 2);
            return View(lfm);
        }

        [HttpPost]
        public ActionResult Index(String button, LogFilterModel lfm)
        {
            if (button.Equals("Filter"))
            {
                LogFilterModel filter = lfm;

                if (!filter.RequestID.Equals(null))
                    filter.Logs = filter.Logs.Where(log => log.RequestID == filter.RequestID);

                filter.PagedLogs = filter.Logs.OrderByDescending(m => m.ID).ToPagedList(1, 2);
                ViewBag.logsPage = filter.PagedLogs;
                ViewBag.request = filter.RequestID;
                return View(filter);
            }
            else
                return View(lfm);
        }
    }
}