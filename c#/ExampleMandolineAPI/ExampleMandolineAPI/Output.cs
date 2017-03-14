using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mandoline.Api.Client.ServiceModels;
using Mandoline.Api.Client.Models;
using System.Data;

namespace ExampleMandolineAPI 
{
    public abstract class Output
    {
        // handles load status indicator, text and visibility
        public abstract void UpdateStatus(bool v);
        public abstract void UpdateStatus(string v);

        // handles updating datagridview source
        public abstract void PrintData(List<DataseriesDto> ld);
        public abstract void PrintData(string s);
        public abstract void PrintData(ControllerDownloadResponseDto response, string filename, string s);
        public abstract void PrintData(ShapedStreamResult result);
        public abstract void PrintData(VariableCollectionDto vc);
        public abstract void PrintData(IEnumerable<Databank> ld);
        public abstract void PrintData(Mandoline.Api.Client.Models.User u);
        public abstract void PrintData(IEnumerable<Mandoline.Api.Client.Models.User> ul);
        public abstract void PrintData(Mandoline.Api.Client.Models.User u, string token);
        public abstract void PrintData(SelectionDto s);
    }

    // this implementation of Output directs output to a DataGridView
    // for a Windows Form application
    class GridOutput : Output
    {
        public Form1 f { get; set; } // to access the main form object

        // getters and setters for updating form fields
        private bool statusLabelVisible
        {   
            set
            {
                f.label1.Visible = value;
            }
        }

        // for updating status text
        private string statusLabelText
        {   
            set
            {
                if(value != null) f.label1.Text = value;
            }
        }

        // for updating the datagridview's data source
        private Object dgv {
            set
            {
                Console.WriteLine("Updating data grid...");
                f.dataGridView1.DataSource = value;
            }
        }

        // constructor, must take windows form object to update load status and gridview
        public GridOutput(Form1 f)
        {
            this.f = f;

        }

        public override void UpdateStatus(string v)
        {
            this.statusLabelText = v;
        }

        public override void UpdateStatus(bool v)
        {
            this.statusLabelVisible = v;
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
            dgv = dt;

            // hide loading indicator
            statusLabelVisible = false;
        }

        // process login output
        public override void PrintData(Mandoline.Api.Client.Models.User u, string token)
        {
            // append user info to list, so that we can pass to the DataGridView object
            var output = new List<Mandoline.Api.Client.Models.User>();
            output.Add(u);

            // pass user list to DataGridView object
            dgv = output;

            // hide loading indicator
            statusLabelVisible = false;

            // change current user's access token
            Settings.API_TOKEN = token;

        }

        // process output for multi user response
        public override void PrintData(IEnumerable<Mandoline.Api.Client.Models.User> ul)
        {
            // pass user list to DataGridView object
            dgv = ul;

            // hide loading indicator
            statusLabelVisible = false;

        }
                
        // process output for login response
        public override void PrintData(Mandoline.Api.Client.Models.User u)
        {
            Console.WriteLine("User has {0} saved selections...", u.SavedSelections.ToList().Count);
            foreach (ResourceLink<Selection> s in u.SavedSelections) Console.WriteLine("{0}: {1}", s.Name, s.Id);

            // append user info to list, so that we can pass to the DataGridView object
            var output = new List<Mandoline.Api.Client.Models.User>();
            output.Add(u);

            // pass user list to DataGridView object
            dgv = output;

            // hide loading indicator
            statusLabelVisible = false;

        }

        // process output for list of databanks
        public override void PrintData(IEnumerable<Databank> ld)
        {
            // parse results into databanks list
            var output = new List<Databank>();
            foreach(Databank m in ld)
            {
                output.Add(m);
            }

            // pass databanks list to DataGridView object
            dgv = output;

            // hide loading indicator
            statusLabelVisible = false;
        }

        // process output for collection of variables
        public override void PrintData(VariableCollectionDto vc)
        {
            var dt = new Table.VariableTable();

            // pass list to DataGridView object
            foreach(VariableDto v in vc.Variables) dt.AddRow(v);
            dgv = dt;

            // hide loading indicator
            statusLabelVisible = false;

        }

        // output for single string data output (catch-all option for anything that returns single point of data)
        public override void PrintData(string s)
        {
            var dt = new DataTable();
            dt.Columns.Add("Data");
            dt.Rows.Add(s);
            dgv = dt;

            // hide loading indicator
            statusLabelVisible = false;
        }

        // shaped table output
        public override void PrintData(ShapedStreamResult result)
        {
            var dt = new Table.ShapeTable(result);
            dgv = dt;

            // hide loading indicator
            statusLabelVisible = false;
        }

        // output for download request
        public override void PrintData(ControllerDownloadResponseDto response, string filename, string ready)
        {
            // create table for displaying selection data
            var dt = new Table.DownloadRequestTable();

            // check to see whether download is ready and process output
            dt.Rows.Add(filename, "CSV", response.ReadyUrl, ready);
            dgv = dt;

            // hide loading indicator
            statusLabelVisible = false;
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

                        }catch(Exception e)
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
            dgv = dt;

            // hide loading indicator
            statusLabelVisible = false;
        }

    }
}
