const API_URL = "https://services.oxfordeconomics.com/api";
const API_KEY = "566dbac8-d0c2-4248-a0ed-ca3a8ce4df5c";

var simple_macro_selection = 
{
    Name: 'Simple macro selection',
    DatabankCode: 'WDMacro',
    MeasureCode: 'L',
    StartYear: 2016,
    EndYear: 2026,
    StackedQuarters: 'false',
    Frequency: 'Annual',
    Sequence: 'EarliestToLatest',
    Precision: 1,
    Order: 'IndicatorLocation',
    GroupingMode: 'false',
    Regions: [],
    Variables: [{ ProductTypeCode: 'WMC', VariableCode: 'GDP$', MeasureCodes: ['PY'] }, { ProductTypeCode: 'WMC', VariableCode: 'CPI', MeasureCodes: ['PY'] }] 
};

var connector = tableau.makeConnector();

connector.getSchema = function(schemaCallback)
{
	var cols = [
		{ 
			id: "LocationCode",
			dataType: tableau.dataTypeEnum.string 
		},
		{ 
			id: "CountryName",
			dataType: tableau.dataTypeEnum.string
		},
		{ 
			id: "GDP",
			dataType: tableau.dataTypeEnum.float
		},
		{ 
			id: "CPI",
			dataType: tableau.dataTypeEnum.float
		}
	];

	var tableSchema = {
		id: "simplemacropull",
		alias: "2016 GDP & CPI by country",
		columns: cols
	};

	schemaCallback([tableSchema]);
}

connector.getData = function(table, doneCallback) {
	var jsonResource = JSON.stringify(simple_macro_selection);
	var apiHeader = { "Api-Key": $("#ApiKey").val() };
	$.support.cors = true;
	var resource_address = encodeURI($("#Hostname").val() + '/download?includeMetadata=true');

	$.ajax({
		url: resource_address,
		type: 'POST',
		headers: apiHeader,
		data: jsonResource,
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',
		success: function(data, status){
			var tableData = [];
			for (i = 0; i < data.length; i++)
			{
				tableData.push({
					"LocationCode": data[i]['LocationCode'],
					"CountryName": data[i]['Metadata']['Location'],
					"GDP": data[i]['AnnualData']['2016'],
					"CPI": data[i]['AnnualData']['2016']
				});
			}

			table.appendRows(tableData);
			doneCallback();
		},
		error: function(data, status){
		    console.log("Error - status: " + status);
		}
	});
}

tableau.registerConnector(connector);

$(document).ready(function(){
	$("#submitButton").click(function()
	{
		// var xmlData = $.ajax({
		// 	url: "AppSettings.config",
		// 	async: false
		// }).responseText;
		// $("#log").val(xmlData);
		

		tableau.connectionName = "Global data workstation";
		tableau.submit();
	});
});
