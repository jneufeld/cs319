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
    //[Authorize(Roles="ADMIN")]
    public class DropDownAdminController : Controller
    {
        private DREAMContext db = new DREAMContext();

        [HttpGet]
        public ActionResult Index(string dropDownClass)
        {
            DbSet dropDowns;
            dropDowns = getDropDowns(dropDownClass);
            return View(dropDowns);
        }

        [HttpGet]
        public ActionResult Add(string dropDownClass)
        {
            DropDown m = null;
            switch (dropDownClass)
            {
                case "RequestType":
                    m = new RequestType();
                    break;
                case "QuestionType":
                    m = new QuestionType();
                    break;
                case "TumourGroup":
                    m = new TumourGroup();
                    break;
                case "Regions":
                    m = new Region();
                    break;
            }
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DropDown model, string dropDownClass)
        {
            DropDown m = null;
            switch (dropDownClass)
            {
                case "RequestType":
                    m = new RequestType();
                    break;
                case "QuestionType":
                    m = new QuestionType();
                    break;
                case "TumourGroup":
                    m = new TumourGroup();
                    break;
                case "Regions":
                    m = new Region();
                    break;
            }
            m.Code = model.Code;
            m.FullName = model.FullName;
            if (ModelState.IsValid)
            {
                DbSet dropDowns;
                dropDowns = getDropDowns(dropDownClass);
                dropDowns.Add(m);
                db.SaveChanges();
                RouteValueDictionary routes = new RouteValueDictionary();
                routes.Add("dropDownId", m.ID);
                routes.Add("dropDownClass", dropDownClass);
                return RedirectToAction("Edit", "DropDownAdmin", routes);
            }
            else
            {
                ModelState.AddModelError(m.FullName, "ModelState is not valid");
                return View(m);
            }
        }
        [HttpGet]
        public ActionResult Edit(int dropDownId, string dropDownCode, string dropDownFullName, string dropDownClass)
        {
            DropDown m = null;
            switch (dropDownClass)
            {
                case "RequestType":
                    m = new RequestType();
                    break;
                case "QuestionType":
                    m = new QuestionType();
                    break;
                case "TumourGroup":
                    m = new TumourGroup();
                    break;
                case "Regions":
                    m = new Region();
                    break;
            }
            m.ID = dropDownId;
            m.Code = dropDownCode;
            m.FullName = dropDownFullName;
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DropDown model, string dropDownClass)
        {
            if (ModelState.IsValid)
            {
                DbSet dropDowns;
                DropDown dropDown = new DropDown();
                dropDowns = getDropDowns(dropDownClass);
                dropDown = (DropDown)dropDowns.Find(model.ID);
                dropDown.Code = model.Code;
                dropDown.FullName = model.FullName;
                db.SaveChanges();
                RouteValueDictionary routes = new RouteValueDictionary();
                routes.Add("dropDownClass", dropDownClass);
                return RedirectToAction("Index", "DropDownAdmin", routes);
            }
            else
            {
                ModelState.AddModelError(model.ToString(), "ModelState is not valid");
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Delete(int dropDownId, string dropDownCode, string dropDownFullName, string dropDownClass)
        {
            DropDown m = null;
            switch (dropDownClass)
            {
                case "RequestType":
                    m = new RequestType();
                    break;
                case "QuestionType":
                    m = new QuestionType();
                    break;
                case "TumourGroup":
                    m = new TumourGroup();
                    break;
                case "Regions":
                    m = new Region();
                    break;
            }
            m.ID = dropDownId;
            m.Code = dropDownCode;
            m.FullName = dropDownFullName;
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int dropDownId, string dropDownClass)
        {
            DbSet dropDowns;
            DropDown dropDown = new DropDown();
            dropDowns = getDropDowns(dropDownClass);
            dropDown = (DropDown)dropDowns.Find(dropDownId);
            dropDowns.Remove(dropDown);
            db.SaveChanges();
            RouteValueDictionary routes = new RouteValueDictionary();
            routes.Add("dropDownClass", dropDownClass);
            return RedirectToAction("Index", "DropDownAdmin", routes);
        }

        private DbSet getDropDowns(string dropDownClass)
        {
            switch (dropDownClass)
            {
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