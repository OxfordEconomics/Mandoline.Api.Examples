
// <copyright file="macro_example.js" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>


var API_URL = "https://services.oxfordeconomics.com";
var DEFAULT_SELECTION = "847ef5ee-5b83-458d-b9e8-292ff7e93db0";
var MEASURE_CODES = {
	"L": "Level values",
	"PY": "% change y/y",
	"DY": "Difference y/y",
	"P": "Percentage q/q",
	"D": "Difference q/q",
	"GR": "Annualized value"
};



// makes a simple get request to any of our API endpoints
// takes an optional id as input, which is appended to the end
// of the request url appropriately
function getHttpResource(path, resource_id, callback, api_key) {
        resource_id = resource_id || "";

        if (resource_id)
        {
                resource_id = "/" + resource_id;
        }

        var hostname = $("#Hostname").val();
        var api_url = hostname + "/api";

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
        var api_key = $("#ApiKey").val();
        $("#log").empty();
        $("#log").append("Fetching selection information for s_id=" + id);
        $("#query_info").empty();
        getHttpResource('/savedselections', id, function(data)
        {
                $("#log").empty();
                $("#query_info").append('<h3>Selection settings</h3>');
                $("#query_info").append('<pre>' + JSON.stringify(data, null, 3)
                        + '</pre>');	
                $('#submit-div').css('display','block');
        }, api_key);
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

        $("#selections_div").append("<h2>2. Select query</h2>");

	for (var i = 0; i < selections_list.length; i++)
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

                var name = selections_list[i]['Name'];
                if (name.length > 25)
                {
                    name = name.substring(0, 25);
                    name = name + "...";
                }
		$("#label_"+i).append(" " + name + "<br />");

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
		id: 'label_other'
	});
	$("#radio_other").after(new_label);
	$("#label_other").append(" Other: ");
        $("#radio_other").click(function()
        {
            $("#query_info").empty();
            $('#submit-div').css('display','block');
        });

	var new_input = $('<input />',
	{
		id: "input_other",
		placeholder: "enter selection id"
	});	
	$("#label_other").append(new_input);
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
function postLogin(username, password, input_key, callback)
{
	$("#selections_div").empty();
	$("#selections_div").css("display", "none");

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
                            if (callback)
                            {
                                callback(data, status);
                            }
			},
			error: function(data,status){
                            if (callback)
                            {
                                callback(data, status);
                            }
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
                            if (callback)
                            {
                                callback(data, status);
                            }
			},
			error: function(data,status){
                            if (callback)
                            {
                                callback(data, status);
                            }
			}
		});
	}

        $.support.cors = true;

}


// takes a single download dictionary and a period and returns a 
// a single row of data corresponding to several years of annual
// data points or a single quarter across several years
function buildRow(download_entry, period)
{
    var new_row = new Object();

    period = period || "Annual";

    new_row["Location"] = download_entry['Metadata']['Location'];
    new_row["Indicator"] = download_entry['Metadata']['IndicatorName'];
    new_row["Units"] = download_entry['Metadata']['Units'];
    new_row["Scale"] = download_entry['Metadata']['ScaleFactor'];
    new_row["Measurement"] = MEASURE_CODES[download_entry['MeasureCode']];
    new_row["Source"] = download_entry['Metadata']['Source'];
    new_row["Seasonal"] = download_entry['Metadata']['SeasonallyAdjusted'];
    new_row["BaseYearPrice"] = download_entry['Metadata']['BaseYearPrice'];
    new_row["BaseYearIndex"] = download_entry['Metadata']['BaseYearIndex'];
    new_row["HistoricalEndYear"] = download_entry['Metadata']['HistoricalEndYear'];
    new_row["HistoricalEndQuarter"] = download_entry['Metadata']['HistoricalEndQuarter'];
    new_row["DateOfLastUpdate"] = download_entry['Metadata']['LastUpdate'];
    new_row["SourceDetails"] = download_entry['Metadata']['SourceDetails'];
    new_row["AdditionalSourceDetails"] = download_entry['Metadata']['AdditionalSourceDetails'];
    new_row["LocationCode"] = download_entry['LocationCode'];
    new_row["IndicatorCode"] = download_entry['VariableCode'];

    if (period == "Annual")
    {
        new_row["Period"] = period;
        var keys = Object.keys(download_entry['AnnualData']);
        for(var x = 0; x < keys.length; x++)
        {
            var key = "" + keys[x];
            new_row[key] = download_entry['AnnualData'][key];
        }
    }
    else
    {
        new_row["Period"] = "Qtr " + period;

        // iterate through each key-data pair in the quarterly
        // data list
        var keys = Object.keys(download_entry['QuarterlyData']);
        for(var x = 0; x < keys.length; x++)
        {
            var key = "" + keys[x];
            if (key.charAt(5) == ("" + period))
            {
                new_row[key.substr(0, 4)] = download_entry['QuarterlyData'][key];
            }
        }
    }

    new_id = new_row["LocationCode"] + "_" +
        new_row["IndicatorCode"] + "_" +
        new_row["Measurement"] + "_" +
        new_row["Period"];
    new_id = new_id.toUpperCase(); 
    new_id = new_id.replace(/\s/g,"");
    new_row["Id"] = new_id;
    

    return new_row;
}

function isEmpty(test_dict)
{
    return Object.keys(test_dict).length == 0;
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
        success: function(data, status)
        {
            var tableData = [];
            for (var i = 0; i < data.length; i++)
            {
                console.log("new data row...");
                var variable_code = data[i]['VariableCode'];
               
                // build annual data
                if (!isEmpty(data[i]['AnnualData']))
                {
                    tableData.push(buildRow(data[i], "Annual"));
                }

                // build quarterly data
                if (!isEmpty(data[i]['QuarterlyData']))
                {
                    for (var x = 1; x < 5; x++)
                    {
                        tableData.push(buildRow(data[i], x));
                    }
                }
            }

            table.appendRows(tableData);
            doneCallback();
        },
        error: function(data, status)
        {
            console.log("Error with host " + api_url + "...");
        }
    });
};

var connector = tableau.makeConnector();

// called by tableau to set up the data table, which it will later 
// populate using the getData method of the user's connection object
connector.getSchema = function(schemaCallback)
{
    getHttpResource('/savedselections', tableau.connectionData, function(data)
    {
        var cols = [];

        // add default columns
        cols.push({ id: "Id", dataType: tableau.dataTypeEnum.string });
        cols.push({ id: "Location", dataType: tableau.dataTypeEnum.string });
        cols.push({ id: "Indicator", dataType: tableau.dataTypeEnum.string });
        cols.push({ id: "Units", dataType: tableau.dataTypeEnum.string });
        cols.push({ id: "Scale", dataType: tableau.dataTypeEnum.string });
        cols.push({ id: "Measurement", dataType: tableau.dataTypeEnum.string });
        cols.push({ id: "Period", dataType: tableau.dataTypeEnum.string });

        // create a new column for each year in annual data
        console.log("Creating year columns..."); 
        for(var i = parseInt(data['StartYear']); i <= parseInt(data['EndYear']); i++)
        {
            console.log("Creating column for year " + i + "..."); 
            cols.push({ id: "" + i, dataType: tableau.dataTypeEnum.float });
        }

        cols.push({ id: "Source", dataType: tableau.dataTypeEnum.string });

        cols.push({ 
            id: "Seasonal", 
            alias: "Seasonally adjusted", 
            dataType: tableau.dataTypeEnum.bool 
        });

        cols.push({ 
            id: "BaseYearPrice", 
            alias: "Base year price", 
            dataType: tableau.dataTypeEnum.string
        });

        cols.push({ 
            id: "BaseYearIndex", 
            alias: "Base year index", 
            dataType: tableau.dataTypeEnum.string
        });

        cols.push({ 
            id: "HistoricalEndYear", 
            alias: "Historical end year", 
            dataType: tableau.dataTypeEnum.string 
        });

        cols.push({ 
            id: "HistoricalEndQuarter", 
            alias: "Historical end quarter", 
            dataType: tableau.dataTypeEnum.int
        });

        cols.push({ 
            id: "DateOfLastUpdate", 
            alias: "Date of last update", 
            dataType: tableau.dataTypeEnum.date 
        });

        cols.push({ 
            id: "SourceDetails", 
            alias: "Source details", 
            dataType: tableau.dataTypeEnum.string 
        });

        cols.push({ 
            id: "AdditionalSourceDetails", 
            alias: "Additional source details", 
            dataType: tableau.dataTypeEnum.string 
        });

        cols.push({ 
            id: "LocationCode", 
            alias: "Location code", 
            dataType: tableau.dataTypeEnum.string 
        });

        cols.push({ 
            id: "IndicatorCode", 
            alias: "Indicator code", 
            dataType: tableau.dataTypeEnum.string 
        });

        // set up table meta data based on selection object
        var tableSchema = {
                id: "selection_data",
                alias: "Selection data from the Oxford Economics Global Data Workstation",
                columns: cols
        };

        schemaCallback([tableSchema]);
    }, tableau.password) 
}


// called by tableau to populate the data table.
// this one simply takes the tableau.password and tableau.connectionData
// fields and passes them to the run query function
connector.getData = function(table, doneCallback) 
{
    runQuery(table, doneCallback, tableau.password, tableau.connectionData);
}


tableau.registerConnector(connector);

function loginHandler(callback)
{
    $("#Password").prop("disabled", true);
    $("#loginButton").prop('disabled', true);
    $("#log").empty();

    // in order to save authentication information between steps
    // they must be set in the tableau object's values. typically,
    // the authetnication token or api key is kept in the 
    // tableau.password variable
    tableau.username = $("#Username").val();
    postLogin(tableau.username, $("#Password").val(), $("#ApiKey").val(), function(data, status)
    {
        $("#log").empty();
        if ( status == "success")
        {
            $("#ApiKey").val(data["ApiKey"]);
            setupSelections(data['SavedSelections']);
            $("#selections_div").css("display", "block");
        }
        else
        {
            $("#log").append("Error: couldn't validate credentials");
        }

        if (callback)
        {
            callback();
        }

        $("#loginButton").prop("disabled", false);
        $("#Password").prop("disabled", false);
    });
}


$(document).ready(function()
{
	$("#Selection").val(DEFAULT_SELECTION);
	$("#loginButton").click(function()
	{
            loginHandler();
	});

	$("#Password").keypress(function(e)
        {
            if (e.which == 13)
            {
                loginHandler();
            }
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
