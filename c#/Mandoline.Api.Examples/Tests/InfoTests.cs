// <copyright file="InfoTests.cs" company="Oxford Economics">
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
    public class InfoTests
    {
        // gets the full list of available databanks
        // expected: count of databanks > 0
        [TestMethod]
        public async Task InfoDatabanksTest()
        {
            var output = new TestOutput();
            await Info.RunGetDatabanksAsync(output);
            Assert.IsTrue(output.ReturnValueInt > 0);
        }

        // gets the full list of available databanks
        // expected: count of databanks > 0
        [TestMethod]
        public async Task InfoRegionsTest()
        {
            var output = new TestOutput();
            await Info.RunGetRegionsAsync(output);
            Assert.IsTrue(output.ReturnValueInt > 0);
        }

        // gets the full list of variables for the macro databank
        // expected: count of variables > 0
        [TestMethod]
        public async Task InfoVariablesTest()
        {
            var output = new TestOutput();
            await Info.RunGetVariablesAsync(output);
            Assert.IsTrue(output.ReturnValueInt > 0);
        }
    }
}
