// <copyright file="api_examples.js" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>


// used as the default selection id
const SELECTION_ID = '2c140fbb-4624-4004-927e-621734f3cb93'

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
	Variables: [{ ProductTypeCode: 'WMC', VariableCode: 'GDP$', MeasureCodes: ['L', 'PY', 'DY'] }, 
		{ ProductTypeCode: 'WMC', VariableCode: 'CPI', MeasureCodes: ['L'] }]
};


// makes a POST request to any endpoint in the API. taks as its
// arguments the path of the endpoint, a resource to be sent in
// the post body, and an optional resource id, which, when set, 
// is passed along in the url of the request
function postHttpResource(path, resource, resource_id) {
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
    postHttpResource('/savedselections', selection_object);
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

function fileDownload() {
    $("#log").empty();
    $("#log").append("Loading...");
    getHttpResource('/filedownload', $("#selection_id").val());
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
function checkQueue(check_url, ready_url) {
    var api_key = $("#ApiKey").val();
    var apiHeader = { "Api-Key": api_key };
    $.support.cors = true;
    
    $.ajax({
    	url: encodeURI(check_url),
    	type: 'GET',
    	headers: apiHeader,
    	contentType: 'application/json; charset=utf-8',
    	dataType: 'json',
    	success: function(data, status){
		$("#log").empty();
		if (data == true)
		{
			$("#log").append(
				"<pre><br/>" 
				+ "Download ready at: <a href=\"" 
				+ ready_url
				+ "\">Click here to download</a>"
				+ "</pre>");
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


// add event handlers to the various buttons on our page
$(document).ready(function(){
    $.support.cors = true;

    $("#selection_id").val(SELECTION_ID);

    $("#btnDownloadFile").click(function() {
	    fileDownload();
    });
    $("#btnQueueDownload").click(function() {
	    queueDownload();
    });
    $("#btnDownloadShaped").click(function() {
	    downloadShaped();
    });
    $("#btnCreateSelection").click(function() {
	    createSelection();
    });
    $("#btnUpdateSelection").click(function() {
	    updateSelection();
    });
    $("#btnGetSelection").click(function() {
	    getSelection();
    });
    $("#btnLogin").click(function() {
    	login();
    });
    $("#btnGetUser").click(function() {
    	getUser();
    });
    $("#btnGetDatabanks").click(function() {
    	getDatabanks();
    });
    $("#btnGetVariables").click(function() {
    	getVariables();
    });
    $("#btnGetRegions").click(function() {
    	getRegions();
    });
    $("#btnGetPaged").click(function() {
        getPagedData(sample_selection, 0);
    });
    $("#btnPostA").click(function() {
        getData(sample_selection);
    });
    $("#btnPostCustom").click(function() {
	var selectionCustom = $.extend({}, sample_selection);
	selectionCustom.Regions = [{ DatabankCode: 'WDMacro', 
		RegionCode: $("#Location").val()}];
	selectionCustom.Variables = [{ ProductTypeCode: 'WMC', 
		VariableCode: $("#Indicator").val(), MeasureCodes: ['L'] }];
        getData(selectionCustom);
    });
});
