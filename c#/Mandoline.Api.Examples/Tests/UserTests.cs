// <copyright file="UserTests.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

using System.Threading.Tasks;
using Core;
using Core.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests;

[TestClass]
public class UserTests
{
    // gets current user
    // expected: returned API key should match the one in settings
    [TestMethod]
    public async Task UserGetTest()
    {
        var output = new TestOutput();
        await User.RunGetUserAsync(output);
        Assert.AreEqual(AppConstants.ApiToken, output.ReturnValueStr);
    }

    // returns user object corresponding to provided credentials
    // expected: returned API key should not be blank
    [TestMethod]
    public async Task UserLoginTest()
    {
        var output = new TestOutput();
        await User.RunLoginAsync(output, AppConstants.UserName, AppConstants.UserPassword);
        Assert.AreNotEqual(string.Empty, output.ReturnValueStr);
    }

    // returns user object corresponding to provided credentials
    // expected: returned API key should be blank
    [TestMethod]
    public async Task UserLoginFailTest()
    {
        var output = new TestOutput();
        await User.RunLoginAsync(output, AppConstants.UserName, string.Empty);
        Assert.AreEqual(string.Empty, output.ReturnValueStr);
    }
}
