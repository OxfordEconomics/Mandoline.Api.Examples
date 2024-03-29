﻿// <copyright file="InfoCommand.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

using System.Threading.Tasks;
using Core;
using Core.Client;

namespace Client.Repl;

public class InfoCommand
{
    private readonly Output output;

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
