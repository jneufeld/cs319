using System;
using System.Web.Mvc;
using DREAM;
using DREAM.Controllers;
using DREAM.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedList;
using System.Web.Security;

namespace DREAM.Tests.Controllers
{
    [TestClass]
    public class LogsAdminControllerTest
    {
        private LogsAdminController logsAdminController = new LogsAdminController();
        private DREAMContext db = new DREAMContext();

        /// <summary>
        /// Tests that Index returns a model with a non-null PagedList attribute.
        /// </summary>
        [TestMethod]
        public void testIndex()
        {
            ViewResult result = (ViewResult)logsAdminController.Index();
            LogFilterModel resultModel = (LogFilterModel)result.Model;
            IPagedList<Log> resultList = resultModel.PagedLogs;
            Assert.IsNotNull(resultList);
        }

        /// <summary>
        /// Tests that the proper code path is followed when the Filter button is pressed,
        /// and that the models are identical except for their PagedLogs attributes.
        /// </summary>
        [TestMethod]
        public void testFilterButton()
        {
            LogFilterModel lfm = new LogFilterModel();
            lfm.RequestID = 1;
            ViewResult result = (ViewResult)logsAdminController.Index("Filter", lfm);
            LogFilterModel resultModel = (LogFilterModel)result.Model;
            resultModel.PagedLogs = lfm.PagedLogs;
            Assert.AreEqual(resultModel, lfm);
        }

        /// <summary>
        /// Tests that the proper code path is followed when the Clear button is pressed,
        /// and that the model returned is fresh/not equal to the model passed to the method.
        /// </summary>
        [TestMethod]
        public void testClearButton()
        {
            LogFilterModel lfm = new LogFilterModel();
            lfm.RequestID = 1;
            ViewResult result = (ViewResult)logsAdminController.Index("Clear", lfm);
            LogFilterModel resultModel = (LogFilterModel)result.Model;
            Assert.AreNotEqual(resultModel, lfm);
        }
    }
}
