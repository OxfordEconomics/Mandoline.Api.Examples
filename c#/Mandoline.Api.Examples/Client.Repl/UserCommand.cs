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

        public void GetUser()
        {
            User.RunGetUserAsync(this.output).RunSync();
        }

        public void GetUsers()
        {
            User.RunGetAllUsersAsync(this.output).RunSync();
        }

        public void LogIn()
        {
            do
            {
                Console.Write("Username: ");
                string user = Console.ReadLine();
                Console.Write("Password: ");
                string pass = Console.ReadLine();
                Core.User.RunLoginAsync(this.output, user, pass).RunSync();
            }
            while (Core.AppConstants.ApiToken == null);
        }
    }
}
