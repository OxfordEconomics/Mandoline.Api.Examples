// <copyright file="DownloadTests.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

namespace Tests
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DownloadTests
    {
        // gets a sample data download from the macro databank
        // expected: count of dataseries > 0
        [TestMethod]
        public async Task DownloadTest()
        {
            var output = new TestOutput();
            await Download.RunDownloadAsync(output);
            Assert.IsTrue(output.ReturnValueInt > 0);
        }

        // gets a sample data download from the macro databank
        // expected: count of variables > 0
        [TestMethod]
        public async Task DownloadRequestTest()
        {
            var output = new TestOutput();
            await Download.RunRequestDownloadAsync(output);
            Assert.AreNotEqual(output.ReturnValueStr, string.Empty);
        }

        // gets the full list of variables for the macro databank
        // expected: count of variables > 0
        [TestMethod]
        public async Task DownloadFileTest()
        {
            var output = new TestOutput();
            await Download.RunDownloadFileAsync(output);
            Assert.AreNotEqual(output.ReturnValueStr, string.Empty);
        }
    }
}
