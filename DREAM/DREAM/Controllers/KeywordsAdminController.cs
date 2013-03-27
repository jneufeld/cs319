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
	    public ActionResult Edit(String keywordText) {
            Keyword keyword = findKeywordFromText(keywordText);

            if (keyword == null)
            {
                ModelState.AddModelError("", "The keyword cannot be found.");
            }

            return View(keyword); 
        }

       
        private Keyword findKeywordFromText(String keywordText)
        {
            DbSet<Keyword> allKeywords = db.Keywords;
            Keyword keywordToReturn = null;

            foreach (Keyword k in allKeywords)
            {
                if (k.KeywordText.Equals(keywordText))
                {
                    keywordToReturn = k;
                }
            }

            return keywordToReturn;
        }

        private List<Question> allQuestionsWithKeyword(Keyword keyword)
        {
            DbSet<Question> questions = db.Questions;
            bool keywordFoundInQuestion=false;

            List<Question> questionsThatHaveKeyword = null;

            foreach (Question q in questions)
            {
                List<Keyword> qKeywords = q.Keywords;

                foreach (Keyword k in qKeywords)
                {
                    if (k.KeywordText.Equals(keyword.KeywordText) && !keywordFoundInQuestion)
                    {
                        questionsThatHaveKeyword.Add(q);
                        keywordFoundInQuestion = true;
                    }

                }
                keywordFoundInQuestion = false;
            }

            return questionsThatHaveKeyword;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        // parameter keyword = old one updated with the changes
        public ActionResult Edit(Keyword model){
            if (ModelState.IsValid)
            {
               Keyword oldKeyword = db.Keywords.Find(model.ID);
               Keyword versionOfNewKeywordInDB = findKeywordFromText(model.KeywordText);

               if (versionOfNewKeywordInDB != null)
               {
                   List<Question> questionsWithKeyword = allQuestionsWithKeyword(oldKeyword);

                   foreach (Question q in questionsWithKeyword)
                   {
                       q.Keywords.Remove(oldKeyword);
                       q.Keywords.Add(model);
                   }
               }
               else
               {
                   oldKeyword.KeywordText = model.KeywordText;
               }

               db.SaveChanges();

                RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
                routeValueDictionary.Add("page", 1);
                return RedirectToAction("Index", "KeywordsAdmin", routeValueDictionary);
            }
            else
            {
                ModelState.AddModelError(model.KeywordText, "Keyword cannot be edited.");
                return View(model);
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
