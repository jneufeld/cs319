using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DREAM.CustomMembership;
using System.Web.Security;
using DREAM.Models;

namespace DREAM.Tests.CustomMembership
{
    [TestClass]
    public class DREAMMembershipProviderTest
    {
        MembershipUser user;

        [TestInitialize]
        public void SetUpData()
        {
            // if the test user already exists delete it
            Membership.DeleteUser("user1");

            user = Membership.CreateUser("user1", "Password1!", "user1@example.com");
        }

        [TestCleanup]
        public void Cleanup()
        {
            Membership.DeleteUser("user1");
        }

        [TestMethod]
        public void ExtractPasswordData()
        {
            Tuple<MembershipPasswordFormat, string, string> passwordData = DREAMMembershipProvider.ExtractPasswordData(user);

            Assert.AreEqual(passwordData.Item1, MembershipPasswordFormat.Hashed);
            Assert.IsNotNull(passwordData.Item2);
            Assert.IsNotNull(passwordData.Item3);
        }

        [TestMethod]
        public void CreateUser()
        {
            using (DREAMContext db = new DREAMContext())
            {
                Assert.AreEqual(1, db.PreviousPasswords.Where(p => p.UserID.Equals((Guid)user.ProviderUserKey)).Count());
            }
        }

        [TestMethod]
        public void ChangePassword()
        {
            Assert.IsTrue(user.ChangePassword("Password1!", "NewPassword1!"));
            try
            {
                user.ChangePassword("NewPassword1!", "Password1!");
                Assert.Fail("ChangePassword didn't through an PreviouslyUsedPasswordException.");
            }
            catch (PreviouslyUsedPasswordException) { }
            catch (Exception)
            {
                Assert.Fail("ChangePassword didn't through an PreviouslyUsedPasswordException.");
            }
        }
    }
}
