﻿// <copyright file="DownloadShaped.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Client;
using Core.Client.ServiceModels;

namespace Core;

public class DownloadShaped
{
    // runs simple shaped stream download
    public static async Task RunDownloadShapedStreamAsync(Output output)
    {
        // get our sample selection
        SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

        // set up api object for making call
        ApiClient api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

        // set up simple configuration for shape request
        ShapeConfigurationDto config = new ShapeConfigurationDto()
        {
            Pivot = false,
            StackedQuarters = false,
            Frequency = FrequencyEnum.Annual,
            Format = Core.Client.ServiceModels.FormatEnum.Default,
        };

        // run download
        Client.Models.Assertion<Client.Models.ShapedStreamResult> shapedResult = await api.DownloadShapedStreamAsync(
            sampleSelect,
            config,
            new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ConfigureAwait(true);

        // process data
        output.PrintData(shapedResult.Result);
    }

    // runs simple shaped download
    public static async Task RunDownloadShapedAsync(Output output)
    {
        // get our sample selection
        SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

        // set up api object for making call
        ApiClient api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

        // set up simple configuration for shape request
        ShapeConfigurationDto config = new ShapeConfigurationDto()
        {
            Pivot = false,
            StackedQuarters = false,
            Frequency = FrequencyEnum.Annual,
            Format = Core.Client.ServiceModels.FormatEnum.Default,
        };

        // run download
        Client.Models.Assertion<Client.Models.ShapedStreamResult> shapedResult = await api.DownloadShapedAsync(
            sampleSelect,
            config,
            new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ConfigureAwait(true);

        // process output data
        output.PrintData(shapedResult.Result);
    }
}
