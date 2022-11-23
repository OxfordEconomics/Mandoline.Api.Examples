// <copyright file="SavedSelection.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

namespace Core
{
    using Core.Client;
    using Core.Client.ServiceModels;
    using System;
    using System.Threading.Tasks;

    public class SavedSelection
    {
        // creates a new saved selection based on SelectionDto sampleSelect
        public static async Task RunCreateSavedSelection(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

            // make sure id is empty
            sampleSelect.Id = Guid.Empty;

            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            var selectionResult = await api.CreateSavedSelectionAsync(
                sampleSelect,
                new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ConfigureAwait(true);

            await RunGetSavedSelection(selectionResult.Result.Id, output);
        }

        // updates saved selection based on id and SelectionDto sampleSelect
        // note: the selection object passed into this must have the id field set
        //       i.e. it isn't enough just to include the id i"n the function params
        public static async Task RunUpdateSavedSelection(Output output)
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // change selection object for update
            var oldSelect = await api.GetSavedSelection(AppConstants.SavedSelectionId).ConfigureAwait(true);
            if (oldSelect.Reason == System.Net.HttpStatusCode.NotFound)
            {
                output.PrintData("Selection not found...");
                return;
            }

            oldSelect.Result.Id = AppConstants.SavedSelectionId;
            oldSelect.Result.Name = "Selection - Updated: " + DateTime.Now.ToString();

            // run update
            var updateResult = await api.UpdateSavedSelectionAsync(
                AppConstants.SavedSelectionId,
                oldSelect.Result,
                new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ConfigureAwait(true);

            // check for changed selection
            await RunGetSavedSelection(AppConstants.SavedSelectionId, output);
        }

        // creates a new saved selection based on SelectionDto sampleSelect
        // takes output object, selection id
        public static async Task RunGetSavedSelection(Guid s_id, Output output)
        {
            // Console.WriteLine(string.Format("Checking selection with id={0}...", s_id));

            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // run get
            var getResult = await api.GetSavedSelection(s_id).ConfigureAwait(true);

            // check request for success/fail
            if (getResult.Reason == System.Net.HttpStatusCode.NotFound)
            {
                output.PrintData("Saved selection not found...");
            }
            else
            {
                // process output data
                output.PrintData(getResult.Result);
            }
        }
    }
}
