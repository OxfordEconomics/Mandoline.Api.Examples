namespace Tests
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

        public override void UpdateStatus(string v)
        {
            this.StatusLabelText = v;
        }

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
        public override void PrintData(SelectionDto s)
        {
            this.ReturnValueDate = s.LastUpdate;
            this.ReturnValueStr = s.Id.ToString();
        }

        // process login output
        public override void PrintData(Mandoline.Api.Client.Models.User u, string token)
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
        public override void PrintData(IEnumerable<Mandoline.Api.Client.Models.User> ul)
        {
            foreach (Mandoline.Api.Client.Models.User u in ul)
            {
                Console.WriteLine("\t{0} {1} - Selections: {2}", u.FirstName, u.LastName, u.SavedSelections.Count());
            }
        }

        // process output for single user
        public override void PrintData(Mandoline.Api.Client.Models.User u)
        {
            this.ReturnValueStr = u.ApiKey;
        }

        // process output for list of databanks
        public override void PrintData(IEnumerable<Databank> ld)
        {
            this.ReturnValueInt = ld.Count();
        }

        // process output for collection of variables
        public override void PrintData(VariableCollectionDto vc)
        {
            this.ReturnValueInt = vc.Variables.Count();
        }

        // output for single string data output (catch-all option for anything that returns single point of data)
        public override void PrintData(string s)
        {
            this.ReturnValueStr = s;
        }

        // shaped table output
        public override void PrintData(ShapedStreamResult result)
        {
            this.ReturnValueInt = result.Rows.Count();
        }

        // output for download request
        public override void PrintData(ControllerDownloadResponseDto response, string filename, string ready)
        {
            this.ReturnValueStr = response.ReadyUrl;
        }

        // output for downloads
        public override void PrintData(List<DataseriesDto> ld)
        {
            this.ReturnValueInt = ld.Count();
        }
    }
}
