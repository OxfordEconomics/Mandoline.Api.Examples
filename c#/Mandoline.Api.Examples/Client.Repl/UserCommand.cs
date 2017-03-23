namespace Client.Repl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Mandoline.Api.Client;
    using Replify;

    public class UserCommand : IReplCommand
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
                    await Core.User.RunLoginAsync(this.output, user, pass).ConfigureAwait(true);
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
