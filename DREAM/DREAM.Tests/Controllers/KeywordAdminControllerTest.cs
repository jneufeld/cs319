using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [TestMethod]
        public void Edit(Keyword testKeyword)
        {
            // arrange
            KeywordsAdminController keywordsAdminController = new KeywordsAdminController();
            Keyword testKeyword1 = new Keyword
            {
                KeywordText = "Test Keyword",
                Enabled = true,
            };

            Question testQuestion1 = new Question();
            testQuestion1.Keywords.Add(testKeyword1);
            testKeyword1.AssociatedQuestions.Add(testQuestion1);

            // act
            ViewResult editResult = keywordsAdminController.Edit(testKeyword1) as ViewResult;

            // assert
            Assert.IsNotNull(editResult);
        }
    }
}
