// <copyright file="ConsoleOutput.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

namespace Client.Repl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Mandoline.Api.Client.Models;
    using Mandoline.Api.Client.ServiceModels;

    // this implementation of Output directs output to console
    internal class ConsoleOutput : Output
    {
        public ConsoleOutput()
        {
        }

        // status isn't indicated in this client
        private string StatusLabelText { get; set; }

        // status isn't indicated in this client
        public override void UpdateStatus(string v)
        {
        }

        // status isn't indicated in this client
        public override void UpdateStatus(bool v)
        {
        }

        // prints cells of generic DataTable
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
        public override void PrintData(SelectionDto s)
        {
            // create table for displaying selection data
            var dt = new Table.SelectionTable();

            // process output
            var output = s;
            Console.WriteLine("SELECTION ID: {0}...", s.Id);

            // pass databanks list to DataGridView object
            dt.Rows.Add(output.Id, output.Name, output.DatabankCode, output.MeasureCode, output.DownloadUrl);

            this.PrintTable(dt);
        }

        // process login output
        public override void PrintData(Mandoline.Api.Client.Models.User u, string token)
        {
            Core.AppConstants.ApiToken = token;
            this.PrintData(u);
        }

        // process output for multi user response
        public override void PrintData(IEnumerable<Mandoline.Api.Client.Models.User> ul)
        {
            foreach (Mandoline.Api.Client.Models.User u in ul)
            {
                Console.WriteLine("\t{0} {1} - Selections: {2}", u.FirstName, u.LastName, u.SavedSelections.Count());
            }
        }

        // process output for login response
        public override void PrintData(Mandoline.Api.Client.Models.User u)
        {
            Console.WriteLine("{0} {1} - Saved selections:", u.FirstName, u.LastName);
            foreach (ResourceLink<Selection> s in u.SavedSelections)
            {
                Console.WriteLine("\t{0}: {1}", s.Name, s.Id);
            }
        }

        // process output for list of databanks
        public override void PrintData(IEnumerable<Databank> ld)
        {
            foreach (DatabankDto d in ld)
            {
                Console.WriteLine("{0}: {1}", d.DatabankCode, d.Name);
            }
        }

        // process output for collection of variables
        public override void PrintData(VariableCollectionDto vc)
        {
            var dt = new Table.VariableTable();

            // pass list to DataGridView object
            foreach (VariableDto v in vc.Variables)
            {
                dt.AddRow(v);
            }

            this.PrintTable(dt);
        }

        // process output for collection of variables
        public override void PrintData(RegionCollectionDto regions)
        {
            var dt = new Table.RegionTable();

            // pass list to DataGridView object
            foreach (RegionDto region in regions.Regions)
            {
                dt.AddRow(region);
            }

            this.PrintTable(dt);
        }

        // output for single string data output (catch-all option for anything that returns single point of data)
        public override void PrintData(string s)
        {
            var dt = new DataTable();
            dt.Columns.Add("Data");
            dt.Rows.Add(s);
            this.PrintTable(dt);
        }

        // shaped table output
        public override void PrintData(ShapedStreamResult result)
        {
            var dt = new Table.ShapeTable(result);
            this.PrintTable(dt);
        }

        // output for download request
        public override void PrintData(ControllerDownloadResponseDto response, string filename, string ready)
        {
            // create table for displaying selection data
            var dt = new Table.DownloadRequestTable();

            // check to see whether download is ready and process output
            dt.Rows.Add(filename, "CSV", response.ReadyUrl, ready);
            this.PrintTable(dt);
        }

        // output for downloads
        public override void PrintData(List<DataseriesDto> ld)
        {
            // set up download table
            var dt = new Table.DownloadTable();

            // pull annual, quarterly data point into a new row for each
            foreach (DataseriesDto d in ld)
            {
                if (d.AnnualData != null)
                {
                    // make a new row for each annual data point
                    foreach (var entry in d.AnnualData)
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
                    foreach (var entry in d.QuarterlyData)
                    {
                        // note the cells representing annual data are left blank
                        dt.Rows.Add(d.DatabankCode, d.VariableCode, d.LocationCode, null, null, entry.Key, entry.Value);
                    }
                }
            }

            // update the DataGridView with the data table
            this.PrintTable(dt);
        }

        // output for paged downloads
        public override void PrintData(List<List<DataseriesDto>> ld)
        {
            // set up download table
            var dt = new Table.PagedDownloadTable();

            // pull annual, quarterly data point into a new row for each
            for (int x = 0; x < ld.Count; x++)
            {
                foreach (DataseriesDto d in ld[x])
                {
                    if (d.AnnualData != null)
                    {
                        // make a new row for each annual data point
                        foreach (var entry in d.AnnualData)
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
                        foreach (var entry in d.QuarterlyData)
                        {
                            // note the cells representing annual data are left blank
                            dt.Rows.Add(x + 1, d.DatabankCode, d.VariableCode, d.LocationCode, null, null, entry.Key, entry.Value);
                        }
                    }
                }
            }

            // update the DataGridView with the data table
            this.PrintTable(dt);
        }
    }
}
