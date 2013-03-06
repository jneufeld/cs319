using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DREAM.Models;

namespace DREAM.Tests.Models
{
    [TestClass]
    public class PasswordResetRequestTest
    {
        [TestMethod]
        public void GenerateNewIDTest()
        {
            Assert.IsNotNull(PasswordResetRequest.GenerateNewID(), "ID should never be null", null);
        }
    }
}
