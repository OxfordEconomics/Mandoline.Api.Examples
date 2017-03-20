using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client;
using Replify;
using Core;

namespace Client.Repl
{
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
                Core.User.RunLoginAsync(output, user, pass).RunSync();
            } while (Core.AppConstants.API_TOKEN == null);

        }

    }

    public class InfoCommand : IReplCommand
    {

        private Output output;

        public InfoCommand()
        {
            this.output = new ConsoleOutput();
        }

        public void GetDatabanks()
        {
            Info.RunGetDatabanksAsync(this.output).RunSync(); 
        }

        public void GetVariables()
        {
            Info.RunGetVariablesAsync(this.output).RunSync(); 
        }

    }
}
