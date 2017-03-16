using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client;
using Mandoline.Api.Client.Models;
using Mandoline.Api.Client.ServiceModels;
using System.Data;

namespace ExampleMandolineAPI 
{
    class Info
    {
        // downloads the full list of available databanks, loading up onto the DataGridView
        public static void RunGetDatabanksAsync(Output output)
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BASE_URL, AppConstants.API_TOKEN);

            // queue asynchronous api call
            api.GetDatabanksAsync().ContinueWith(t => {
                Console.WriteLine("STATUS: {0}...", t.Result.Reason);

                // process output
                output.PrintData(t.Result.Result);

            },TaskScheduler.FromCurrentSynchronizationContext());
        }

        // get variables for a given databank code
        public static void RunGetVariablesAsync(Output output)
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BASE_URL, AppConstants.API_TOKEN);

            // queue asynchronous api call
            api.GetVariablesAsync("WDMacro").ContinueWith(t => {
                Console.WriteLine("STATUS: {0}...", t.Result.Reason);

                // process data
                output.PrintData(t.Result.Result);

            },TaskScheduler.FromCurrentSynchronizationContext());
        }

    }
}
