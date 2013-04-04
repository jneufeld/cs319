﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DREAM;
using DREAM.Controllers;
using DREAM.Models;

namespace DREAM.Tests.Controllers
{
    [TestClass]
    public class DropDownAdminControllerTest
    {
        private DropDownAdminController dDAdminController = new DropDownAdminController();
        private DREAMContext db = new DREAMContext();

        [TestMethod]
        public void nullIndexTest()
        {
            ActionResult result = dDAdminController.Index(null);
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void emptyIndexTest()
        {
            ActionResult result = dDAdminController.Index("");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void wrongIndexTest()
        {
            ActionResult result = dDAdminController.Index("Bubba");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void regionIndexTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("Region");
            DbSet model = (DbSet)result.Model;
            Assert.IsNotNull(model);
            Assert.AreEqual("DREAM.Models.Region", model.ElementType.FullName);
            foreach (DropDown dp in model)
            {
                Assert.IsNotNull(dp.ID);
                Assert.IsNotNull(dp.Code);
                Assert.IsNotNull(dp.FullName);
            }
        }

        [TestMethod]
        public void requesterTypeIndexTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("RequesterType");
            DbSet model = (DbSet)result.Model;
            Assert.IsNotNull(model);
            Assert.AreEqual("DREAM.Models.RequesterType", model.ElementType.FullName);
            foreach (DropDown dp in model)
            {
                Assert.IsNotNull(dp.ID);
                Assert.IsNotNull(dp.Code);
                Assert.IsNotNull(dp.FullName);
            }
        }

        [TestMethod]
        public void questionTypeIndexTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("QuestionType");
            DbSet model = (DbSet)result.Model;
            Assert.IsNotNull(model);
            Assert.AreEqual("DREAM.Models.QuestionType", model.ElementType.FullName);
            foreach (DropDown dp in model)
            {
                Assert.IsNotNull(dp.ID);
                Assert.IsNotNull(dp.Code);
                Assert.IsNotNull(dp.FullName);
            }
        }

        [TestMethod]
        public void tumourGroupIndexTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("TumourGroup");
            DbSet model = (DbSet)result.Model;
            Assert.IsNotNull(model);
            Assert.AreEqual("DREAM.Models.TumourGroup", model.ElementType.FullName);
            foreach (DropDown dp in model)
            {
                Assert.IsNotNull(dp.ID);
                Assert.IsNotNull(dp.Code);
                Assert.IsNotNull(dp.FullName);
            }
        }

        [TestMethod]
        public void nullAddTest()
        {
            ActionResult result = dDAdminController.Add(null);
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void emptyAddTest()
        {
            ActionResult result = dDAdminController.Add("");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void wrongAddTest()
        {
            ActionResult result = dDAdminController.Add("Bubba");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void regionAddTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("Region");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void requesterTypeAddTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("RequesterType");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void questionTypeAddTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("QuestionType");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void tumourGroupAddTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("TumourGroup");
            Assert.IsNotNull(result.Model);
        }
        
        [TestMethod]
        public void nullAddLogicTest()
        {
            DropDown m = new DropDown();
            ActionResult result = dDAdminController.Add(m, null);
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
      
        [TestMethod]
        public void emptyAddLogicTest()
        {
            Region m = new Region();
            m.Code = "";
            m.FullName = "";
            ViewResult result = (ViewResult)dDAdminController.Add(m, "Region");
            Assert.AreEqual("System.Web.Mvc.ViewResult", result.GetType().FullName);
        }
        
        [TestMethod]
        public void wrongAddLogicTest()
        {
            DropDown m = new DropDown();
            ActionResult result = dDAdminController.Add(m, "Bubba");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
        
        [TestMethod]
        public void regionAddLogicTest()
        {
            Region m = new Region();
            m.Code = "TST";
            m.FullName = "RegionAddLogicTest";
            m.Enabled = true;
            ActionResult result = dDAdminController.Add(m, "Region");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
       
        [TestMethod]
        public void requesterTypeAddLogicTest()
        {
            RequesterType m = new RequesterType();
            m.Code = "TST";
            m.FullName = "RequesterTypeAddLogicTest";
            m.Enabled = true;
            ActionResult result = dDAdminController.Add(m, "RequesterType");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void questionTypeAddLogicTest()
        {
            QuestionType m = new QuestionType();
            m.Code = "TST";
            m.FullName = "QuestionTypeAddLogicTest";
            m.Enabled = true;
            ActionResult result = dDAdminController.Add(m, "QuestionType");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void tumourGroupAddLogicTest()
        {
            TumourGroup m = new TumourGroup();
            m.Code = "TST";
            m.FullName = "TumourGroupAddLogicTest";
            m.Enabled = true;
            ActionResult result = dDAdminController.Add(m, "TumourGroup");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
        /*
        [TestMethod]
        public void nullEditTest()
        {
            ActionResult result = dDAdminController.Edit(0, null);
            //db.Regions;
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
        
        [TestMethod]
        public void emptyAddTest()
        {
            ActionResult result = dDAdminController.Add("");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void wrongAddTest()
        {
            ActionResult result = dDAdminController.Add("Bubba");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void regionAddTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("Region");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void requesterTypeAddTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("RequesterType");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void questionTypeAddTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("QuestionType");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void tumourGroupAddTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("TumourGroup");
            Assert.IsNotNull(result.Model);
        }
        
        [TestMethod]
        public void nullAddLogicTest()
        {
            DropDown m = new DropDown();
            ActionResult result = dDAdminController.Add(m, null);
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
      
        [TestMethod]
        public void emptyAddLogicTest()
        {
            Region m = new Region();
            m.Code = "";
            m.FullName = "";
            ViewResult result = (ViewResult)dDAdminController.Add(m, "Region");
            Assert.AreEqual("System.Web.Mvc.ViewResult", result.GetType().FullName);
        }
        
        [TestMethod]
        public void wrongAddLogicTest()
        {
            DropDown m = new DropDown();
            ActionResult result = dDAdminController.Add(m, "Bubba");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
        
        [TestMethod]
        public void regionAddLogicTest()
        {
            Region m = new Region();
            m.Code = "TST";
            m.FullName = "RegionAddLogicTest";
            m.Enabled = true;
            ActionResult result = dDAdminController.Add(m, "Region");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
       
        [TestMethod]
        public void requesterTypeAddLogicTest()
        {
            RequesterType m = new RequesterType();
            m.Code = "TST";
            m.FullName = "RequesterTypeAddLogicTest";
            m.Enabled = true;
            ActionResult result = dDAdminController.Add(m, "RequesterType");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void questionTypeAddLogicTest()
        {
            QuestionType m = new QuestionType();
            m.Code = "TST";
            m.FullName = "QuestionTypeAddLogicTest";
            m.Enabled = true;
            ActionResult result = dDAdminController.Add(m, "QuestionType");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void tumourGroupAddLogicTest()
        {
            TumourGroup m = new TumourGroup();
            m.Code = "TST";
            m.FullName = "TumourGroupAddLogicTest";
            m.Enabled = true;
            ActionResult result = dDAdminController.Add(m, "TumourGroup");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
        */

    }
}