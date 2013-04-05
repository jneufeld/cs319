using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using DREAM;
using DREAM.Controllers;
using DREAM.Models;
using System.Web.Security;

namespace DREAM.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private UsersController usersController = new UsersController();
        private UsersAdminController usersAdminController = new UsersAdminController();
        MembershipUser user;

        [TestInitialize]
        public void SetupData()
        {
            // make sure the test user doesn't exist
            Membership.DeleteUser("user1");

            // make sure the bad userName doesn't exist
            Membership.DeleteUser("BadUser");

            user = Membership.CreateUser("user1", "Password1!", "user1@example.com");
        }

        /// <summary>
        /// Checks that a userName that does not exist in the system 
        /// will fail to successfully login by checking the right code path is followed
        /// identified by the returning of a ViewResult
        /// </summary>
        [TestMethod]
        public void TestLoginWithIncorrectUsername()
        {
            LoginModel loginModel = new LoginModel();
            loginModel.UserName = "BadUser";
            loginModel.Password = "Password1!";
            ViewResult result = (ViewResult)usersController.Login(loginModel, "");
            Assert.AreEqual("System.Web.Mvc.ViewResult", result.GetType().FullName);
        }

        /// <summary>
        /// Checks that given a wrong password for an existing and valid username, the method login
        /// will fail to successfully login by checking the right code path is followed
        /// identified by the returning of a ViewResult
        /// </summary>
        [TestMethod]
        public void TestLoginWithIncorrectPassword()
        {
            LoginModel loginModel = new LoginModel();
            loginModel.UserName = "user1";
            loginModel.Password = "Password1!Wrong";
            ViewResult result = (ViewResult)usersController.Login(loginModel, "");
            Assert.AreEqual("System.Web.Mvc.ViewResult", result.GetType().FullName);
        }

        /// <summary>
        /// Checks that a user can login regardless of their username's case by accessing 
        /// the code for RedirectFromLoginPage which will throw a
        /// NullReferenceException as there is no way to create a test value for this method
        /// </summary>
        [TestMethod]
        public void TestLoginWithDiffCaseUserName()
        {
            LoginModel loginModel = new LoginModel();
            loginModel.UserName = "USER1";
            loginModel.Password = "Password1!";
            try
            {
                RedirectToRouteResult result = (RedirectToRouteResult)usersController.Login(loginModel, "www.google.com");
                Assert.AreEqual(0, 1);
            }
            catch (NullReferenceException e)
            {
                Assert.AreEqual(1, 1);
            }
        }

        /// <summary>
        /// Checks a user can login by accessing the code for RedirectFromLoginPage which will throw a
        /// NullReferenceException as there is no way to create a test value for this method
        /// </summary>
        [TestMethod]
        public void TestCorrectLogin()
        {
            LoginModel loginModel = new LoginModel();
            loginModel.UserName = "user1";
            loginModel.Password = "Password1!";
            try
            {
                RedirectToRouteResult result = (RedirectToRouteResult)usersController.Login(loginModel, "www.google.com");
                Assert.AreEqual(0, 1);
            }
            catch (NullReferenceException e) 
            {
                Assert.AreEqual(1,1);
            }            
        }

        /// <summary>
        /// Checks that change password for a wrong userName will fail
        /// </summary>
        [TestMethod]
        public void TestChangePasswordIncorrectUserName()
        {
            ChangePasswordModel cpModel = new ChangePasswordModel();
            cpModel.UserName = "BadUser";
            cpModel.CurrentPassword = "Password1!";
            cpModel.NewPassword = "Password2!";
            cpModel.NewPassword2 = "Password2!";
            ActionResult result = usersController.ChangePassword(cpModel);
            Assert.AreEqual("System.Web.Mvc.RedirectToRouteResult", result.GetType().FullName);
        }

        /// <summary>
        /// Checks that change password for a correct userName and Password will pass by accessing 
        /// the code for Request.IsAuthenticated which will throw a
        /// NullReferenceException as there is no way to create a test value for this method
        /// </summary>
        [TestMethod]
        public void TestChangePassword()
        {
            ChangePasswordModel cpModel = new ChangePasswordModel();
            cpModel.UserName = "user1";
            cpModel.CurrentPassword = "Password1!";
            cpModel.NewPassword = "Password2!";
            cpModel.NewPassword2 = "Password2!";
            try
            {
                ActionResult result = usersController.ChangePassword(cpModel);
                Assert.AreEqual(0, 1);
            }
            catch (NullReferenceException e)
            {
                Assert.AreEqual(1, 1);
            }    
        }
        [TestMethod]
        public void TestResetPassword()
        {
            PasswordResetRequestModel prrModel = new PasswordResetRequestModel();
            prrModel.Email = "user1@example.com";
            RedirectToRouteResult result = (RedirectToRouteResult)usersController.ResetPassword(prrModel);
            Assert.IsTrue(result.RouteValues.ContainsValue("Password Reset Successfully"));
        }
    }
}
