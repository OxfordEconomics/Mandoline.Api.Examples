﻿// <copyright file="Table.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Data;
using Core.Client.Models;
using Core.Client.ServiceModels;

namespace Core;

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

    // DataTable for displaying Download objects in DataGridView
    // adds column for page number
    public class PagedDownloadTable : DataTable
    {
        public PagedDownloadTable()
        {
            this.Columns.Add("Page");
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

    // DataTable for displaying Variable objects in DataGridView
    public class RegionTable : DataTable
    {
        public RegionTable()
        {
            this.Columns.Add("DatabankCode");
            this.Columns.Add("RegionCode");
            this.Columns.Add("RegionName");
        }

        public void AddRow(RegionDto region)
        {
            this.Rows.Add(region.DatabankCode, region.RegionCode, region.Name);
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
                if (this.Columns.Count == 0)
                {
                    foreach (ShapeCellDto c in r)
                    {
                        this.Columns.Add(c.Value);
                    }
                }

                // add new data row
                else
                {
                    List<string> newList = new List<string>();

                    foreach (ShapeCellDto c in r)
                    {
                        newList.Add(c.Value);
                    }

                    DataRow newRow = this.Rows.Add(newList.ToArray());
                }
            }
        }
    }
}
