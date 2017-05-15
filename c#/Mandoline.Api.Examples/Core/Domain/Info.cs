// <copyright file="Info.cs" company="Oxford Economics">
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
    using Mandoline.Api.Client.Models;
    using Mandoline.Api.Client.ServiceModels;

    public class Info
    {
        // downloads the full list of available databanks, loading up onto the DataGridView
        public static async Task RunGetDatabanksAsync(Output output)
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // queue asynchronous api call
            var databanksTask = await api.GetDatabanksAsync().ConfigureAwait(true);
            output.PrintData(databanksTask.Result);
        }

        // get variables for a given databank code
        public static async Task RunGetVariablesAsync(Output output)
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // queue asynchronous api call
            var variablesTask = await api.GetVariablesAsync("WDMacro").ConfigureAwait(true);
            output.PrintData(variablesTask.Result);
        }

        // get regions for a given databank code
        public static async Task RunGetRegionsAsync(Output output)
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // queue asynchronous api call
            var regionsTask = await api.GetRegionsAsync("WDMacro").ConfigureAwait(true);
            output.PrintData(regionsTask.Result);
        }
    }
}
