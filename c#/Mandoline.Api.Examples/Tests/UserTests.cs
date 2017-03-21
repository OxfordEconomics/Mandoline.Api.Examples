using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;

namespace Tests
{
    [TestClass]
    public class UserTests
    {

        // gets current user
        // expected: returned API key should match the one in settings
        [TestMethod]
        public void TestGetUser()
        {
            var output = new TestOutput();
            User.RunGetUserAsync(output).RunSync();
            Assert.AreEqual(AppConstants.API_TOKEN, output.returnValueStr);
        }

        // returns user object corresponding to provided credentials
        // expected: returned API key should match the one in settings
        [TestMethod]
        public void TestLogin()
        {
            string expected = AppConstants.API_TOKEN;
            var output = new TestOutput();
            User.RunLoginAsync(output, AppConstants.USER_NAME, AppConstants.USER_PASS).RunSync();
            Assert.AreEqual(expected, output.returnValueStr);
        }


    }
}
