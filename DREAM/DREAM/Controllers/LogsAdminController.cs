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

        public ActionResult Index(int? request = null, String act = null, String user = null, DateTime? before = null, DateTime? after = null, int page = 1)
        {
            LogFilterModel lfm = new LogFilterModel(request, user, act, before, after, page);
            ViewBag.ActionList = BuildActionDropdownList();
            lfm.filter();
            return View(lfm);
        }

        [HttpPost]
        public ActionResult Index(String button, LogFilterModel lfm)
        {
            ViewBag.ActionList = BuildActionDropdownList();

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
                filter.PagedLogs = filter.Logs.OrderByDescending(m => m.ID).ToPagedList(1, 10);
                return View(filter);
            }
            else
                return View(lfm);
            
        }

        private IEnumerable<SelectListItem> BuildActionDropdownList()
        {
            List<SelectListItem> actions = new List<SelectListItem>();
            actions.Add(new SelectListItem
            {
                Value = "",
                Text = "All Actions",
            });
            actions.Add(new SelectListItem
            {
                Value = "0",
                Text = "CREATE",
            });
            actions.Add(new SelectListItem
            {
                Value = "1",
                Text = "EDIT",
            });
            actions.Add(new SelectListItem
            {
                Value = "2",
                Text = "CLOSE",
            });
            actions.Add(new SelectListItem
            {
                Value = "3",
                Text = "VIEW",
            });

            return actions;
        }
    }
}