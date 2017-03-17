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
            User.RunGetUserAsync(this.output); 
        }

    }
}
