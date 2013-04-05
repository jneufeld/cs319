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

        /// <summary>
        /// Tests Registering a user code
        /// </summary>
        [TestMethod]
        public void TestRegisterWithoutUserName()
        {
            Membership.DeleteUser("TestUser1");
            MembershipUser user = Membership.CreateUser("TestUser1", "Password1!", "user1@example.com");
            Assert.IsNotNull(user);
        }
    }
}
