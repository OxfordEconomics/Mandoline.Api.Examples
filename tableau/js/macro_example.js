var API_URL = "https://services.oxfordeconomics.com";
var DEFAULT_SELECTION = "2c140fbb-4624-4004-927e-621734f3cb93";

function postLogin(username, password)
{
        var hostname = API_URL;
        var api_url = hostname + "/api";
	var user_data = {
	    UserName: username,
	    Password: password
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
			$("#ApiKey").val(data["ApiKey"]);
			$("#log").empty();
                },
                error: function(data,status){
			$("#log").empty();
			$("#log").append("Error: couldn't validate credentials");
                }
        });
}


function runQuery(table, doneCallback, api_key, selection_id) 
{
	if (!selection_id || !api_key)
	{
		$("#log").empty();
		$("#log").append("Error: missing api_key or selection_id");
	}

	var api_url = API_URL;
	var jsonResource = JSON.stringify(simple_macro_selection);
	var apiHeader = { "Api-Key": api_key };
	$.support.cors = true;
	var resource_address = encodeURI( api_url + '/api/download/' + selection_id + '?includeMetadata=true');
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


connector.getData = function(table, doneCallback) 
{
	runQuery(table, doneCallback, tableau.password, tableau.connectionData);
}



tableau.registerConnector(connector);

$(document).ready(function()
{
	$("#Selection").val(DEFAULT_SELECTION);
	$("#loginButton").click(function()
	{
		$("#log").empty();
		$("#log").append("Validating credentials...");
		tableau.username = $("#Username").val();
		tableau.connectionData = $("#Selection").val();
		postLogin(tableau.username, $("#Password").val());
	});

	$("#submitButton").click(function()
	{
		tableau.password = $("#ApiKey").val();
		tableau.connectionName = "Global data workstation";
		tableau.submit();
	});
});
