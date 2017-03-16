using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client;
using Mandoline.Api.Client.Models;

namespace ExampleMandolineAPI
{
    class User
    {
        // downloads user data, loading up onto the DataGridView
        public static void RunGetUserAsync(Output output) 
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BASE_URL, AppConstants.API_TOKEN);

            // queue asynchronous api call
            api.GetUserAsync().ContinueWith(t => {
                Console.WriteLine("STATUS: {0}...", t.Result.Reason);

                // process output
                var lu = new List<Mandoline.Api.Client.Models.User>();
                lu.Add(t.Result.Result);
                output.PrintData(lu);

            },TaskScheduler.FromCurrentSynchronizationContext());
        }

        // passes login credentials to the api, which returns a user object
        public static void RunLoginAsync(Output output, string user, string pass)
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BASE_URL, AppConstants.API_TOKEN);

            // queue asynchronous api call
            api.LoginAsync(user, pass, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t => {
                Console.WriteLine("STATUS: {0}...", t.Result.Reason);
                if (t.Result.Failed == false) output.PrintData(t.Result.Result, t.Result.Result.ApiKey);
                else output.UpdateStatus("Operation failed. Check input.");

            },TaskScheduler.FromCurrentSynchronizationContext());
        }

        // downloads all user data, loading up onto the DataGridView
        // note: depending on access, users are likely to get 403/Forbidden error 
        public static void RunGetAllUsersAsync(Output output) 
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BASE_URL, AppConstants.API_TOKEN);

            // queue asynchronous api call
            api.GetAllUsersAsync().ContinueWith(t => {
                Console.WriteLine("STATUS: {0}...", t.Result.Reason);
                if (t.Result.Failed == false) output.PrintData(t.Result.Result);
                else output.UpdateStatus("Operation failed. Check input.");

            },TaskScheduler.FromCurrentSynchronizationContext());

        }

    }
}
