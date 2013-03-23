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

        public ActionResult Index(int? request = null, int? act = null, String user = null, DateTime? before = null, DateTime? after = null, int page = 1)
        {
            LogFilterModel lfm = new LogFilterModel(request, user, act, before, after, page);
            lfm.filter();
            return View(lfm);
        }

        [HttpPost]
        public ActionResult Index(String button, LogFilterModel lfm)
        {
            if (button.Equals("Filter"))
            {
                LogFilterModel filter = lfm;
                filter.page = 1;
                filter.filter();
                return View(filter);
            }

            else if (button.Equals("Clear"))
            {
                ModelState.Clear();
                LogFilterModel filter = new LogFilterModel();
                filter.PagedLogs = filter.Logs.OrderByDescending(m => m.ID).ToPagedList(1, 2);
                return View(filter);
            }
            else
                return View(lfm);
        }
    }
}