using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DREAM.Models;
using System.Web.Security;

namespace DREAM.Tests.Models
{
    [TestClass]
    public class LogTest
    {
        Request request;
        MembershipUser user;
        DREAMContext db = new DREAMContext();

        [TestInitialize]
        public void SetupData()
        {
            // make sure the test user doesn't exist
            Membership.DeleteUser("user1");

            request = new Request();
            user = Membership.CreateUser("user1", "Password1!", "user1@example.com");

            db.SaveChanges();
        }

        [TestMethod]
        public void Create()
        {
            Log log = Log.Create(request, user);

            Assert.AreEqual(user.ProviderUserKey, log.User.ProviderUserKey);
            Assert.AreEqual(request.ID, log.RequestID);
            Assert.AreEqual(LogAction.CREATE, log.Action);
        }

        [TestMethod]
        public void Edit()
        {
            Log log = Log.Edit(request, user);

            Assert.AreEqual(user.ProviderUserKey, log.User.ProviderUserKey);
            Assert.AreEqual(request.ID, log.RequestID);
            Assert.AreEqual(LogAction.EDIT, log.Action);
        }

        [TestMethod]
        public void View()
        {
            Log log = Log.View(request, user);

            Assert.AreEqual(user.ProviderUserKey, log.User.ProviderUserKey);
            Assert.AreEqual(request.ID, log.RequestID);
            Assert.AreEqual(LogAction.VIEW, log.Action);
        }

        [TestMethod]
        public void Close()
        {
            Log log = Log.Close(request, user);

            Assert.AreEqual(user.ProviderUserKey, log.User.ProviderUserKey);
            Assert.AreEqual(request.ID, log.RequestID);
            Assert.AreEqual(LogAction.CLOSE, log.Action);
        }

        [TestCleanup]
        public void CleanUpData()
        {
            Membership.DeleteUser("user1");
        }
    }
}
