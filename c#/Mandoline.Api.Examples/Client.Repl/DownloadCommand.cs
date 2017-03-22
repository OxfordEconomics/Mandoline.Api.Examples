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

    public class DownloadCommand : IReplCommand
    {
        private Output output;

        public DownloadCommand()
        {
            this.output = new ConsoleOutput();
        }

        public void DownloadFile()
        {
            Download.RunDownloadFileAsync(this.output).RunSync();
        }

        public void RequestDownload()
        {
            Download.RunRequestDownloadAsync(this.output).RunSync();
        }

        public void RunDownload()
        {
            Download.RunDownloadAsync(this.output).RunSync();
        }
    }
}
