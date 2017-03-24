// <copyright file="ShapedDownloadCommand.cs" company="Oxford Economics">
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

    public class ShapedDownloadCommand : IReplCommand
    {
        private Output output;

        public ShapedDownloadCommand()
        {
            this.output = new ConsoleOutput();
        }

        public async Task ShapedDownload()
        {
            await DownloadShaped.RunDownloadShapedAsync(this.output);
        }

        public async Task ShapedDownloadStream()
        {
            await DownloadShaped.RunDownloadShapedStreamAsync(this.output);
        }
    }
}
