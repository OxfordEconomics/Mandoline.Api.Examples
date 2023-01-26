// <copyright file="TestOutput.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Client;
using Core.Client.Models;
using Core.Client.ServiceModels;

namespace Tests;

// this implementation of Output directs output to console
internal class TestOutput : Output
{
    // value to check while testing
    public string ReturnValueStr;
    public int ReturnValueInt;
    public DateTime ReturnValueDate;

    // ensure that api operations are performed synchronously
    public TestOutput()
    {
        this.ReturnValueStr = string.Empty;
        this.ReturnValueInt = -1;
        this.ReturnValueDate = DateTime.MinValue;
    }

    // for updating status text
    private string StatusLabelText
    {
        set
        {
            Console.WriteLine(value);
        }
    }

    /// <inheritdoc/>
    public override void UpdateStatus(string v)
    {
        this.StatusLabelText = v;
    }

    /// <inheritdoc/>
    public override void UpdateStatus(bool v)
    {
    }

    public void PrintTable(DataTable table)
    {
        foreach (DataRow row in table.Rows)
        {
            for (int x = 0; x < table.Columns.Count; x++)
            {
                if (x != 0 && row[x].ToString() != string.Empty)
                {
                    Console.Write(" - ");
                }

                Console.Write("{0}", row[x].ToString());
            }

            Console.WriteLine();
        }
    }

    // updates gridview with selection object information

    /// <inheritdoc/>
    public override void PrintData(SelectionDto s)
    {
        this.ReturnValueDate = s.LastUpdate;
        this.ReturnValueStr = s.Id.ToString();
    }

    // process login output

    /// <inheritdoc/>
    public override void PrintData(Core.Client.Models.User u, string token)
    {
        try
        {
            this.ReturnValueStr = token;
        }
        catch (NullReferenceException)
        {
            this.ReturnValueStr = string.Empty;
        }
    }

    // process output for multi user response

    /// <inheritdoc/>
    public override void PrintData(IEnumerable<Core.Client.Models.User> ul)
    {
        foreach (Core.Client.Models.User u in ul)
        {
            Console.WriteLine("\t{0} {1} - Selections: {2}", u.FirstName, u.LastName, u.SavedSelections.Count());
        }
    }

    // process output for single user

    /// <inheritdoc/>
    public override void PrintData(Core.Client.Models.User u)
    {
        this.ReturnValueStr = u.ApiKey;
    }

    // process output for list of databanks

    /// <inheritdoc/>
    public override void PrintData(IEnumerable<Databank> ld)
    {
        this.ReturnValueInt = ld.Count();
    }

    // process output for collection of variables

    /// <inheritdoc/>
    public override void PrintData(VariableCollectionDto vc)
    {
        this.ReturnValueInt = vc.Variables.Count();
    }

    // process output for collection of regions

    /// <inheritdoc/>
    public override void PrintData(RegionCollectionDto regions)
    {
        this.ReturnValueInt = regions.Regions.Count();
    }

    // output for single string data output (catch-all option for anything that returns single point of data)

    /// <inheritdoc/>
    public override void PrintData(string s)
    {
        this.ReturnValueStr = s;
    }

    // shaped table output

    /// <inheritdoc/>
    public override void PrintData(ShapedStreamResult result)
    {
        this.ReturnValueInt = result.Rows.Count();
    }

    // output for download request

    /// <inheritdoc/>
    public override void PrintData(ControllerDownloadResponseDto response, string filename, string ready)
    {
        this.ReturnValueStr = response.ReadyUrl;
    }

    // output for downloads

    /// <inheritdoc/>
    public override void PrintData(List<DataseriesDto> ld)
    {
        this.ReturnValueInt = ld.Count();
    }

    // output for downloads

    /// <inheritdoc/>
    public override void PrintData(List<List<DataseriesDto>> ld)
    {
        this.ReturnValueInt = ld.Count();
    }
}
