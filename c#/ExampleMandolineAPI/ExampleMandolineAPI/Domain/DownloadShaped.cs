using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client;
using Mandoline.Shaping.Models;
using Mandoline.Api.Client.ServiceModels;
using Mandoline.Shaping;

namespace ExampleMandolineAPI 
{
    class DownloadShaped
    {
        // runs simple shaped stream download
        static public void RunDownloadShapedStreamAsync(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelect.GetInstance();

            // set up api object for making call
            var api = new ApiClient("https://services.oxfordeconomics.com/", AppConstants.API_TOKEN);

            // set up simple configuration for shape request
            var config = new ShapeConfigurationDto()
            {
                Pivot = false,
                StackedQuarters = false,
                Frequency = FrequencyEnum.Annual,
                Format = Mandoline.Api.Client.ServiceModels.FormatEnum.Default,

            };

            // queue asynchronous api call
            api.DownloadShapedStreamAsync(sampleSelect, config, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t => {
                Console.WriteLine("STATUS: {0}...", t.Result.Reason);

                // process output
                output.PrintData(t.Result.Result);

            // now check to see whether that download is ready
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        // runs simple shaped download
        static public void RunDownloadShapedAsync(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelect.GetInstance();

            // set up api object for making call
            var api = new ApiClient("https://services.oxfordeconomics.com/", AppConstants.API_TOKEN);

            // set up simple configuration for shape request
            var config = new ShapeConfigurationDto()
            {
                Pivot = false,
                StackedQuarters = false,
                Frequency = FrequencyEnum.Annual,
                Format = Mandoline.Api.Client.ServiceModels.FormatEnum.Default,

            };

            // queue asynchronous api call
            api.DownloadShapedAsync(sampleSelect, config, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t => {
                Console.WriteLine("STATUS: {0}...", t.Result.Reason);

                // process output
                output.PrintData(t.Result.Result);

            // now check to see whether that download is ready
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }
    }
}
