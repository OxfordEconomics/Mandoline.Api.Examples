// <copyright file="DownloadCommand.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

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

        public async Task DownloadFile()
        {
            await Download.RunDownloadFileAsync(this.output);
        }

        public async Task RequestDownload()
        {
            await Download.RunRequestDownloadAsync(this.output);
        }

        public async Task RunDownload()
        {
            await Download.RunDownloadAsync(this.output);
        }
    }
}
