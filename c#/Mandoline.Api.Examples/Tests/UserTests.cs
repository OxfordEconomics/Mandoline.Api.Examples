namespace Tests
{
    using System;
    using Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.AreEqual(AppConstants.ApiToken, output.ReturnValueStr);
        }

        // returns user object corresponding to provided credentials
        // expected: returned API key should not be blank
        [TestMethod]
        public void UserLoginTest()
        {
            var output = new TestOutput();
            User.RunLoginAsync(output, AppConstants.UserName, AppConstants.UserPassword).RunSync();
            Assert.AreNotEqual(string.Empty, output.ReturnValueStr);
        }

        // returns user object corresponding to provided credentials
        // expected: returned API key should be blank
        [TestMethod]
        public void UserLoginFailTest()
        {
            var output = new TestOutput();
            User.RunLoginAsync(output, AppConstants.UserName, string.Empty).RunSync();
            Assert.AreEqual(string.Empty, output.ReturnValueStr);
        }
    }
}
