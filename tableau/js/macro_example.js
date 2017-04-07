
// <copyright file="index.html" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>


var API_URL = "https://services.oxfordeconomics.com";
var DEFAULT_SELECTION = "2c140fbb-4624-4004-927e-621734f3cb93";
var MEASURE_CODES = {
	"L": "Level values",
	"PY": "% change y/y",
	"DY": "Difference y/y"
};


// takes the user credentials provided by the user and attempts to 
// log in. success changes the api key input field to the one returned
// in the user object
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


// populates the tableau table with data based on an http request
// to the download endpoint. takes the table and doneCallback from
// the getData method and the api_key and selection_id, which are
// hopefully present when the submit button is pressed.
//
// this function is responsible for generating each row entry
// in the data table and then running the callback
function runQuery(table, doneCallback, api_key, selection_id) 
{
	if (!selection_id || !api_key)
	{
		$("#log").empty();
		$("#log").append("Error: missing api_key or selection_id");
	}

	var api_url = API_URL;
	var apiHeader = { "Api-Key": api_key };
	$.support.cors = true;
	var resource_address = encodeURI( api_url + '/api/download/' + selection_id + '?includeMetadata=true');
	$.ajax({
		url: resource_address,
		type: 'GET',
		headers: apiHeader,
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
					tableData.push({
						"LocationCode": data[i]['LocationCode'],
						"Location": data[i]['Metadata']['Location'],
						"VariableCode": data[i]['VariableCode'],
						"MeasureCode": MEASURE_CODES[data[i]['MeasureCode']],
						"Year": keys[x],
						"YearValue": data[i]['AnnualData'][keys[x]],
						"Quarter": null,
						"QuarterValue": null
					});
				}

				keys = Object.keys(data[i]['QuarterlyData']);
				for(x = 0; x < keys.length; x++)
				{
					console.log("adding " + keys[x] + " " + variable_code + " data row");
					tableData.push({
						"LocationCode": data[i]['LocationCode'],
						"Location": data[i]['Metadata']['Location'],
						"VariableCode": data[i]['VariableCode'],
						"MeasureCode": MEASURE_CODES[data[i]['MeasureCode']],
						"Quarter": keys[x],
						"QuarterValue": data[i]['QuarterlyData'][keys[x]],
						"Year": null,
						"YearValue": null
					});
				}
			}

			table.appendRows(tableData);
			doneCallback();
		},
		error: function(data, status){
		    console.log("Error with host " + api_url + "...");
		}
	});
};

var connector = tableau.makeConnector();

// called by tableau to set up the data table, which it will later 
// populate using the getData method of the user's connection object
connector.getSchema = function(schemaCallback)
{
	var cols = [];

	// add default columns
	cols.push({ id: "LocationCode", dataType: tableau.dataTypeEnum.string });
	cols.push({ id: "Location", dataType: tableau.dataTypeEnum.string });
	cols.push({ id: "VariableCode", dataType: tableau.dataTypeEnum.string });
	cols.push({ id: "MeasureCode", dataType: tableau.dataTypeEnum.string });
	cols.push({ id: "Quarter", dataType: tableau.dataTypeEnum.int });
	cols.push({ id: "QuarterValue", dataType: tableau.dataTypeEnum.float });
	cols.push({ id: "Year", dataType: tableau.dataTypeEnum.int });
	cols.push({ id: "YearValue", dataType: tableau.dataTypeEnum.float });

	// set up table meta data based on selection object
	var tableSchema = {
		id: "selection_data",
		alias: "Selection data from the Oxford Economics Global Data Workstation",
		columns: cols
	};

	schemaCallback([tableSchema]);
}


// called by tableau to populate the data table.
// this one simply takes the tableau.password and tableau.connectionData
// fields and passes them to the run query function
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

		// in order to save authentication information between steps
		// they must be set in the tableau object's values. typically,
		// the authetnication token or api key is kept in the 
		// tableau.password variable
		tableau.username = $("#Username").val();
		tableau.connectionData = $("#Selection").val();
		postLogin(tableau.username, $("#Password").val());
	});

	// assumes the user has authenticated and then runs the query based
	// on the selection id provided
	$("#submitButton").click(function()
	{
		tableau.password = $("#ApiKey").val();
		tableau.connectionName = "Global data workstation";
		tableau.submit();
	});
});
