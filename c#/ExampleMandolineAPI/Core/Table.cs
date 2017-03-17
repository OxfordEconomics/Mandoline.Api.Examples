using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Mandoline.Api.Client.Models;
using Mandoline.Api.Client.ServiceModels;

namespace Core 
{
    public class Table
    {
        // DataTable for displaying Selection objects in DataGridView
        public class SelectionTable : DataTable
        {
            public SelectionTable()
            {
                this.Columns.Add("Id");
                this.Columns.Add("Name");
                this.Columns.Add("DatabankCode");
                this.Columns.Add("MeasureCode");
                this.Columns.Add("DownloadUrl");

            } 

        }

        // DataTable for displaying Download objects in DataGridView
        public class DownloadTable : DataTable
        {
            public DownloadTable()
            {
                this.Columns.Add("DatabankCode");
                this.Columns.Add("MeasureCode");
                this.Columns.Add("Region");
                this.Columns.Add("Year");
                this.Columns.Add("YearValue");
                this.Columns.Add("Quarter");
                this.Columns.Add("QuarterValue");
            }
        }

        // DataTable for displaying Variable objects in DataGridView
        public class VariableTable : DataTable
        {
            public VariableTable()
            {
                this.Columns.Add("DatabankCode");
                this.Columns.Add("ProductTypeCode");
                this.Columns.Add("VariableCode");
                this.Columns.Add("VariableName");
                this.Columns.Add("SectorCode");
                this.Columns.Add("SectorName");
            }

            public void AddRow(VariableDto var)
            {
                this.Rows.Add(var.DatabankCode, var.ProductTypeCode, var.VariableCode, var.VariableName);
            }

        }

        // DataTable for displaying Download Request objects in DataGridView
        public class DownloadRequestTable : DataTable
        {
            public DownloadRequestTable()
            {
                this.Columns.Add("Filename");
                this.Columns.Add("File format");
                this.Columns.Add("Ready URL");
                this.Columns.Add("Download ready");
            }
        }

        // DataTable for displaying shaped data rows in DataGridView
        public class ShapeTable : DataTable
        {
            public ShapeTable(ShapedStreamResult result)
            {

                // add each row of data
                foreach (IEnumerable<ShapeCellDto> r in result.Rows)
                {
                    // make column headers
                    if(this.Columns.Count == 0)
                    {
                        foreach (ShapeCellDto c in r)
                            this.Columns.Add(c.Value);
                    }

                    // add new data row
                    else
                    {
                        var newList = new List<string>();

                        foreach (ShapeCellDto c in r)
                            newList.Add(c.Value);

                        var newRow = this.Rows.Add(newList.ToArray());;

                    }
                }
            }

        }

    }
}
