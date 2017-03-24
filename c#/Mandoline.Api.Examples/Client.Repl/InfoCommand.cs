// <copyright file="InfoCommand.cs" company="Oxford Economics">
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

        public async Task GetRegions()
        {
            await Info.RunGetRegionsAsync(this.output);
        }
    }
}
