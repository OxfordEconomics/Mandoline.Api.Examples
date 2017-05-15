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
    using Mandoline.Api.Client.ServiceModels;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SelectionTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

            var api = new Mandoline.Api.Client.ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            var newSelection = api.CreateSavedSelectionAsync(
                sampleSelect,
                new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).Result;

            AppConstants.SavedSelectionId = newSelection.Result.Id;
        }

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
            // get the selection pre-update, store original LastUpdated value 
            var api = new Mandoline.Api.Client.ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);
            var oldSelection = await api.GetSavedSelection(AppConstants.SavedSelectionId);
            var preTestUpdateTime = oldSelection.Result.LastUpdate;

            // update saved selection with new name only
            var output = new TestOutput();
            await SavedSelection.RunUpdateSavedSelection(output);

            // get saved selection again, storing new LastUpdated value
            var newSelection = await api.GetSavedSelection(AppConstants.SavedSelectionId);
            var postTestUpdateTime = newSelection.Result.LastUpdate;

            // new LastUpdated should be greater than old
            Assert.IsTrue(postTestUpdateTime > preTestUpdateTime);
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
