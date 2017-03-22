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
