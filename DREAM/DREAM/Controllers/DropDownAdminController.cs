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
    public class DropDownAdminController : Controller
    {
        private DREAMContext db = new DREAMContext();

        [HttpGet]
        public ActionResult Index(string dropDownClass) {
            DbSet dropDowns;
		    dropDowns = getDropDowns(dropDownClass);
		    return View(dropDowns);
        }

	    [HttpGet]
	        public ActionResult Add(string dropDownClass){
		        return View();
        }

	    [HttpPost]
	    [ValidateAntiForgeryToken]
	    public ActionResult Add(DropDown model, string dropDownClass) {
		    if(ModelState.IsValid) {
                DbSet dropDowns;
                dropDowns = getDropDowns(dropDownClass);
		        dropDowns.Add(model);
			    db.SaveChanges();
                RouteValueDictionary routes = new RouteValueDictionary();
                routes.Add("id", model.ID);
                routes.Add("dropDownClass", dropDownClass);
                return RedirectToAction("Edit", "DropDownAdminController", routes);
            }
            else {
                ModelState.AddModelError(model.ToString(), "ModelState is not valid");
                return View(model);
            }
        }
        [HttpGet]
	    public ActionResult Edit(int id, string dropDownClass) {
            DbSet dropDowns;
            DropDown dropDown;
		    dropDowns = getDropDowns(dropDownClass);
		    dropDown = (DropDown)dropDowns.Find(id);
            return View(dropDown);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DropDown model, string dropDownClass){
            if(ModelState.IsValid) {
                db.SaveChanges();
                RouteValueDictionary routes = new RouteValueDictionary();
                routes.Add("id", model.ID);
                routes.Add("dropDownClass", dropDownClass);
                return RedirectToAction("Edit", "DropDownAdminController", routes);
            }
	        else {
                ModelState.AddModelError(model.ToString(),"ModelState is not valid");
	            return View(model);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, string dropDownClass) {
            DbSet dropDowns;
            DropDown dropDown;
	        dropDowns = getDropDowns(dropDownClass);
	        dropDown = (DropDown)dropDowns.Find(id);
	        dropDowns.Remove(dropDown);
            RouteValueDictionary routes = new RouteValueDictionary();
            routes.Add("dropDownClass", dropDownClass);
            return RedirectToAction("Index", "DropDownAdminController", routes);
        }

	    private DbSet getDropDowns(string dropDownClass)
        {
		    switch(dropDownClass) {
			    case "RequestType":
                    return db.RequestTypes;
			    case "QuestionType":
                    return db.QuestionTypes;
			    case "TumourGroup":
				    return db.TumourGroups;
                case "Regions":
                    return db.Regions;
            }
            return null;
        }
    }
}
