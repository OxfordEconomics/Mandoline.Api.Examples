// <copyright file="Info.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

using System.Threading.Tasks;
using Core.Client;

namespace Core;

public class Info
{
    // downloads the full list of available databanks, loading up onto the DataGridView
    public static async Task RunGetDatabanksAsync(Output output)
    {
        // set up api object for making call
        ApiClient api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

        // queue asynchronous api call
        Client.Models.Assertion<System.Collections.Generic.IEnumerable<Client.Models.Databank>> databanksTask = await api.GetDatabanksAsync().ConfigureAwait(true);
        output.PrintData(databanksTask.Result);
    }

    // get variables for a given databank code
    public static async Task RunGetVariablesAsync(Output output)
    {
        // set up api object for making call
        ApiClient api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

        // queue asynchronous api call
        Client.Models.Assertion<Client.ServiceModels.VariableCollectionDto> variablesTask = await api.GetVariablesAsync("WDMacro").ConfigureAwait(true);
        output.PrintData(variablesTask.Result);
    }

    // get regions for a given databank code
    public static async Task RunGetRegionsAsync(Output output)
    {
        // set up api object for making call
        ApiClient api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

        // queue asynchronous api call
        Client.Models.Assertion<Client.ServiceModels.RegionCollectionDto> regionsTask = await api.GetRegionsAsync("WDMacro").ConfigureAwait(true);
        output.PrintData(regionsTask.Result);
    }
}
