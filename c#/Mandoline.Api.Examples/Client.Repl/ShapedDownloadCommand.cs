// <copyright file="ShapedDownloadCommand.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project 
// root for full license information.
// </copyright>

namespace Client.Repl
{
    using System.Threading.Tasks;
    using Core;

    public class ShapedDownloadCommand
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
