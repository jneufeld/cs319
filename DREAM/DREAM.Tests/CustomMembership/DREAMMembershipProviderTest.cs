using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DREAM.CustomMembership;
using System.Web.Security;

namespace DREAM.Tests.CustomMembership
{
    [TestClass]
    public class DREAMMembershipProviderTest
    {
        [TestMethod]
        public void ExtractPasswordData()
        {
            // if the test user already exists delete it
            Membership.DeleteUser("user1");

            MembershipUser user = Membership.CreateUser("user1", "Password1!", "user1@example.com");

            Assert.IsNotNull(user);

            Tuple<MembershipPasswordFormat, string, string> passwordData = DREAMMembershipProvider.ExtractPasswordData(user);

            Assert.AreEqual(passwordData.Item1, MembershipPasswordFormat.Hashed);
            Assert.IsNotNull(passwordData.Item2);
            Assert.IsNotNull(passwordData.Item3);

            Membership.DeleteUser("user1");
        }
    }
}
