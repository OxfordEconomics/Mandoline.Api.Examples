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

    public class ShapedDownloadCommand : IReplCommand
    {
        private Output output;

        public ShapedDownloadCommand()
        {
            this.output = new ConsoleOutput();
        }

        public void ShapedDownload()
        {
            DownloadShaped.RunDownloadShapedAsync(this.output).RunSync();
        }

        public void ShapedDownloadStream()
        {
            DownloadShaped.RunDownloadShapedStreamAsync(this.output).RunSync();
        }
    }
}
