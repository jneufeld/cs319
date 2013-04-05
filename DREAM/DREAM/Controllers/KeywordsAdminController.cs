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
    [Authorize(Roles=Role.ADMIN)]
    public class KeywordsAdminController : Controller
    {
        private DREAMContext db = new DREAMContext();

        /// <summary> Index of the Keyword Admin page; this is the default page shown when accessing Keyword Admin </summary>
        /// <param name="page"> This is the optional page GET parameter which indicates the current page (defaults to 1)</param>
        /// <returns> The view of Keyword Admin where all keywords currently existing in the system are displayed </returns>
        [HttpGet]
        public ActionResult Index(int page = 1) {
           DbSet keywords = db.Keywords;

            ViewBag.KeywordAdminActive = true;
            return View(keywords);  
        }

        /// <summary> To edit a keyword </summary>
        /// <param name="keywordText"> The keyword that needs to be edited </param>
        /// <returns> The view for editing a keyword </returns>
        [HttpGet]
	    public ActionResult Edit(String keywordText) {
            Keyword keyword = findKeywordFromText(keywordText);

            if (keyword == null)
            {
                ModelState.AddModelError("", "The keyword cannot be found.");
            }

            ViewBag.KeywordAdminActive = true;
            return View(keyword); 
        }

        /// <summary> Helper method to find the keyword object based on the keyword text </summary>
        /// <param name="keywordText"> The keyword text of the keyword that needs to be found </param>
        /// <returns> The keyword with the keyword text of value keywordText(parameter) </returns>
        private Keyword findKeywordFromText(String keywordText)
        {
            Keyword keywordToReturn = null;

            if (keywordText != null && keywordText.Trim() != null)
            {
                DbSet<Keyword> allKeywords = db.Keywords;
                bool foundKeyword = false;
                String keywordTextLowerCase = keywordText.ToLower();

                foreach (Keyword k in allKeywords)
                {
                    String curKeywordLowerCase = "";

                    if (k.KeywordText != null && k.KeywordText.Trim() != null && !foundKeyword)
                    {
                        curKeywordLowerCase = k.KeywordText.ToLower();

                        if (curKeywordLowerCase.Equals(keywordTextLowerCase)) 
                        {
                            keywordToReturn = k;
                            foundKeyword = true;
                        }
                    }
                }
            }

            return keywordToReturn;
        }

        /// <summary> Helper method to remove all of the references to the old keyword and change it to all the references to the new keyword </summary>
        /// <param name="newKeyword"> The new keyword that we want the references to point to </param>
        /// <param name="oldKeyword"> The old keyword that we want to remove all references to </param>
        /// <returns> None </returns>
        private void removeOldKeywordFromQuestions(Keyword newKeyword, Keyword oldKeyword)
        {
            DbSet<Question> allQuestions = db.Questions;
            String newKeywordTestLowerCase = newKeyword.KeywordText.ToLower();

            foreach (Question q in allQuestions)
            {
                List<Keyword> keywordsInQuestion = q.Keywords;

                foreach (Keyword k in keywordsInQuestion)
                {
                    String lowerCaseCurKeyword = k.KeywordText.ToLower();

                    if (k.KeywordText.Equals(newKeywordTestLowerCase))
                    {
                        q.Keywords.Remove(oldKeyword);
                        q.Keywords.Add(newKeyword);
                        break;
                    }
                }
            }
        }


        /// <summary> The actual logic that implements editing a keyword </summary>
        /// <param name="model"> The keyword that needs to be modified with keyword text variable be the new keyword (but the id of the keyword will stay the same) </param>>
        /// <returns> The main Keyword Admin page on success and stays on the same page upon error </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Keyword model){
            ViewBag.KeywordAdminActive = true;
            if (ModelState.IsValid)
            {
                if (model.KeywordText != null)
                {
                    if (model.KeywordText.Trim() != "")
                    {
                        Keyword oldKeyword = db.Keywords.Find(model.ID);
                        String modelKeywordTextLowerCase = model.KeywordText.ToLower();
                        Keyword versionOfNewKeywordInDB = findKeywordFromText(model.KeywordText);

                        if (oldKeyword.KeywordText.ToLower().Equals(modelKeywordTextLowerCase))
                        {
                            oldKeyword.KeywordText = model.KeywordText;
                        }
                        else
                        {

                            if (versionOfNewKeywordInDB != null)
                            {
                                removeOldKeywordFromQuestions(versionOfNewKeywordInDB, oldKeyword);
                                db.Keywords.Remove(oldKeyword);
                            }
                            else
                            {
                                oldKeyword.KeywordText = model.KeywordText;
                            }
                        }
                        

                        db.SaveChanges();

                        RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
                        routeValueDictionary.Add("page", 1);
                        return RedirectToAction("Index", "KeywordsAdmin", routeValueDictionary);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Keyword needs characters other than space(s).");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Keyword cannot be empty.");
                    return View(model);
                }
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
