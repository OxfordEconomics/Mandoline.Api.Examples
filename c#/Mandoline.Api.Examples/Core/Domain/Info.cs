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
            if (output.IsAsync)
            {
                // queue asynchronous api call
                await api.GetDatabanksAsync().ContinueWith(t => output.PrintData(t.Result.Result));
            }
            else
            {
                var result = api.GetDatabanksAsync().GetAwaiter().GetResult();
                output.PrintData(result.Result);
            }
        }

        // get variables for a given databank code
        public static async Task RunGetVariablesAsync(Output output)
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // queue asynchronous api call
            if (output.IsAsync)
            {
                // queue asynchronous api call
                await api.GetVariablesAsync("WDMacro").ContinueWith(t => output.PrintData(t.Result.Result));
            }
            else
            {
                var result = api.GetVariablesAsync("WDMacro").GetAwaiter().GetResult();
                output.PrintData(result.Result);
            }
        }
    }
}
