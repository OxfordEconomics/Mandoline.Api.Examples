using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client.ServiceModels;
using Mandoline.Api.Client.Models;
using System.Data;
using Core;

namespace Tests
{

    // this implementation of Output directs output to console
    class TestOutput : Output
    {

        // value to check while testing 
        public string returnValueStr;
        public int returnValueInt;
        public DateTime returnValueDate;

        // ensure that api operations are performed synchronously
        public TestOutput()
        {
            this.isAsync = false;
            returnValueStr = string.Empty;
            returnValueInt = -1;
            returnValueDate = DateTime.MinValue;
        }

        // for updating status text
        private string statusLabelText
        {   
            set
            {
                Console.WriteLine(value);
            }
        }

        public override void UpdateStatus(string v)
        {
            this.statusLabelText = v;
        }

        public override void UpdateStatus(bool v)
        {
        }

        public void printTable(DataTable table)
        {
            foreach(DataRow row in table.Rows)
            {
                for(int x = 0; x < table.Columns.Count; x++)
                {
                    if (x != 0 && row[x].ToString() != "") Console.Write(" - ");
                    Console.Write("{0}", row[x].ToString());
                }
                Console.WriteLine();
            }
        }

        // updates gridview with selection object information
        public override void PrintData(SelectionDto s)
        {
            this.returnValueDate = s.LastUpdate;
            this.returnValueStr = s.Id.ToString();
        }

        // process login output
        public override void PrintData(Mandoline.Api.Client.Models.User u, string token)
        {
            this.returnValueStr = token;
        }

        // process output for multi user response
        public override void PrintData(IEnumerable<Mandoline.Api.Client.Models.User> ul)
        {
            foreach (Mandoline.Api.Client.Models.User u in ul) Console.WriteLine("\t{0} {1} - Selections: {2}", u.FirstName, u.LastName, u.SavedSelections.Count());
        }
                
        // process output for single user
        public override void PrintData(Mandoline.Api.Client.Models.User u)
        {
            this.returnValueStr = u.ApiKey;
        }

        // process output for list of databanks
        public override void PrintData(IEnumerable<Databank> ld)
        {
            this.returnValueInt = ld.Count();
        }

        // process output for collection of variables
        public override void PrintData(VariableCollectionDto vc)
        {
            returnValueInt = vc.Variables.Count();
        }

        // output for single string data output (catch-all option for anything that returns single point of data)
        public override void PrintData(string s)
        {
            this.returnValueStr = s;
        }

        // shaped table output
        public override void PrintData(ShapedStreamResult result)
        {
            var dt = new Table.ShapeTable(result);
            printTable(dt);

        }

        // output for download request
        public override void PrintData(ControllerDownloadResponseDto response, string filename, string ready)
        {
            this.returnValueStr = response.ReadyUrl;
        }

        // output for downloads
        public override void PrintData(List<DataseriesDto> ld)
        {
            this.returnValueInt = ld.Count();
        }

    }
}
