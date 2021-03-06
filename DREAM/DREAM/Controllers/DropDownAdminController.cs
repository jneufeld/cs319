﻿using System;
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
    public class DropDownAdminController : Controller
    {
        private DREAMContext db = new DREAMContext();

        /// <summary>
        /// Directs the user to a page displaying all items from the given drop down menu
        /// </summary>
        /// <param name="dropDownClass"> The drop down menu </param>
        /// <returns> The view of the given drop down menu's index of all items </returns>
        [HttpGet]
        public ActionResult Index(string dropDownClass)
        {
            DbSet dropDowns;
            dropDowns = getDropDowns(dropDownClass);
            if (dropDowns == null)
            {
                return RedirectToAction("Index", "Home");
            }
            addAdminVariables(dropDownClass);
            return View(dropDowns);
        }

        /// <summary>
        /// Directs the user to the 'add an element to a specified drop down' page
        /// </summary>
        /// <param name="dropDownClass"> The drop down menu </param>
        /// <returns> The view for adding an element to the given drop down </returns>
        [HttpGet]
        public ActionResult Add(string dropDownClass)
        {
            DropDown m = getDropDownType(dropDownClass);
            if (m == null)
            {
                return RedirectToAction("Index", "Home");
            }
            addAdminVariables(dropDownClass);
            return View(m);
        }

        /// <summary>
        /// Adds the specified element to the given drop down menu
        /// </summary>
        /// <param name="model"> The element to add </param>
        /// <param name="dropDownClass"> The drop down menu to add to </param>
        /// <returns> Redirects to the Edit page for the given item that was added else stays on the same page </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DropDown model, string dropDownClass)
        {
            DropDown m = getDropDownType(dropDownClass);
            if (m == null)
            {
                return RedirectToAction("Index", "Home");
            }
            m.Code = model.Code;
            m.FullName = model.FullName;
            if (m.Code == null || m.Code == "" || m.FullName == null || m.FullName == "")
            {
                return View(m);
            }
            m.Enabled = true;
            if (ModelState.IsValid)
            {
                DbSet dropDowns;
                dropDowns = getDropDowns(dropDownClass);
                dropDowns.Add(m);
                db.SaveChanges();
                RouteValueDictionary routes = new RouteValueDictionary();
                routes.Add("dropDownClass", dropDownClass);
                return RedirectToAction("Index", "DropDownAdmin", routes);
            }
            else
            {
                ModelState.AddModelError("", "ModelState is not valid");
                addAdminVariables(dropDownClass);
                return View(m);
            }
        }

        /// <summary>
        ///  Directs the user to the 'edit a specified element in a drop down menu' page
        /// </summary>
        /// <param name="dropDownId"> The ID of the element to be edited </param>
        /// <param name="dropDownCode"> The code of the element to be edited </param>
        /// <param name="dropDownFullName"> The full name of the element to be edited </param>
        /// <param name="dropDownClass"> The drop down menu of the element to be edited </param>
        /// <returns>The view for editing an element from a given drop down </returns>
        [HttpGet]
        public ActionResult Edit(int dropDownId, string dropDownClass)
        {
            DbSet dropDowns;
            DropDown m = getDropDownType(dropDownClass);
            if (m == null)
            {
                return RedirectToAction("Index", "Home");
            }
            dropDowns = getDropDowns(dropDownClass);
            m = (DropDown)dropDowns.Find(dropDownId);
            addAdminVariables(dropDownClass);
            return View(m);
        }

        /// <summary>
        /// Edits a specified element from a given drop down menu
        /// </summary>
        /// <param name="model"> The element to be edited </param>
        /// <param name="dropDownClass"> The drop down menu of the element to be edited </param>
        /// <returns> Redirects to the Index of the given drop down menu else stays on the same page </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DropDown model, string dropDownClass)
        {
            if (dropDownClass == null || (dropDownClass != "RequesterType" && dropDownClass != "QuestionType" && dropDownClass != "TumourGroup" && dropDownClass != "Region"))
            {
                return RedirectToAction("Index", "Home");
            }
            addAdminVariables(dropDownClass);
            if (model.Code == null || model.FullName == null)
            {
                DropDown m = getDropDownType(dropDownClass);
                m.ID = model.ID;
                m.Code = model.Code;
                m.FullName = model.FullName;
                m.Enabled = model.Enabled;
                return View(m);
            }
            if (ModelState.IsValid)
            {
                DbSet dropDowns;
                DropDown dropDown = new DropDown();
                dropDowns = getDropDowns(dropDownClass);
                dropDown = (DropDown)dropDowns.Find(model.ID);
                if (model.Code == null || model.Code == "" || model.FullName == null || model.FullName == "")
                {
                    return View(model);
                }
                dropDown.Code = model.Code;
                dropDown.FullName = model.FullName;
                db.SaveChanges();
                RouteValueDictionary routes = new RouteValueDictionary();
                routes.Add("dropDownClass", dropDownClass);
                return RedirectToAction("Index", "DropDownAdmin", routes);
            }
            else
            {
                ModelState.AddModelError("", "ModelState is not valid");
                return View(model);
            }
        }

        /// <summary>
        /// Directs the user to the 'delete an element from a specified drop down menu' page
        /// </summary>
        /// <param name="dropDownId"> The ID of the element to be deleted </param>
        /// <param name="dropDownCode"> The code of the element to be deleted </param>
        /// <param name="dropDownFullName"> The full name of the element to be deleted </param>
        /// <param name="dropDownClass"> The drop down menu of the element to be deleted </param>
        /// <returns>The view for deleting an element from a given drop down </returns>
        [HttpGet]
        public ActionResult Delete(int dropDownId, bool enabled, string dropDownClass)
        {
            DbSet dropDowns;
            DropDown m = getDropDownType(dropDownClass);
            if (m == null)
            {
                return RedirectToAction("Index", "Home");
            }
            dropDowns = getDropDowns(dropDownClass);
            m = (DropDown)dropDowns.Find(dropDownId);
            addAdminVariables(dropDownClass);
            return View(m);
        }

        /// <summary>
        /// Deletes the specified element from the given drop down menu
        /// </summary>
        /// <param name="dropDownId"> The ID of the element to be deleted</param>
        /// <param name="dropDownClass"> The drop down menu to delete an element from</param>
        /// <returns> Redirects to the Index of the given drop down menu </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int dropDownId, string dropDownClass)
        {
            DbSet dropDowns;
            DropDown dropDown = new DropDown();
            dropDowns = getDropDowns(dropDownClass);
            if (dropDowns == null)
            {
                return RedirectToAction("Index", "Home");
            }
            dropDown = (DropDown)dropDowns.Find(dropDownId);
            dropDown.Enabled = !dropDown.Enabled;
            db.SaveChanges();
            RouteValueDictionary routes = new RouteValueDictionary();
            routes.Add("dropDownClass", dropDownClass);
            return RedirectToAction("Index", "DropDownAdmin", routes);
        }

        /// <summary>
        /// Gets the DbSet of all elements of the given drop down menu
        /// </summary>
        /// <param name="dropDownClass"> The drop down menu </param>
        /// <returns> The desired DbSet else null</returns>
        private DbSet getDropDowns(string dropDownClass)
        {
            switch (dropDownClass)
            {
                case "RequesterType":
                    return db.RequesterTypes;
                case "QuestionType":
                    return db.QuestionTypes;
                case "TumourGroup":
                    return db.TumourGroups;
                case "Region":
                    return db.Regions;
            }
            return null;
        }
        private DropDown getDropDownType(string dropDownClass)
        {
            switch (dropDownClass)
            {
                case "RequesterType":
                    return new RequesterType();
                case "QuestionType":
                    return new QuestionType();
                case "TumourGroup":
                    return new TumourGroup();
                case "Region":
                    return new Region();
            }
            return null;
        }

        private void addAdminVariables(string dropDownClass)
        {
            switch (dropDownClass)
            {
                case "RequesterType":
                    ViewBag.ActiveDropDownAdminClass = "Requester Type";
                    break;
                case "QuestionType":
                    ViewBag.ActiveDropDownAdminClass = "Question Type";
                    break;
                case "TumourGroup":
                    ViewBag.ActiveDropDownAdminClass = "Tumour Group";
                    break;
                case "Region":
                    ViewBag.ActiveDropDownAdminClass = "Region";
                    break;
            }
        }
    }
}