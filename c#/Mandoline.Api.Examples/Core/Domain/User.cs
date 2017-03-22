using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client;
using Mandoline.Api.Client.Models;

namespace Core
{
    public class User
    {
        // downloads user data, loading up onto the DataGridView
        public static async Task RunGetUserAsync(Output output)
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // queue asynchronous api call
            if (output.IsAsync)
            {
                await api.GetUserAsync().ContinueWith(t => output.PrintData(t.Result.Result));
            }
            else
            {
                var result = api.GetUserAsync().GetAwaiter().GetResult();
                output.PrintData(result.Result);
            }
        }

        // passes login credentials to the api, which returns a user object
        public static async Task RunLoginAsync(Output output, string user, string pass)
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // queue asynchronous api call
            if (output.IsAsync)
            {
                await api.LoginAsync(user, pass, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t =>
                    output.PrintData(t.Result.Result, t.Result.Result.ApiKey));
            }
            else
            {
                var result = api.LoginAsync(user, pass, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).GetAwaiter().GetResult();
                output.PrintData(result.Result, result.Result.ApiKey);
            }
        }

        // downloads all user data, loading up onto the DataGridView
        // note: depending on access, users are likely to get 403/Forbidden error
        public static async Task RunGetAllUsersAsync(Output output)
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // queue asynchronous api call
            if (output.IsAsync)
            {
                await api.GetAllUsersAsync().ContinueWith(t =>
                {
                    Console.WriteLine("STATUS: {0}...", t.Result.Reason);
                    if (t.Result.Failed == false)
                    {
                        output.PrintData(t.Result.Result);
                    }
                    else
                    {
                        output.UpdateStatus("Operation failed. Check input.");
                    }
                });
            }
            else
            {
                var result = api.GetAllUsersAsync().GetAwaiter().GetResult();
                output.PrintData(result.Result);
            }
        }
    }
}
