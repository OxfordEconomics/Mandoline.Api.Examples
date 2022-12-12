// <copyright file="User.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;

namespace Core.Client;

public class User
{
    // downloads user data, loading up onto the DataGridView
    public static async Task RunGetUserAsync(Output output)
    {
        // set up api object for making call
        ApiClient api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

        // run user get
        Models.Assertion<Models.User> userResult = await api.GetUserAsync().ConfigureAwait(true);

        // process output
        output.PrintData(userResult.Result);
    }

    // passes login credentials to the api, which returns a user object
    public static async Task RunLoginAsync(Output output, string user, string pass)
    {
        // set up api object for making call
        ApiClient api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

        // run login
        Models.Assertion<Models.User> loginResult = await api.LoginAsync(
            user,
            pass,
            new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ConfigureAwait(true);

        // process output data
        try
        {
            output.PrintData(loginResult.Result, loginResult.Result.ApiKey);
        }
        catch (NullReferenceException)
        {
            output.PrintData(loginResult.Result, string.Empty);
        }
    }

    // downloads all user data, loading up onto the DataGridView
    // note: depending on access, users are likely to get 403/Forbidden error
    public static async Task RunGetAllUsersAsync(Output output)
    {
        // set up api object for making call
        ApiClient api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

        Models.Assertion<System.Collections.Generic.List<Models.User>> usersResult = await api.GetAllUsersAsync().ConfigureAwait(true);
        output.PrintData(usersResult.Result);
    }
}
