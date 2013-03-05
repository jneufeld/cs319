using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
                return View(model);//RedirectTo(“Edit”, model.ID, dropDownClass);
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
		        return View(model);//RedirectTo(“Edit”, model.ID, dropDownClass);
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
	        return null;//RedirectTo(“Index”, dropDownName);
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
