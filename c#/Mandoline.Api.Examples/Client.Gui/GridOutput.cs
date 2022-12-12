// <copyright file="GridOutput.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core;
using Core.Client;
using Core.Client.ServiceModels;
using Core.Client.Models;

namespace Client.Gui;

// this implementation of Output directs output to a DataGridView
// for a Windows Form applicationv
internal class GridOutput : Output
{
    // constructor, must take windows form object to update load status and gridview
    public GridOutput(Form1 f)
    {
        this.FormInstance = f;
    }

    // delegate for passing data source in thread-safe way
    private delegate void ObjectDelegate(object newData);

    public Form1 FormInstance { get; set; } // to access the main form object

    // getters and setters for updating form fields
    private bool StatusLabelVisible
    {
        set
        {
            this.FormInstance.label1.Visible = value;
        }
    }

    // for updating status text
    private string StatusLabelText
    {
        set
        {
            if (value != null)
            {
                this.FormInstance.label1.Text = value;
            }
        }
    }

    private object DataGridInstance
    {
        set
        {
            this.FormInstance.dataGridView1.DataSource = value;
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
        this.StatusLabelVisible = v;
    }

    // updates gridview with selection object information

    /// <inheritdoc/>
    public override void PrintData(SelectionDto s)
    {
        // create table for displaying selection data
        Table.SelectionTable dt = new Table.SelectionTable();

        // process output
        SelectionDto output = s;
        Console.WriteLine("SELECTION ID: {0}...", s.Id);

        // pass databanks list to DataGridView object
        dt.Rows.Add(output.Id, output.Name, output.DatabankCode, output.MeasureCode, output.DownloadUrl);
        this.DataGridInstance = dt;

        // hide loading indicator
        this.StatusLabelVisible = false;
    }

    // process login output

    /// <inheritdoc/>
    public override void PrintData(Core.Client.Models.User u, string token)
    {
        // append user info to list, so that we can pass to the DataGridView object
        List<Core.Client.Models.User> output = new List<Core.Client.Models.User>();
        output.Add(u);

        // pass user list to DataGridView object
        this.DataGridInstance = output;

        // hide loading indicator
        this.StatusLabelVisible = false;

        // change current user's access token
        AppConstants.ApiToken = token;
    }

    // process output for multi user response

    /// <inheritdoc/>
    public override void PrintData(IEnumerable<Core.Client.Models.User> ul)
    {
        // pass user list to DataGridView object
        this.DataGridInstance = ul;

        // hide loading indicator
        this.StatusLabelVisible = false;
    }

    // process output for login response

    /// <inheritdoc/>
    public override void PrintData(Core.Client.Models.User u)
    {
        Console.WriteLine("User has {0} saved selections...", u.SavedSelections.ToList().Count);
        foreach (ResourceLink<Selection> s in u.SavedSelections)
        {
            Console.WriteLine("{0}: {1}", s.Name, s.Id);
        }

        // append user info to list, so that we can pass to the DataGridView object
        List<Core.Client.Models.User> output = new List<Core.Client.Models.User>();
        output.Add(u);

        // pass user list to DataGridView object
        this.DataGridInstance = output;

        // hide loading indicator
        this.StatusLabelVisible = false;
    }

    // process output for list of databanks

    /// <inheritdoc/>
    public override void PrintData(IEnumerable<Databank> ld)
    {
        // parse results into databanks list
        List<Databank> output = new List<Databank>();
        foreach (Databank m in ld)
        {
            output.Add(m);
        }

        // pass databanks list to DataGridView object
        this.DataGridInstance = output;

        // hide loading indicator
        this.StatusLabelVisible = false;
    }

    // process output for collection of variables

    /// <inheritdoc/>
    public override void PrintData(VariableCollectionDto vc)
    {
        Table.VariableTable dt = new Table.VariableTable();

        // pass list to DataGridView object
        foreach (VariableDto v in vc.Variables)
        {
            dt.AddRow(v);
        }

        this.DataGridInstance = dt;

        // hide loading indicator
        this.StatusLabelVisible = false;
    }

    // process output for collection of regions

    /// <inheritdoc/>
    public override void PrintData(RegionCollectionDto regions)
    {
        Table.RegionTable dt = new Table.RegionTable();

        // pass list to DataGridView object
        foreach (RegionDto region in regions.Regions)
        {
            dt.AddRow(region);
        }

        this.DataGridInstance = dt;

        // hide loading indicator
        this.StatusLabelVisible = false;
    }

    // output for single string data output (catch-all option for anything that returns single point of data)

    /// <inheritdoc/>
    public override void PrintData(string s)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Data");
        dt.Rows.Add(s);
        this.DataGridInstance = dt;

        // hide loading indicator
        this.StatusLabelVisible = false;
    }

    // shaped table output

    /// <inheritdoc/>
    public override void PrintData(ShapedStreamResult result)
    {
        Table.ShapeTable dt = new Table.ShapeTable(result);
        this.DataGridInstance = dt;

        // hide loading indicator
        this.StatusLabelVisible = false;
    }

    // output for download request

    /// <inheritdoc/>
    public override void PrintData(ControllerDownloadResponseDto response, string filename, string ready)
    {
        // create table for displaying selection data
        Table.DownloadRequestTable dt = new Table.DownloadRequestTable();

        // check to see whether download is ready and process output
        dt.Rows.Add(filename, "CSV", response.ReadyUrl, ready);
        this.DataGridInstance = dt;

        // hide loading indicator
        this.StatusLabelVisible = false;
    }

    /// <inheritdoc/>
    public override void PrintData(List<List<DataseriesDto>> ld)
    {
        // set up download table
        Table.PagedDownloadTable dt = new Table.PagedDownloadTable();

        // pull annual, quarterly data point into a new row for each
        for (int x = 0; x < ld.Count; x++)
        {
            foreach (DataseriesDto d in ld[x])
            {
                if (d.AnnualData != null)
                {
                    // make a new row for each annual data point
                    foreach (KeyValuePair<string, double?> entry in d.AnnualData)
                    {
                        try
                        {
                            // note the cells representing quarterly data are left blank
                            dt.Rows.Add(x + 1, d.DatabankCode, d.VariableCode, d.LocationCode, entry.Key, entry.Value);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: {0}", e.ToString());
                        }
                    }
                }

                if (d.QuarterlyData != null)
                {
                    // make a new row for each quarterly data point
                    foreach (KeyValuePair<string, double?> entry in d.QuarterlyData)
                    {
                        // note the cells representing annual data are left blank
                        dt.Rows.Add(x + 1, d.DatabankCode, d.VariableCode, d.LocationCode, null, null, entry.Key, entry.Value);
                    }
                }
            }
        }

        // update the DataGridView with the data table
        this.DataGridInstance = dt;

        // hide loading indicator
        this.StatusLabelVisible = false;
    }

    // output for downloads

    /// <inheritdoc/>
    public override void PrintData(List<DataseriesDto> ld)
    {
        // set up download table
        Table.DownloadTable dt = new Table.DownloadTable();

        // pull annual, quarterly data point into a new row for each
        foreach (DataseriesDto d in ld)
        {
            if (d.AnnualData != null)
            {
                // make a new row for each annual data point
                foreach (KeyValuePair<string, double?> entry in d.AnnualData)
                {
                    try
                    {
                        // note the cells representing quarterly data are left blank
                        dt.Rows.Add(d.DatabankCode, d.VariableCode, d.LocationCode, entry.Key, entry.Value);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: {0}", e.ToString());
                    }
                }
            }

            if (d.QuarterlyData != null)
            {
                // make a new row for each quarterly data point
                foreach (KeyValuePair<string, double?> entry in d.QuarterlyData)
                {
                    // note the cells representing annual data are left blank
                    dt.Rows.Add(d.DatabankCode, d.VariableCode, d.LocationCode, null, null, entry.Key, entry.Value);
                }
            }
        }

        // update the DataGridView with the data table
        this.DataGridInstance = dt;

        // hide loading indicator
        this.StatusLabelVisible = false;
    }
}
