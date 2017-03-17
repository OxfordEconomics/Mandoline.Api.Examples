using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client;
using System.Data;
using Mandoline.Api.Client.ServiceModels;

namespace ExampleMandolineAPI
{
    class Download 
    {
        // runs the download request operation, which queues a download and returns a ready URL
        public static void RunRequestDownloadAsync(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelect.GetInstance();

            // set up api object for making call
            var api = new ApiClient(AppConstants.BASE_URL, AppConstants.API_TOKEN);

            string filename = "SampleDownload-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv";

            // queue asynchronous api call
            api.RequestDownloadAsync(sampleSelect, FileFormat.Csv, filename,new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t => {
                Console.WriteLine("STATUS: {0}...", t.Result.Reason);
                return t.Result;

            // now check to see whether that download is ready
            }).ContinueWith(u =>
            {
                // queue asynchronous api call
                api.DownloadReadyAsync(u.Result.Result, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(v => {
                    output.PrintData(u.Result.Result, filename, v.Result.Result);

                });


            });

        }

        // runs the download file request operation, which queues a download and returns a string
        public static void RunDownloadFileAsync(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelect.GetInstance();

            // set up api object for making call
            var api = new ApiClient(AppConstants.BASE_URL, AppConstants.API_TOKEN);

            // set up download request
            var req = new ControllerDownloadRequestDto()
            {
                selections = new SelectionDto[] { sampleSelect },
                format = FileFormat.Csv,
                name = "SampleFileDownload-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv"
            };

            // queue asynchronous api call
            api.DownloadFileAsync(req, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t => {
                Console.WriteLine("STATUS: {0}...", t.Result.Reason);
                output.PrintData(t.Result.Result);

            });

        }

        // downloads a sample selection from the macro databank, loading up onto the DataGridView
        public static void RunDownloadAsync(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelect.GetInstance();

            // set the number of items (i.e. in this case DataseriesDto objects) to be included in each page of data
            const int PAGE_SIZE = 5;

            // initialize api object
            var api = new ApiClient(AppConstants.BASE_URL, AppConstants.API_TOKEN);

            // queue api requests till all pages of data have been received
            // first section here returns a list of data series objects
            Task.Factory.StartNew(() =>
            {
                // this will track which page of data is currently being requested from the api
                int page = 0;
                var newPage = new List<DataseriesDto>();
                var allPages = new List<DataseriesDto>();

                // loop till the number of elements in the new page of data doesn't equal the page size we expect
                do
                {
                    api.DownloadAsync(sampleSelect, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token, page, PAGE_SIZE).ContinueWith(t =>
                    {
                        newPage = t.Result.Result;
                        
                        // iterate through each element in the new page of data, adding each data series to the allPages collection
                        foreach (DataseriesDto d in newPage) allPages.Add(d);

                    }).Wait(); // wait for each page to download

                    Console.WriteLine("Downloaded page {0}...", ++page);

                } while (newPage.Count == PAGE_SIZE);

                return allPages;

            // once that's finished, process the collected data into table-form and update the DataGridView accordingly
            }).ContinueWith(t =>
            {
				// process data
                output.PrintData(t.Result);

            });

        }

    }
}
