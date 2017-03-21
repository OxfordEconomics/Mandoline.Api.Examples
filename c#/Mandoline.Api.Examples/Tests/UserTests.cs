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
        public void UserGetTest()
        {
            var output = new TestOutput();
            User.RunGetUserAsync(output).RunSync();
            Assert.AreEqual(AppConstants.API_TOKEN, output.returnValueStr);
        }

        // returns user object corresponding to provided credentials
        // expected: returned API key should not be blank
        [TestMethod]
        public void UserLoginTest()
        {
            var output = new TestOutput();
            User.RunLoginAsync(output, AppConstants.USER_NAME, AppConstants.USER_PASS).RunSync();
            Assert.AreNotEqual(string.Empty, output.returnValueStr);
        }

        // returns user object corresponding to provided credentials
        // expected: returned API key should be blank
        [TestMethod]
        public void UserLoginFailTest()
        {
            var output = new TestOutput();
            User.RunLoginAsync(output, AppConstants.USER_NAME, string.Empty).RunSync();
            Assert.AreEqual(string.Empty, output.returnValueStr);
        }


    }
}
