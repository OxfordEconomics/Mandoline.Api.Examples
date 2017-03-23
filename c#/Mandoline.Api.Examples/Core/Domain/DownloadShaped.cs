namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Mandoline.Api.Client;
    using Mandoline.Api.Client.ServiceModels;
    using Mandoline.Shaping;
    using Mandoline.Shaping.Models;

    public class DownloadShaped
    {
        // runs simple shaped stream download
        public static async Task RunDownloadShapedStreamAsync(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // set up simple configuration for shape request
            var config = new ShapeConfigurationDto()
            {
                Pivot = false,
                StackedQuarters = false,
                Frequency = FrequencyEnum.Annual,
                Format = Mandoline.Api.Client.ServiceModels.FormatEnum.Default,
            };

            // queue asynchronous api call
            if (output.IsAsync)
            {
                await api.DownloadShapedStreamAsync(sampleSelect, config, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t =>

                    // process output
                    output.PrintData(t.Result.Result));
            }
            else
            {
                var result = api.DownloadShapedStreamAsync(sampleSelect, config, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).GetAwaiter().GetResult();
                output.PrintData(result.Result);
            }
        }

        // runs simple shaped download
        public static async Task RunDownloadShapedAsync(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // set up simple configuration for shape request
            var config = new ShapeConfigurationDto()
            {
                Pivot = false,
                StackedQuarters = false,
                Frequency = FrequencyEnum.Annual,
                Format = Mandoline.Api.Client.ServiceModels.FormatEnum.Default,
            };

            // queue asynchronous api call
            if (output.IsAsync)
            {
                await api.DownloadShapedAsync(sampleSelect, config, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t =>

                    // process output
                    output.PrintData(t.Result.Result));
            }
            else
            {
                var result = api.DownloadShapedAsync(sampleSelect, config, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).GetAwaiter().GetResult();
                output.PrintData(result.Result);
            }
        }
    }
}
