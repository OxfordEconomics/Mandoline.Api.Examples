// <copyright file="api_examples.js" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

// this is the number of characters into the URL string to find the
// generate link id for file and queue download
GENERATE_LINK_INDEX = 54;


WEBTASK_URL = 'https://wt-ec7f34285e98bfc354c433be6ef8e7c3-0.run.webtask.io/file-download';


// sample selection: when run, this will download GDP and inflation
// data for the UK and United States from the year 2015 to 2021 from
// the global macro databank
var sample_selection =
{
	Name: 'Sample selection template',
	DatabankCode: 'WDMacro',
	MeasureCode: 'L',
	StartYear: 2015,
	EndYear: 2021,
	StackedQuarters: 'false',
	Frequency: 'Annual',
	Sequence: 'EarliestToLatest',
	Precision: 1,
	Order: 'IndicatorLocation',
	GroupingMode: 'false',
	Regions: [{ DatabankCode: 'WDMacro', RegionCode: 'GBR' }, 
		{ DatabankCode: 'WDMacro', RegionCode: 'USA' }],
	
	// note: the measure code field in each variable object
	// L - Flat annual value
	// PY - Percentage year/year
	// DY - Difference year to year
	// P - Percentage quarter/quarter
	// D - Difference quarter to quarter
	// GR - Annualized data
	Variables: [{ ProductTypeCode: 'WMC', VariableCode: 'GDP$', MeasureCodes: ['L', 'PY', 'DY'] }, 
		{ ProductTypeCode: 'WMC', VariableCode: 'CPI', MeasureCodes: ['L'] }]
};


// makes a POST request to any endpoint in the API. taks as its
// arguments the path of the endpoint, a resource to be sent in
// the post body, and an optional resource id, which, when set, 
// is passed along in the url of the request
function postHttpResource(path, resource, resource_id, callback) {
	resource_id = resource_id || '';
	var hostname = $("#Hostname").val();
	var api_url = hostname + "/api";
	var api_key = $("#ApiKey").val();
	if (resource_id)
	{
		path = path + "/" + resource_id;
	}

	var jsonResource = JSON.stringify(resource);
	var apiHeader = { "Api-Key": api_key };
	
	$.support.cors = true;
	
	$.ajax({
		url: encodeURI(api_url + path),
		type: 'POST',
		headers: apiHeader,
		data: jsonResource,
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',
		success: function(data,status){
			$("#log").empty();
			$("#log").append(
					"<pre><br/>" 
					+ JSON.stringify(data, null, 4) 
					+ "</pre>"
					);
			if (callback)
			{
				callback(data);
			}
		},
		error: function(data,status){
		    alert(" Error Status: " + status);
		    $("#log").empty();
		    $("#log").append("Error running request");
		}
	});
};

// makes a PUT request to any endpoint in the API. taks as its
// arguments the path of the endpoint, a resource to be sent in
// the post body, and an optional resource id, which, when set, 
// is passed along in the url of the request
function putHttpResource(path, resource, resource_id) {
	resource_id = resource_id || '';

	if (resource_id)
	{
		resource_id = "/" + resource_id;
	}

	var hostname = $("#Hostname").val();
	var api_url = hostname + "/api";
	var api_key = $("#ApiKey").val();

	var jsonResource = JSON.stringify(resource);
	var apiHeader = { "Api-Key": api_key };
	
	$.support.cors = true;
	
	$.ajax({
		url: encodeURI(api_url + path + resource_id),
		type: 'PUT',
		headers: apiHeader,
		data: jsonResource,
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',
		success: function(data,status){
			getHttpResource(path, resource_id)
		},
		error: function(data,status){
		    alert(" Error Status: " + status);
		    $("#log").empty();
		    $("#log").append("Error running request");
		}
	});
};

// this function downloads a single page of data from the download
// endpoint and then calls itself recursively. it takes as its argument 
// a selection object and a page index. it continues, incrementing the 
// requested page number, till the POST request returns a set of data 
// with less than five entries
function getPagedData(resource, page) {
	var hostname = $("#Hostname").val();
	var api_url = hostname + "/api";
	var api_key = $("#ApiKey").val();

	var jsonResource = JSON.stringify(resource);
	var apiHeader = { "Api-Key": api_key };
	
	$.support.cors = true;
	
	$.ajax({
		url: encodeURI(
			api_url 
			+ '/download?includeMetadata=true&page='
			+ page
			+ '+&pagesize=5'),
		type: 'POST',
		headers: apiHeader,
		data: jsonResource,
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',
		success: function(data,status){
			if (page == 0){
				$("#log").empty();
			}

			$("#log").append(
					"<pre><b>Page: " 
					+ (page + 1)
					+ ", size: "
					+ data.length
					+ "</b><br/>" 
					+ JSON.stringify(data, null, 4) 
					+ "</pre>"
					);

			if (data.length == 5){
				getPagedData(resource, page+1)
			}
		},
		error: function(data,status){
		    alert(" Error Status: " + status);
		    $("#log").empty();
		    $("#log").append("Error running request");
		}
	});
};

// post log-in credentials to get back user data
// sets api field to the one returned
function postLogin(path, resource) {
	var hostname = $("#Hostname").val();
	var api_url = hostname + "/api";
	var api_key = $("#ApiKey").val();

	var jsonResource = JSON.stringify(resource);
	var apiHeader = { "Api-Key": api_key };
	
	$.support.cors = true;
	
	$.ajax({
		url: encodeURI(api_url + path),
		type: 'POST',
		headers: apiHeader,
		data: jsonResource,
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',

		// on success, post data to the log and update
		// the api key input field as well for future
		// API requests
		success: function(data,status){
			$("#log").empty();
			$("#log").append(
				"<pre><br/>" 
				+ JSON.stringify(data, null, 4) 
				+ "</pre>"
			);
			$("#ApiKey").val(data["ApiKey"]);
		},
		error: function(data,status){
		    alert(" Error Status: " + status);
		    $("#log").empty();
		    $("#log").append("Error running request");
		}
	});
};

// makes a simple get request to any of our API endpoints
// takes an optional id as input, which is appended to the end
// of the request url appropriately
function getHttpResource(path, resource_id) {
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
			$("#log").empty();
			$("#log").append("<pre><br/>" + JSON.stringify(data, null, 4) + "</pre>");
		},
		error: function(data, textStatus, errorThrown){
		    $("#log").empty();
		    $("#log").append("Error running request");
		    $("#log").append("<br />Resource id: " + resource_id);
		    $("#log").append("<br />Response headers: " + request.getAllResponseHeaders());
		}
	});
};

// takes the username and password provided by the user and attempts
// to log in using the users endpoint, setting the current api key
// and returning the user object information
function login() {
    $("#log").empty();
    $("#log").append("Loading...");
    var user_data = {
    	UserName: $("#user_name").val(),
    	Password: $("#user_pass").val()
    }
    postLogin('/users', user_data);
    $("#user_pass").val("")
};

// gets a selection based on the selection id provided by the user
function getSelection() {
    select_id = $("#selection_id").val();
    $("#log").empty();
    $("#log").append("Fetching selection: " + select_id + "...");
    getHttpResource('/savedselections', select_id);
};

// creates a selection based on the sample_selection object near the
// beginning of this document
function createSelection() {
    var selection_object = sample_selection;
    var dt = new Date();
    selection_object.Name ="Selection - (Updated: " + dt.getFullYear() + '/'
	+ (dt.getMonth() + 1) + "/"
	+ dt.getDate() + " at "
	+ dt.getHours() + ":" + dt.getMinutes() + ")";
    $("#log").empty();
    $("#log").append("Creating selection...");
    postHttpResource('/savedselections', selection_object, null, function(data)
    {
	    $("#selection_id").val(data['Id']);
    });
};

// takes the selection based on the id provided by the user and updates
// its name to reflect the time it was changed
function updateSelection() {
	$("#log").empty();
	$("#log").append("Loading...");
	var hostname = $("#Hostname").val();
	var select_id = $("#selection_id").val()
	var url = hostname + "/api/savedselections/" + select_id;
	var api_key = $("#ApiKey").val();

	var apiHeader = { "Api-Key": api_key };
	
	$.support.cors = true;

	// first, we get the original selection, so that all other values
	// remain unchanged	
	$.ajax({
		url: encodeURI(url),
		type: 'GET',
		headers: apiHeader,
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',

		// now that we have a full selection object, we update the name
		// value according to current time and make the PUT request
		// to the savedselections endpoint
		success: function(data, status){
		    var dt = new Date();
		    data.Name ="Selection - (Updated: " + dt.getFullYear() + '/'
			+ (dt.getMonth() + 1) + "/"
			+ dt.getDate() + " at "
			+ dt.getHours() + ":" + dt.getMinutes() + ")";
		    putHttpResource('/savedselections', data, data['Id']);
		},
		error: function(data,status){
		    alert(" Error Status: " + status);
		    $("#log").empty();
		    $("#log").append("Error running request");
		}
	});
};


// the server side of this method is a Webtask application, source code available
// at ../webtask/file-download.js
function getFromWebTasks(path, resource_id) {
	var payload = {
                "api_path": path,
		"api_resource_id": resource_id,
		"api_key": $("#ApiKey").val()
	}

	$.support.cors = true;
	
	var request = $.ajax({
		url: WEBTASK_URL,
		type: 'POST',
		data: JSON.stringify(payload),
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',
		success: function(data, status){
			$("#log").empty();
			$("#log").append("<pre><br/><a href=" + JSON.stringify(data, null, 4) + 
				">Click here to download your file.</a></pre>");
		},
		error: function(data, textStatus, errorThrown){
		    $("#log").empty();
		    $("#log").append("Error: " + errorThrown);
		}
	});
};


function fileDownload() {
    $("#log").empty();
    $("#log").append("Loading...");
    getFromWebTasks('/filedownload', $("#selection_id").val());
};

function getUser() {
    $("#log").empty();
    $("#log").append("Loading...");
    getHttpResource('/users/me');
};

function getDatabanks() {
    $("#log").empty();
    $("#log").append("Loading...");
    getHttpResource('/databank');
};

function getVariables() {
    $("#log").empty();
    $("#log").append("Loading...");
    getHttpResource('/variable/WDMacro');
};

function getRegions() {
    $("#log").empty();
    $("#log").append("Loading...");
    getHttpResource('/region/WDMacro');
};

function getData(selection) {
    $("#log").empty();
    $("#log").append("Loading...");
    postHttpResource('/download?includeMetadata=true', selection);
};

function downloadShaped() {
    $("#log").empty();
    var selection_id = $("#selection_id").val();
    $("#log").append("Running shaped download for selection id " + selection_id + "...");
    var shapeFormat = {
	    Pivot: "false",
	    StackedQuarters: "false",
	    Frequency: "Annual"
    }
    postHttpResource('/shapeddownload', shapeFormat, selection_id);
};

// this function takes the ReadyUrl from the queuedownload endpoint
// and uses it as a target URL for a GET request, which in turn 
// returns either a true or false value indicating whether the 
// download is ready
function checkQueue(url, ready_url) {
    var api_key = $("#ApiKey").val();
    var apiHeader = { "Api-Key": api_key };
    $.support.cors = true;
    
    $.ajax({
    	url: encodeURI(url),
    	type: 'GET',
    	headers: apiHeader,
    	contentType: 'application/json; charset=utf-8',
    	dataType: 'json',
    	success: function(data, status){
		$("#log").empty();
                $("#log").append("Fetching download link...");

		if (data == true)
		{
                    getFromWebTasks('/generatelink', ready_url.substring(GENERATE_LINK_INDEX));
		}
		else
		{
			$("#log").append(
				"<pre><br/>" 
				+ "Download not yet ready."
				+ "</pre>");
		}
	},
    	error: function(data,status){
    	    alert(" Error Status: " + status);
    	    $("#log").empty();
    	    $("#log").append("Error running request");
	}
    });

}

// queues a download using the selction with the id provided by the user
// and generates a button, which allows for polling to see whether the
// download is ready
function queueDownload() {
    $("#log").empty();
    $("#log").append("Loading...");

    var resource_id = $("#selection_id").val();
    if (resource_id)
    {
    	selection_id = "/" + resource_id;
    }
    
    var hostname = $("#Hostname").val();
    var api_url = hostname + "/api";
    var api_key = $("#ApiKey").val();
    var apiHeader = { "Api-Key": api_key };
    $.support.cors = true;
    
    $.ajax({
    	url: encodeURI(api_url + '/queuedownload' + selection_id),
    	type: 'GET',
    	headers: apiHeader,
    	contentType: 'application/json; charset=utf-8',
    	dataType: 'json',

	// the data returned from this request includes a ReadyUrl for
	// checking to see whether or not the download is ready
    	success: function(data, status){
    		$("#log").empty();
    		$("#log").append("Success");
		$("#btnCheckQueue").remove();

		// set up a button for checking the new queued download
		var newButton = $('<button />',
		{
			text: 'Check queue',
			id: 'btnCheckQueue',
			click: function()
			{ 
				checkQueue(data['ReadyUrl'], data['Url']); 
			}
		});	

		$("#btnQueueDownload").after(newButton);

    	},
    	error: function(data,status){
    	    alert(" Error Status: " + status);
    	    $("#log").empty();
    	    $("#log").append("Error running request");
    	}
    });
};



// checks to makes sure api key isn't blank. creates
// alert to notify the user if it is. returns true/false
// based on presence/absence
function checkKey()
{
	var return_status = true;
	if (!$("#ApiKey").val())
	{
		return_status = false;
		alert("Error: please enter an API key");
	}
	return return_status;
}


// checks to makes sure selection id isn't blank. creates
// alert to notify the user if it is. returns true/false
// based on presence/absence
function checkSelection()
{
	return_status = true;
	if (!$("#selection_id").val())
	{
		return_status = false;
		alert("Error: please enter a selection id");
	}
	if (!checkKey())
	{
		return_status = false;
	}
	return return_status;
}


// add event handlers to the various buttons on our page
$(document).ready(function(){
    $.support.cors = true;

    $("#btnDownloadFile").click(function() {
	    if (checkSelection())
	    {
		    fileDownload();
	    }
    });
    $("#btnQueueDownload").click(function() {
	    if (checkSelection())
	    {
		    queueDownload();
	    }
    });
    $("#btnDownloadShaped").click(function() {
	    if (checkSelection())
	    {
		    downloadShaped();
	    }
    });
    $("#btnCreateSelection").click(function() {
	    if (checkKey())
	    {
		    createSelection();
	    }
    });
    $("#btnUpdateSelection").click(function() {
	    if (checkSelection())
	    {
		    updateSelection();
	    }
    });
    $("#btnGetSelection").click(function() {
	    if (checkSelection())
	    {
		    getSelection();
	    }
    });
    $("#btnLogin").click(function() {
    	login();
    });
    $("#btnGetUser").click(function() {
	    if (checkKey())
	    {
		getUser();
	    }
    });
    $("#btnGetDatabanks").click(function() {
	    if (checkKey())
	    {
		getDatabanks();
	    }
    });
    $("#btnGetVariables").click(function() {
	    if (checkKey())
	    {
		getVariables();
	    }
    });
    $("#btnGetRegions").click(function() {
	    if (checkKey())
	    {
		getRegions();
	    }
    });
    $("#btnGetPaged").click(function() {
	    if (checkKey())
	    {
		getPagedData(sample_selection, 0);
	    }
    });
    $("#btnPostA").click(function() {
	    if (checkKey())
	    {
		getData(sample_selection);
	    }
    });
    $("#btnPostCustom").click(function() {
	    if (checkKey())
	    {
		var selectionCustom = $.extend({}, sample_selection);
		selectionCustom.Regions = [{ DatabankCode: 'WDMacro', 
			RegionCode: $("#Location").val()}];
		selectionCustom.Variables = [{ ProductTypeCode: 'WMC', 
			VariableCode: $("#Indicator").val(), MeasureCodes: ['L'] }];
		getData(selectionCustom);
	    }
    });
});
