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

        /// <summary> Index of the Keyword Admin page; this is the default page shown when accessing Keyword Admin </summary>
        /// <param name="page"> </param>
        /// <returns> The view of Keyword Admin where all keywords currently existing in the system are displayed </returns>
        [HttpGet]
        public ActionResult Index(int page = 1) {
           DbSet keywords = db.Keywords;

            return View(keywords);  
        }

        /// <summary> To edit a keyword </summary>
        /// <param name="keywordText"> The keyword that needs to be edited </param>>
        /// <returns> The view for editing a keyword </returns>
        [HttpGet]
	    public ActionResult Edit(String keywordText) {
            Keyword keyword = findKeywordFromText(keywordText);

            if (keyword == null)
            {
                ModelState.AddModelError("", "The keyword cannot be found.");
            }

            return View(keyword); 
        }

        /// <summary> Helper method to find the keyword object based on the keyword text </summary>
        /// <param name="keywordText"> The keyword text of the keyword that needs to be found </param>>
        /// <returns> The keyword with the keyword text of value keywordText(parameter) </returns>
        private Keyword findKeywordFromText(String keywordText)
        {
            DbSet<Keyword> allKeywords = db.Keywords;
            Keyword keywordToReturn = null;
            bool foundKeyword = false;

            foreach (Keyword k in allKeywords)
            {
                if (k.KeywordText.Equals(keywordText) && !foundKeyword)
                {
                    keywordToReturn = k;
                    foundKeyword = true;
                }
            }

            return keywordToReturn;
        }

        /// <summary> A helper method to find all questions with the given keyword </summary>
        /// <param name="keyword"> The keyword that we want to find questions for </param>>
        /// <returns> A list of questions that contain the given keyword </returns>
        private List<Question> allQuestionsWithKeyword(Keyword keyword)
        {
            DbSet<Question> questions = db.Questions;
            List<Question> questionsThatHaveKeyword = null;

            foreach (Question q in questions)
            {
                List<Keyword> qKeywords = q.Keywords;

                foreach (Keyword k in qKeywords)
                {
                    if ((k.KeywordText).Equals(keyword.KeywordText))
                    {
                        questionsThatHaveKeyword.Add(q);
                    }

                }
            }

            return questionsThatHaveKeyword;
        }

        /// <summary> To actual logic that implements editing a keyword </summary>
        /// <param name="model"> The keyword that needs to be modified with keyword text variable be the new keyword (but the id of the keyword will stay the same) </param>>
        /// <returns> The main Keyword Admin page on success and stays on the same page upon error </returns>
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
                   DbSet<Question> allQuestions = db.Questions;

                   foreach (Question q in allQuestions)
                   {
                       if (q.Keywords.Contains(oldKeyword))
                       {
                           q.Keywords.Remove(oldKeyword);
                           q.Keywords.Add(versionOfNewKeywordInDB);
                       }
                   }

                   db.Keywords.Remove(oldKeyword);
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

        /// <summary> To change the state of a keyword from enabled to disabled or vice versa </summary>
        /// <param name="keywordID"> The unique ID number of the keyword that needs to be enabled/disabled </param>>
        /// <returns>  The main Keyword Admin page </returns>
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
