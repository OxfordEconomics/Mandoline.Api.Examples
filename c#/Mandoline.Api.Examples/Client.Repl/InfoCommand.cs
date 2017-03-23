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

        public async Task GetDatabanks()
        {
            await Info.RunGetDatabanksAsync(this.output);
        }

        public async Task GetVariables()
        {
            await Info.RunGetVariablesAsync(this.output);
        }
    }
}
