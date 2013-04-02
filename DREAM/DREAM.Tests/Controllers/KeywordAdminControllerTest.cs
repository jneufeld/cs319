using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DREAM;
using DREAM.Controllers;
using DREAM.Models;

namespace DREAM.Tests.Controllers
{
    [TestClass]
    public class KeywordAdminControllerTest
    {
        private  KeywordsAdminController keywordsAdminController = new KeywordsAdminController();
        private DREAMContext dbContext = new DREAMContext();

        [TestMethod]
        public void IndexTest()
        {
            ActionResult index = keywordsAdminController.Index();
            Assert.AreEqual("System.Web.Mvc.ViewResult", index.GetType().FullName);
        }

        [TestMethod]
        public void EditTest(String keyword)
        {

        }


        //[TestMethod]
        //public void Edit(Keyword testKeyword)
        //{
        //    // arrange
        //    Keyword testKeyword1 = new Keyword
        //    {
        //        KeywordText = "Test Keyword",
        //        Enabled = true,
        //    };

        //    Question testQuestion1 = new Question();
        //    testQuestion1.Keywords.Add(testKeyword1);
        //    testKeyword1.AssociatedQuestions.Add(testQuestion1);

        //    // act
        //    ViewResult editResult = keywordsAdminController.Edit(testKeyword1) as ViewResult;

        //    // assert
        //    Assert.IsNotNull(editResult);
        //}
    }
}
