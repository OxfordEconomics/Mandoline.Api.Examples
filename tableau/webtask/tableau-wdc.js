// <!--
// <copyright file="tableau-wdc.js" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>
// -->

var view = (function view() 
{/*<html>
<head>
	<title>Macro databank example</title>
	<meta http-equiv="Cache-Control" content="no-store" />

	<link href="tableau-wdc/css/styles.css" type="text/css" rel="stylesheet"/>
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" type="text/javascript"></script>
	<script src="https://connectors.tableau.com/libs/tableauwdc-2.2.latest.js" type="text/javascript"></script>
	<script src="tableau-wdc/js/macro_example.js" type="text/javascript"></script>
</head>

<body>

	<div class="main">

	<div class="header">

	<img id="logo" src="https://d1iydh3qrygeij.cloudfront.net/Media/Default/landing-pages/oelogo1.JPG" />
	<h2>Tableau Web Data Connector</h2>

	</div>

	<div class="left-div">
	
	<h2>Base URL:</h2>
	Hostname: <input id = "Hostname" style="width: 175px; margin-right: 15px;"
		  value = "https://services.oxfordeconomics.com"/><br/>

	<h2>Authentication:</h2>
	<input id = "ApiKey" style="width: 175px; float:right; margin-right:25px;" value = ""/>
	<p style="margin-top: 12px;">API key:</p>
	<p style="margin: 0px 0px 7px 0px;">-- or --</p>
	<input id = "Username" style="width: 175px; float:right; margin-right:25px;"/>
	<p style="margin-top: 14px; margin-bottom: 0px;">User name:</P>
	<input id = "Password" type="password" style="width: 175px; margin-right:25px; float:right;"/>
	<p style="margin-top: 7px; margin-bottom: 7px;">Password:</p>
	<button id="loginButton" type="button" style="width:80px; margin-left: 190px;">Log in</button>
	<br />

	</div>
	
	<div class="middle-div">
	<h2 id='query_label'>Query:</h2>
	<div id="selections_div" style="display:none;">
	
	</div>
	<button id="submitButton" type="button" style='margin-top: 7px; display:none;'>Run query</button>
	</div>
	
	<div class="right-div">
	<h2>Query information:</h2>
	<div id="query_info"></div>
	</div>
	
	

	<div style="clear:both"></div>

	<h2>Log:</h3>

	<div id = "log"></div>

	</div>
</body>

</html>*/}).toString().match(/[^]*\/\*([^]*)\*\/\s*\}$/)[1];


var css = (function css() 
{/*
h2 {
   margin-bottom: 7px;
}


body { 
	background-color: lightgrey; 
	font-family: Arial;
	}

.main { 
	background-color: #FEFEFE;
	margin-top: -15px;
	padding: 50px;
	padding-top: 15px;
	padding-right:15px;
	margin-left: auto;
	margin-right: auto;
	width: 1125px;
	overflow: hidden;
	}

.middle-div {
	width: 285px;
	margin-left: 15px;
	float: left;
	}
	
.right-div {
	width: 285px;
	padding-right: 15px;
	margin-left: 15px;
	float: left;
	}

.left-div {
	width: 300px;
	float: left;
}

.header {
	width: 1065px;
	overflow: hidden;
	margin-bottom: 25px;
}

.header h2 {
	font-size:35px;
	margin-top: 60px;
	float: right;
}

#copyright {
	font-size: 12px;
	}

#logo {
	float: left;
	margin-left: -28px;
}
}*/}).toString().match(/[^]*\/\*([^]*)\*\/\s*\}$/)[1];

var js = (function js() 
{/*
var API_URL = "https://services.oxfordeconomics.com";
var DEFAULT_SELECTION = "847ef5ee-5b83-458d-b9e8-292ff7e93db0";
var MEASURE_CODES = {
	"L": "Level values",
	"PY": "% change y/y",
	"DY": "Difference y/y"
};



// makes a simple get request to any of our API endpoints
// takes an optional id as input, which is appended to the end
// of the request url appropriately
function getHttpResource(path, resource_id, callback) {
        resource_id = resource_id || "";

        if (resource_id)
        {
                resource_id = "/" + resource_id;
        }

        var hostname = $("#Hostname").val();
        var api_url = hostname + "/api";
        var api_key = $("#ApiKey").val();

        var apiHeader = { "Api-Key": api_key };

        $.support.cors = true;

        var request = $.ajax({
                url: encodeURI(api_url + path + resource_id),
                type: 'GET',
                headers: apiHeader,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(data, status){
			callback(data);
                },
                error: function(data, textStatus, errorThrown){
			callback(data);
                }
        });
};


// gets the selection information from the savedselections endpoint using
// the given selection id
function fillQueryInfo(id)
{
	return function()
	{
		$("#log").empty();
		$("#log").append("Fetching selection information for s_id=" + id);
		$("#query_info").empty();
		getHttpResource('/savedselections', id, function(data)
		{
                	$("#log").empty();
			$("#query_info").append('<pre>' + JSON.stringify(data, null, 3)
		       		+ '</pre>');	
		});
	}	
}


// takes a list of selections and populates an interface with the
// options the user can run in this query
function setupSelections(selections_list)
{
	if (!selections_list)
	{
		return;
	}

	for (i = 0; i < selections_list.length; i++)
	{
		var new_selection = $('<input />',
		{
			id: 'radio_' + i,
			type: 'radio',
			name: 'selection_choice',
			value: selections_list[i]['Id'],
		});

		$("#selections_div").append(new_selection);

		var new_label = $('<label />', 
		{
			id: 'label_' + i
		});

		$("#radio_"+i).after(new_label);
		$("#label_"+i).append(" " + selections_list[i]['Name'] + "<br />");

		$("#radio_"+i).click(fillQueryInfo(selections_list[i]['Id']));
	}

	var new_selection = $('<input />',
	{
		id: 'radio_other',
		type: 'radio',
		name: 'selection_choice',
		value: 'other'
	});
	$("#selections_div").append(new_selection);

	var new_label = $('<label />', 
	{
		id: 'label_other',
	});
	$("#radio_other").after(new_label);
	$("#label_other").append(" Other: ");

	var new_input = $('<input />',
	{
		id: "input_other",
		placeholder: "enter selection id"
	});	
	$("#label_other").append(new_input);


	$('#submitButton').css('display','block');
}


// checks for user's selection choice and returns its Id
function getSelectionId()
{
	var checked_radio = $("input[name=selection_choice]:checked").val();

	if (checked_radio != 'other')
	{
		return checked_radio;
	}

	return $("#input_other").val();
}



// takes the user credentials provided by the user and attempts to 
// log in. success changes the api key input field to the one returned
// in the user object and populates the list of available selections
function postLogin(username, password, input_key)
{
	$("#selections_div").empty();
	$("#selections_div").css("display", "none");
	$("#submitButton").css("display", "none");

        var hostname = API_URL;
        var api_url = hostname + "/api";

	if (input_key)
	{
		$("#log").append("Validating by key...");

		var headers = 
		{
			"Api-Key": input_key
		};

		$.ajax({
			url: encodeURI(hostname + '/api/users/me'),
			type: 'GET',
			headers: headers,
			contentType: 'application/json; charset=utf-8',
			dataType: 'json',
			success: function(data, status){
				$("#ApiKey").val(data["ApiKey"]);
				$("#log").empty();
				setupSelections(data['SavedSelections']);
				$("#selections_div").css("display", "block");
				$("#submitButton").css("display", "block");
			},
			error: function(data,status){
				$("#log").empty();
				$("#log").append("Error: couldn't validate credentials");
			}
		});
	}
	else
	{
		$("#log").append("Validating by credentials...");

		var user_data = {
		    UserName: username,
		    Password: password
		};

		var jsonResource = JSON.stringify(user_data);

		$.ajax({
			url: encodeURI(hostname + '/api/users'),
			type: 'POST',
			data: jsonResource,
			contentType: 'application/json; charset=utf-8',
			dataType: 'json',
			success: function(data, status){
				$("#ApiKey").val(data["ApiKey"]);
				$("#log").empty();
				setupSelections(data['SavedSelections']);
				$("#selections_div").css("display", "block");
				$("#submitButton").css("display", "block");
			},
			error: function(data,status){
				$("#log").empty();
				$("#log").append("Error: couldn't validate credentials");
			}
		});
	}

        $.support.cors = true;

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

		// in order to save authentication information between steps
		// they must be set in the tableau object's values. typically,
		// the authetnication token or api key is kept in the 
		// tableau.password variable
		tableau.username = $("#Username").val();
		postLogin(tableau.username, $("#Password").val(), $("#ApiKey").val());
	});

	// assumes the user has authenticated and then runs the query based
	// on the selection id provided
	$("#submitButton").click(function()
	{
		tableau.connectionData = getSelectionId();
		tableau.password = $("#ApiKey").val();
		tableau.connectionName = "Global data workstation";
		tableau.submit();
	});
});
*/}).toString().match(/[^]*\/\*([^]*)\*\/\s*\}$/)[1];

var Express = require('express');
var Webtask = require('webtask-tools');
var app = Express();

app.get('/', function(req, res)
{
	res.send(view);
});

app.get('/css/styles.css', function(req, res)
{
	res.send(css);
});

app.get('/js/macro_example.js', function(req, res)
{
	res.send(js);
});

module.exports = Webtask.fromExpress(app);
