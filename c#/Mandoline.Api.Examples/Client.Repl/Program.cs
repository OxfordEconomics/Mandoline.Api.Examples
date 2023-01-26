// <copyright file="Program.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core;
using Sharprompt;

namespace Client.Repl;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // Entry point fo REPL command line interface
        var commands = new Dictionary<string, Func<Task>>();

        UserCommand userCommands = new UserCommand();
        commands.Add("GetUser", userCommands.GetUser);
        commands.Add("LogIn", userCommands.LogIn);

        ShapedDownloadCommand shapedDownloadCommands = new ShapedDownloadCommand();
        commands.Add("ShapedDownload", shapedDownloadCommands.ShapedDownload);
        commands.Add("ShapedDownloadStream", shapedDownloadCommands.ShapedDownloadStream);

        SelectionCommand selectionCommands = new SelectionCommand();
        commands.Add("GetSelection", selectionCommands.GetSelection);
        commands.Add("CreateSelection", selectionCommands.CreateSelection);
        commands.Add("UpdateSelection", selectionCommands.UpdateSelection);

        InfoCommand infoCommands = new InfoCommand();
        commands.Add("GetDatabanks", infoCommands.GetDatabanks);
        commands.Add("GetVariables", infoCommands.GetVariables);
        commands.Add("GetRegions", infoCommands.GetRegions);

        DownloadCommand downloadCommands = new DownloadCommand();
        commands.Add("DownloadFile", downloadCommands.DownloadFile);
        commands.Add("RequestDownload", downloadCommands.RequestDownload);
        commands.Add("RunDownload", downloadCommands.RunDownload);

        while (true)
        {
            string value = Prompt.Select<string>("$", commands.Keys);
            await commands[value]();
        }
    }
}