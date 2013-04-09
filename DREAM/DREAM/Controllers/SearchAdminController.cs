using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DREAM.Models;
using Lucene.Net.Store;

namespace DREAM.Controllers
{
    [Authorize(Roles = Role.ADMIN)]
    public class SearchAdminController : Controller
    {
        private DREAMContext db = new DREAMContext();

        //
        // GET: /SearchAdmin/

        [HttpGet]
        public ActionResult Index()
        {
            SearchAdminViewModel svm = new SearchAdminViewModel();
            ViewBag.SearchAdminActive = true;
            return View(svm);
        }

        [HttpPost]
        public ActionResult Index(SearchAdminViewModel svm)
        {
            var messages = new List<MsgViewModel>();

            if (svm.Action == "Requests")
            {
                using (var searchIndex = new SearchIndex<Request, RequestIndexDefinition>())
                {
                    searchIndex.ClearLuceneIndex();
                    searchIndex.AddOrUpdateAll(GetAllRequests());
                    messages.Add(MsgViewModel.SuccessMsg("Index has been rebuilt."));
                }
            }
            /* else if (svm.Action == "Autocomplete")
            {
                using (FSDirectory d = FSDirectory.Open(new DirectoryInfo(SearchIndex<Request, RequestIndexDefinition>.DirPath)))
                {
                    SearchAutoComplete sac = new SearchAutoComplete(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/SearchAutocompleteIndex");
                    sac.BuildAutoCompleteIndex(d, "Keywords");
                }
                messages.Add(MsgViewModel.SuccessMsg("Rebuilt autocomplete index."));
            } */

            ViewBag.Alerts = messages;

            ViewBag.SearchAdminActive = true;
            return View(svm);
        }

        private IEnumerable<Request> GetAllRequests()
        {
            return db.Requests.Include(r => r.Caller)
                              .Include(r => r.Patient)
                              .Include(r => r.Caller.Type)
                              .Include(r => r.Caller.Region)
                              .Include(p => p.Questions.Select(c => c.Keywords))
                              .Include(p => p.Questions.Select(c => c.References))
                              .Include(p => p.Questions.Select(c => c.QuestionType))
                              .Include(p => p.Questions.Select(c => c.TumourGroup))
                              .Where(r => r.Enabled);
        }
    }
}
