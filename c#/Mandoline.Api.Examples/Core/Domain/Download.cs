﻿// <copyright file="Download.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Client;
using Core.Client.ServiceModels;

namespace Core;

public class Download
{
    // runs the download request operation, which queues a download and returns a ready URL
    public static async Task RunRequestDownloadAsync(Output output)
    {
        // get our sample selection
        SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

        // set up api object for making call
        ApiClient api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

        string filename = "SampleDownload-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv";

        // run download request
        Client.Models.Assertion<ControllerDownloadResponseDto> requestResult = await api.RequestDownloadAsync(
            sampleSelect,
            FileFormat.Csv,
            filename,
            new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ConfigureAwait(true);

        // run ready check
        Client.Models.Assertion<string> readyResult = await api.DownloadReadyAsync(
            requestResult.Result,
            new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ConfigureAwait(true);

        // print results
        output.PrintData(requestResult.Result, filename, readyResult.Result);
    }

    // runs the download file request operation, which queues a download and returns a string
    public static async Task RunDownloadFileAsync(Output output)
    {
        // get our sample selection
        SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

        // set up api object for making call
        ApiClient api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

        // set up download request
        ControllerDownloadRequestDto req = new ControllerDownloadRequestDto()
        {
            selections = new SelectionDto[] { sampleSelect },
            format = FileFormat.Csv,
            name = "SampleFileDownload-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv",
        };

        // make file request
        Client.Models.Assertion<string> fileResult = await api.DownloadFileAsync(
            req,
            new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ConfigureAwait(true);
        output.PrintData(fileResult.Result);
    }

    // downloads a sample selection from the macro databank, loading up onto the DataGridView
    public static async Task RunDownloadAsync(Output output)
    {
        // queue api requests till all pages of data have been received
        // first section here returns a list of data series objects
        List<List<DataseriesDto>> downloadResult = await DownloadPages().ConfigureAwait(true);
        output.PrintData(downloadResult);
    }

    // runs a download and returns a list of dataseries lists, effectively preserving
    // the pages as they arrived
    private static async Task<List<List<DataseriesDto>>> DownloadPages()
    {
        // get our sample selection
        SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

        // set the number of items (i.e. in this case DataseriesDto objects) to be included in each page of data
        const int PAGE_SIZE = 5;

        // initialize api object
        var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

        // this will track which page of data is currently being requested from the api
        int page = 0;
        List<DataseriesDto> newPage;
        List<List<DataseriesDto>> pageList = new List<List<DataseriesDto>>();

        // loop till the number of elements in the new page of data doesn't equal the page size we expect
        do
        {
            // make the download request
            Client.Models.Assertion<List<DataseriesDto>> downloadResult = await api.DownloadAsync(
                sampleSelect,
                new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token,
                page,
                PAGE_SIZE).ConfigureAwait(true);

            newPage = downloadResult.Result;

            pageList.Add(newPage);

            Console.WriteLine("Downloaded page {0}...", ++page);
        }
        while (newPage.Count == PAGE_SIZE);

        return pageList;
    }
}
