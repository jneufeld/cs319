using System;
using System.Web.Mvc;
using DREAM;
using DREAM.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Security;

namespace DREAM.Tests.Controllers
{
    [TestClass]
    public class UsersAdminControllerTest
    {
        private UsersAdminController usersAdminController = new UsersAdminController();

        /// <summary>
        /// Tests that index returns a view with a not null collection of users
        /// </summary>
        [TestMethod]
        public void TestIndex()
        {
            ViewResult result = (ViewResult)usersAdminController.Index();
            MembershipUserCollection users = (MembershipUserCollection)result.Model;
            Assert.IsNotNull(users);
        }
        /*
        /// <summary>
        /// Tests Registering a user without a user name will fail
        /// </summary>
        [TestMethod]
        public void TestRegisterWithoutUserName()
        {
            ViewResult result = (ViewResult)usersAdminController.Index();
            MembershipUserCollection users = (MembershipUserCollection)result.Model;
            Assert.IsNotNull(users);
        }
        */
    }
}
