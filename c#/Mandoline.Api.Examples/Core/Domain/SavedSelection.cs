// <copyright file="SavedSelection.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Mandoline.Api.Client;
    using Mandoline.Api.Client.ServiceModels;

    public class SavedSelection
    {
        // creates a new saved selection based on SelectionDto sampleSelect
        public static async Task RunCreateSavedSelection(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            var selectionResult = await api.CreateSavedSelectionAsync(
                sampleSelect,
                new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ConfigureAwait(true);

            await RunGetSavedSelection(selectionResult.Result.Id, output);
        }

        // updates saved selection based on id and SelectionDto sampleSelect
        // note: the selection object passed into this must have the id field set
        //       i.e. it isn't enough just to include the id in the function params
        public static async Task RunUpdateSavedSelection(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

            // create table for displaying selection data
            Table.SelectionTable dt = new Table.SelectionTable();

            // change selection object for update
            sampleSelect.Id = AppConstants.SavedSelectionId;
            sampleSelect.Name = "Selection - Updated: " + DateTime.Now;

            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // run update
            var getResult = await api.UpdateSavedSelectionAsync(
                sampleSelect.Id,
                sampleSelect,
                new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ConfigureAwait(true);

            // check for changed selection
            await RunGetSavedSelection(AppConstants.SavedSelectionId, output);
        }

        // creates a new saved selection based on SelectionDto sampleSelect
        // takes output object, selection id
        public static async Task RunGetSavedSelection(Guid s_id, Output output)
        {
            Console.WriteLine(string.Format("Checking selection with id={0}...", s_id));

            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

            // create table for displaying selection data
            Table.SelectionTable dt = new Table.SelectionTable();

            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // run get
            var getResult = await api.GetSavedSelection(s_id).ConfigureAwait(true);

            // process output data
            output.PrintData(getResult.Result);
        }
    }
}
