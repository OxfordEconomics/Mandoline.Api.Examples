// <copyright file="UserCommand.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project 
// root for full license information.
// </copyright>

namespace Client.Repl
{
    using System;
    using System.Threading.Tasks;
    using Core.Client;

    public class UserCommand
    {
        private Output output;

        public UserCommand()
        {
            this.output = new ConsoleOutput();
        }

        public async Task GetUser()
        {
            await User.RunGetUserAsync(this.output);
        }

        public async Task GetUsers()
        {
            await User.RunGetAllUsersAsync(this.output);
        }

        public async Task LogIn()
        {
            bool success = false;
            do
            {
                Console.Write("Username: ");
                string user = Console.ReadLine();
                Console.Write("Password: ");
                string pass = Console.ReadLine();

                try
                {
                    await User.RunLoginAsync(this.output, user, pass).ConfigureAwait(true);
                    success = true;
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("Login failed. Please try again.");
                }
            }
            while (!success);

        }
    }
}
