// <copyright file="DownloadShapedTests.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests;

[TestClass]
public class DownloadShapedTests
{
    // gets a sample data download from the macro databank
    // expected: count of data rows > 0
    [TestMethod]
    [Ignore]
    public async Task DownloadShapedTest()
    {
        TestOutput output = new TestOutput();
        await DownloadShaped.RunDownloadShapedAsync(output);
        Assert.IsTrue(output.ReturnValueInt > 0);
    }

    // gets a sample data download from the macro databank
    // expected: count of data rows > 0
    [TestMethod]
    [Ignore]
    public async Task DownloadShapedStreamTest()
    {
        TestOutput output = new TestOutput();
        await DownloadShaped.RunDownloadShapedStreamAsync(output);
        Assert.IsTrue(output.ReturnValueInt > 0);
    }
}
