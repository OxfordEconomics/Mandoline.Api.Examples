// <copyright file="SelectionTest.cs" company="Oxford Economics">
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
    public class SelectionTest
    {
        // retrieves saved selection object based on selection id in settings
        // expected: selection object id matches provided selection id
        [TestMethod]
        public async Task SelectionGetTest()
        {
            var output = new TestOutput();
            await SavedSelection.RunGetSavedSelection(AppConstants.SavedSelectionId, output);
            Assert.AreEqual(AppConstants.SavedSelectionId.ToString(), output.ReturnValueStr);
        }

        // updates a saved selection
        // expected: selection's last update is > before test run
        [TestMethod]
        public async Task SelectionUpdateTest()
        {
            var preTestTime = DateTime.Now;
            var output = new TestOutput();
            await SavedSelection.RunUpdateSavedSelection(output);
            Assert.IsTrue(output.ReturnValueDate > preTestTime);
        }

        // creates a new non-temp saved selection
        // expected: id of newly created selection exists
        [TestMethod]
        public async Task SelectionCreateTest()
        {
            var output = new TestOutput();
            await SavedSelection.RunGetSavedSelection(AppConstants.SavedSelectionId, output);
            Assert.AreNotEqual(string.Empty, output.ReturnValueStr);
        }
    }
}
