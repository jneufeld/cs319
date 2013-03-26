using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Routing;
using DREAM.Models;

namespace DREAM.Controllers
{
   [Authorize(Roles="ADMIN")]
    public class KeywordsAdminController : Controller
    {
        private DREAMContext db = new DREAMContext();

        [HttpGet]
        public ActionResult Index(int page = 1) {
           DbSet keywords = db.Keywords;

            return View(keywords);  
        }

        [HttpGet]
	    public ActionResult Edit(int id) {
            Keyword keyword = db.Keywords.Find(id);

            if (keyword == null)
            {
                ModelState.AddModelError("", "The keyword cannot be found.");
            }

            return View(keyword); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // parameter keyword = old one updated with the changes
        public ActionResult Edit(Keyword model, Keyword keyword){
            if (ModelState.IsValid)
            {
                Keyword currKeywordToChange = db.Keywords.Find(keyword.KeywordText);
                if (currKeywordToChange != null)
                {
                    List<Question> questionsReferencingOldKeyword = keyword.AssociatedQuestions;

                    for (int i = 0; i < questionsReferencingOldKeyword.Count; i++)
                    {
                        List<Keyword> keywordsInCurrQuestion = questionsReferencingOldKeyword[i].Keywords;

                        foreach (Keyword curKeyword in keywordsInCurrQuestion)
                        {
                            if (keywordsInCurrQuestion[i].ID == keyword.ID)
                            {
                                keywordsInCurrQuestion[i].KeywordText = keyword.KeywordText;
                            }
                        }
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Edit", currKeywordToChange.KeywordText);
            }
            else
            {
                ModelState.AddModelError(keyword.KeywordText, "Keyword cannot be edited.");
                return View(keyword);
            }
        }
       
       [HttpGet]
       public ActionResult ChangeKeywordStatus(int keywordID)
        {
           
            Keyword keywordToWorkWith = db.Keywords.Find(keywordID);

            if (keywordToWorkWith != null)
            {
                keywordToWorkWith.Enabled = !keywordToWorkWith.Enabled;
                db.SaveChanges();
            }
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
                routeValueDictionary.Add("page", 1);
                return RedirectToAction("Index", "KeywordsAdmin", routeValueDictionary);
        }
    }
}
