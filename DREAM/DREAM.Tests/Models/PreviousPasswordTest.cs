using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DREAM.Models;
using System.Web.Security;

namespace DREAM.Tests.Models
{
    [TestClass]
    public class PreviousPasswordTest
    {
        MembershipUser user;
        DREAMContext db = new DREAMContext();

        [TestInitialize]
        public void SetupData()
        {
            // make sure the test user doesn't exist
            Membership.DeleteUser("user1");

            user = Membership.CreateUser("user1", "Password1!", "user1@example.com");
        }

        [TestMethod]
        public void CheckPasswordTest()
        {
            Assert.IsFalse(PreviousPassword.CheckPassword(user, "Password1!"));
            Assert.IsTrue(PreviousPassword.CheckPassword(user, "NewPassword1!"));
        }
    }
}
