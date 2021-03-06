﻿// <copyright file="User.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

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

            // run user get
            var userResult = await api.GetUserAsync().ConfigureAwait(true);

            // process output
            output.PrintData(userResult.Result);
        }

        // passes login credentials to the api, which returns a user object
        public static async Task RunLoginAsync(Output output, string user, string pass)
        {
            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // run login
            var loginResult = await api.LoginAsync(
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
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            var usersResult = await api.GetAllUsersAsync().ConfigureAwait(true);
            output.PrintData(usersResult.Result);
        }
    }
}
