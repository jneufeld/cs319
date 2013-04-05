using System;
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
            ViewResult result = (ViewResult)dDAdminController.Add("Region");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void requesterTypeAddTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Add("RequesterType");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void questionTypeAddTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Add("QuestionType");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void tumourGroupAddTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Add("TumourGroup");
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
            Region justAdded = db.Regions.ToArray().Last();
            Assert.IsTrue(justAdded.Code.Equals("TST") && justAdded.FullName.Equals("RegionAddLogicTest") && justAdded.Enabled);
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
            RequesterType justAdded = db.RequesterTypes.ToArray().Last();
            Assert.IsTrue(justAdded.Code.Equals("TST") && justAdded.FullName.Equals("RequesterTypeAddLogicTest") && justAdded.Enabled);
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
            QuestionType justAdded = db.QuestionTypes.ToArray().Last();
            Assert.IsTrue(justAdded.Code.Equals("TST") && justAdded.FullName.Equals("QuestionTypeAddLogicTest") && justAdded.Enabled);
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
            TumourGroup justAdded = db.TumourGroups.ToArray().Last();
            Assert.IsTrue(justAdded.Code.Equals("TST") && justAdded.FullName.Equals("TumourGroupAddLogicTest") && justAdded.Enabled);
        }
        
        [TestMethod]
        public void nullEditTest()
        {
            ActionResult result = dDAdminController.Edit(0, null);
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
        
        [TestMethod]
        public void emptyEditTest()
        {
            ActionResult result = dDAdminController.Edit(0, "");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
        
        [TestMethod]
        public void wrongEditTest()
        {
            ActionResult result = dDAdminController.Edit(0, "Bubba");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
        
        [TestMethod]
        public void regionEditTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Edit(1, "Region");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void requesterTypeEditTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Edit(1, "RequesterType");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void questionTypeEditTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Edit(1, "QuestionType");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void tumourGroupEditTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Edit(1, "TumourGroup");
            Assert.IsNotNull(result.Model);
        }
        
        [TestMethod]
        public void nullEditLogicTest()
        {
            DropDown m = new DropDown();
            ActionResult result = dDAdminController.Edit(m, null);
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }
      
        [TestMethod]
        public void emptyEditLogicTest()
        {
            Region m = new Region();
            m.Code = "";
            m.FullName = "";
            ViewResult result = (ViewResult)dDAdminController.Edit(m, "Region");
            Assert.AreEqual("System.Web.Mvc.ViewResult", result.GetType().FullName);
        }
        
        [TestMethod]
        public void wrongEditLogicTest()
        {
            DropDown m = new DropDown();
            ActionResult result = dDAdminController.Edit(m, "Bubba");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void regionEditLogicTest()
        {
            Region last = db.Regions.ToArray().Last();
            last.Code = "TSTEdit";
            last.FullName = "RegionAddLogicTestEdit";
            ActionResult result = dDAdminController.Edit(last, "Region");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
            DREAMContext db2 = new DREAMContext();
            Region justChanged = db2.Regions.Find(last.ID);
            Assert.IsTrue(justChanged.Code.Equals("TSTEdit") && justChanged.FullName.Equals("RegionAddLogicTestEdit"));
        }

        [TestMethod]
        public void requesterTypeEditLogicTest()
        {
            RequesterType last = db.RequesterTypes.ToArray().Last();
            last.Code = "TSTEdit";
            last.FullName = "RequesterTypeAddLogicTestEdit";
            ActionResult result = dDAdminController.Edit(last, "RequesterType");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
            DREAMContext db2 = new DREAMContext();
            RequesterType justChanged = db2.RequesterTypes.Find(last.ID);
            Assert.IsTrue(justChanged.Code.Equals("TSTEdit") && justChanged.FullName.Equals("RequesterTypeAddLogicTestEdit"));
        }

        [TestMethod]
        public void questionTypeEditLogicTest()
        {
            QuestionType last = db.QuestionTypes.ToArray().Last();
            last.Code = "TSTEdit";
            last.FullName = "QuestionTypeAddLogicTestEdit";
            ActionResult result = dDAdminController.Edit(last, "QuestionType");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
            DREAMContext db2 = new DREAMContext();
            QuestionType justChanged = db2.QuestionTypes.Find(last.ID);
            Assert.IsTrue(justChanged.Code.Equals("TSTEdit") && justChanged.FullName.Equals("QuestionTypeAddLogicTestEdit"));
        }

        [TestMethod]
        public void tumourGroupEditLogicTest()
        {
            TumourGroup last = db.TumourGroups.ToArray().Last();
            last.Code = "TSTEdit";
            last.FullName = "TumourGroupAddLogicTestEdit";
            ActionResult result = dDAdminController.Edit(last, "TumourGroup");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
            DREAMContext db2 = new DREAMContext();
            TumourGroup justChanged = db2.TumourGroups.Find(last.ID);
            Assert.IsTrue(justChanged.Code.Equals("TSTEdit") && justChanged.FullName.Equals("TumourGroupAddLogicTestEdit"));
        }

        [TestMethod]
        public void nullDeleteTest()
        {
            ActionResult result = dDAdminController.Delete(0, true, null);
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void emptyDeleteTest()
        {
            ActionResult result = dDAdminController.Delete(0, true, "");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void wrongDeleteTest()
        {
            ActionResult result = dDAdminController.Delete(0, true, "Bubba");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void regionDeleteTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Delete(1, true, "Region");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void requesterTypeDeleteTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Delete(1, true, "RequesterType");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void questionTypeDeleteTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Delete(1, true, "QuestionType");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void tumourGroupDeleteTest()
        {
            ViewResult result = (ViewResult)dDAdminController.Delete(1, true, "TumourGroup");
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void nullDeleteLogicTest()
        {
            DropDown m = new DropDown();
            ActionResult result = dDAdminController.Delete(0, null);
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void emptyDeleteLogicTest()
        {
            Region m = new Region();
            m.Code = "";
            m.FullName = "";
            ActionResult result = dDAdminController.Delete(0, "");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void wrongDeleteLogicTest()
        {
            DropDown m = new DropDown();
            ActionResult result = dDAdminController.Delete(0, "Bubba");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        [TestMethod]
        public void regionDeleteLogicTest()
        {
            Region last = db.Regions.ToArray().Last();
            bool original = last.Enabled;
            ActionResult result = dDAdminController.Delete(last.ID, "Region");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
            DREAMContext db2 = new DREAMContext();
            Region lastChanged = db2.Regions.Find(last.ID);
            Assert.IsTrue(!original == lastChanged.Enabled);
        }

        [TestMethod]
        public void requesterTypeDeleteLogicTest()
        {
            RequesterType last = db.RequesterTypes.ToArray().Last();
            bool original = last.Enabled;
            ActionResult result = dDAdminController.Delete(last.ID, "RequesterType");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
            DREAMContext db2 = new DREAMContext();
            RequesterType lastChanged = db2.RequesterTypes.Find(last.ID);
            Assert.IsTrue(!original == lastChanged.Enabled);
        }

        [TestMethod]
        public void questionTypeDeleteLogicTest()
        {
            QuestionType last = db.QuestionTypes.ToArray().Last();
            bool original = last.Enabled;
            ActionResult result = dDAdminController.Delete(last.ID, "QuestionType");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
            DREAMContext db2 = new DREAMContext();
            QuestionType lastChanged = db2.QuestionTypes.Find(last.ID);
            Assert.IsTrue(!original == lastChanged.Enabled);
        }

        [TestMethod]
        public void tumourGroupDeleteLogicTest()
        {
            TumourGroup last = db.TumourGroups.ToArray().Last();
            bool original = last.Enabled;
            ActionResult result = dDAdminController.Delete(last.ID, "TumourGroup");
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
            DREAMContext db2 = new DREAMContext();
            TumourGroup lastChanged = db2.TumourGroups.Find(last.ID);
            Assert.IsTrue(!original == lastChanged.Enabled);
        }

    }
}