using System;
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
        /*
        [TestMethod]
        public void nullAddLogicTest()
        {
            ActionResult result = dDAdminController.Add(null, null);
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void emptyAddLogicTest()
        {
            ActionResult result = dDAdminController.Add("");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void wrongAddLogicTest()
        {
            ActionResult result = dDAdminController.Add("Bubba");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void regionAddLogicTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("Region");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void requesterTypeAddLogicTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("RequesterType");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void questionTypeAddLogicTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("QuestionType");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void tumourGroupAddLogicTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Index("TumourGroup");
            Assert.IsNotNull(result.Model);
        }
        */
    }
}