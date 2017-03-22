namespace Client.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Core;
    using Mandoline.Api.Client.Models;
    using Mandoline.Api.Client.ServiceModels;

    // this implementation of Output directs output to a DataGridView
    // for a Windows Form application
    internal class GridOutput : Core.Output
    {
        // constructor, must take windows form object to update load status and gridview
        public GridOutput(Form1 f)
        {
            this.FormInstance = f;
            this.IsAsync = true;
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
                Console.WriteLine("Updating data grid...");
                if (this.FormInstance.dataGridView1.InvokeRequired)
                {
                    ObjectDelegate d = new ObjectDelegate(t => { this.FormInstance.dataGridView1.DataSource = t; });
                    this.FormInstance.Invoke(d, value);
                }
                else
                {
                    this.FormInstance.dataGridView1.DataSource = value;
                }
            }
        }

        public override void UpdateStatus(string v)
        {
            this.StatusLabelText = v;
        }

        public override void UpdateStatus(bool v)
        {
            this.StatusLabelVisible = v;
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
            this.DataGridInstance = dt;

            // hide loading indicator
            this.StatusLabelVisible = false;
        }

        // process login output
        public override void PrintData(Mandoline.Api.Client.Models.User u, string token)
        {
            // append user info to list, so that we can pass to the DataGridView object
            var output = new List<Mandoline.Api.Client.Models.User>();
            output.Add(u);

            // pass user list to DataGridView object
            this.DataGridInstance = output;

            // hide loading indicator
            this.StatusLabelVisible = false;

            // change current user's access token
            AppConstants.ApiToken = token;
        }

        // process output for multi user response
        public override void PrintData(IEnumerable<Mandoline.Api.Client.Models.User> ul)
        {
            // pass user list to DataGridView object
            this.DataGridInstance = ul;

            // hide loading indicator
            this.StatusLabelVisible = false;
        }

        // process output for login response
        public override void PrintData(Mandoline.Api.Client.Models.User u)
        {
            Console.WriteLine("User has {0} saved selections...", u.SavedSelections.ToList().Count);
            foreach (ResourceLink<Selection> s in u.SavedSelections)
            {
                Console.WriteLine("{0}: {1}", s.Name, s.Id);
            }

            // append user info to list, so that we can pass to the DataGridView object
            var output = new List<Mandoline.Api.Client.Models.User>();
            output.Add(u);

            // pass user list to DataGridView object
            this.DataGridInstance = output;

            // hide loading indicator
            this.StatusLabelVisible = false;
        }

        // process output for list of databanks
        public override void PrintData(IEnumerable<Databank> ld)
        {
            // parse results into databanks list
            var output = new List<Databank>();
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
        public override void PrintData(VariableCollectionDto vc)
        {
            var dt = new Table.VariableTable();

            // pass list to DataGridView object
            foreach (VariableDto v in vc.Variables)
            {
                dt.AddRow(v);
            }

            this.DataGridInstance = dt;

            // hide loading indicator
            this.StatusLabelVisible = false;
        }

        // output for single string data output (catch-all option for anything that returns single point of data)
        public override void PrintData(string s)
        {
            var dt = new DataTable();
            dt.Columns.Add("Data");
            dt.Rows.Add(s);
            this.DataGridInstance = dt;

            // hide loading indicator
            this.StatusLabelVisible = false;
        }

        // shaped table output
        public override void PrintData(ShapedStreamResult result)
        {
            var dt = new Table.ShapeTable(result);
            this.DataGridInstance = dt;

            // hide loading indicator
            this.StatusLabelVisible = false;
        }

        // output for download request
        public override void PrintData(ControllerDownloadResponseDto response, string filename, string ready)
        {
            // create table for displaying selection data
            var dt = new Table.DownloadRequestTable();

            // check to see whether download is ready and process output
            dt.Rows.Add(filename, "CSV", response.ReadyUrl, ready);
            this.DataGridInstance = dt;

            // hide loading indicator
            this.StatusLabelVisible = false;
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
            this.DataGridInstance = dt;

            // hide loading indicator
            this.StatusLabelVisible = false;
        }
    }
}
