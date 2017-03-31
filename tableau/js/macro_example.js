var API_URL = "https://services.oxfordeconomics.com";
// var API_KEY; = "566dbac8-d0c2-4248-a0ed-ca3a8ce4df5c";

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
    Order: 'LocationIndicator',
    GroupingMode: 'false',
    Regions: [ ],
    Variables: [{ ProductTypeCode: 'WMC', VariableCode: 'GDP$', MeasureCodes: ['PY'] }, 
	        { ProductTypeCode: 'WMC', VariableCode: 'CPI', MeasureCodes: ['PY'] } ] 
};

function runQuery(table, doneCallback, api_key) 
{
	var api_url = API_URL;
	var jsonResource = JSON.stringify(simple_macro_selection);
	var apiHeader = { "Api-Key": api_key };
	$.support.cors = true;
	var resource_address = encodeURI( api_url + '/api/download?includeMetadata=true');
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
				console.log("new data row...");
				var variable_code = data[i]['VariableCode'];

				var keys = Object.keys(data[i]['AnnualData']);

				for(x = 0; x < keys.length; x++)
				{
					console.log("adding " + keys[x] + " " + variable_code + " data row");

					if (variable_code == 'GDP$'){
						gdp_value = data[i]['AnnualData'][keys[x]];
						cpi_value = null;
					}
					else if (variable_code == 'CPI'){
						cpi_value = data[i]['AnnualData'][keys[x]];
						gdp_value = null;
					}
					else
					{
						gdp_value = null;
						cpi_value = null;
					}
					tableData.push({
						"LocationCode": data[i]['LocationCode'],
						"CountryName": data[i]['Metadata']['Location'],
						"Year": keys[x],
						"GDP": gdp_value,
						"CPI": cpi_value
					});
				}
			}

			table.appendRows(tableData);
			doneCallback();
		},
		error: function(data, status){
		    console.log("Error using host=" + api_url + " with key=" + api_key);
		}
	});
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
			id: "Year",
			dataType: tableau.dataTypeEnum.int
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
        var hostname = API_URL;
        var api_url = hostname + "/api";
	var user_data = {
	    UserName: tableau.username,
	    Password: tableau.password
	};
        var jsonResource = JSON.stringify(user_data);
        $.support.cors = true;


        $.ajax({
                url: encodeURI(hostname + '/api/users'),
                type: 'POST',
                data: jsonResource,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(data, status){
			runQuery(table, doneCallback, data["ApiKey"]);
                },
                error: function(data,status){
                    alert("Error logging in. Please check your credentials and try again.");
                }
        });
}

tableau.registerConnector(connector);

$(document).ready(function(){
	$("#submitButton").click(function()
	{
		tableau.username = $("#Username").val();
		tableau.password = $("#Password").val();
		tableau.connectionName = "Global data workstation";
		tableau.submit();
	});
});
