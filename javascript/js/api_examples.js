// <copyright file="api_examples.js" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

const SELECTION_ID = '2c140fbb-4624-4004-927e-621734f3cb93'

// sample selections
var selectionA =
{
    Name: 'API - test',
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
    Regions: [{ DatabankCode: 'WDMacro', RegionCode: 'GBR' }, { DatabankCode: 'WDMacro', RegionCode: 'USA' }],
    Variables: [{ ProductTypeCode: 'WMC', VariableCode: 'GDP$', MeasureCodes: ['L', 'PY', 'DY'] }, { ProductTypeCode: 'WMC', VariableCode: 'CPI', MeasureCodes: ['L'] }]
};

var selectionQ =
{
    Name: 'API - test',
    DatabankCode: 'WDMacro',
    MeasureCode: 'L',
    StartYear: 2015,
    EndYear: 2021,
    StackedQuarters: 'false',
    Frequency: 'Quarterly',
    Sequence: 'EarliestToLatest',
    Precision: 1,
    Order: 'IndicatorLocation',
    GroupingMode: 'false',
    Regions: [{ DatabankCode: 'WDMacro', RegionCode: 'GBR' }],
    Variables: [{ ProductTypeCode: 'WMC', VariableCode: 'GDP$', MeasureCodes: ['L', 'D', 'DY', 'P', 'PY'] }]
};

// make post request to Mandoline API
function postHttpResource(path, resource, resource_id="") {
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

// make post request to Mandoline API
function putHttpResource(path, resource, resource_id="") {
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

// get download broken up into pages
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

// make get request to Mandoline API
function getHttpResource(path, resource_id="") {
	if (resource_id)
	{
		resource_id = "/" + resource_id;
	}

	var hostname = $("#Hostname").val();
	var api_url = hostname + "/api";
	var api_key = $("#ApiKey").val();

	var apiHeader = { "Api-Key": api_key };
	
	$.support.cors = true;
	
	$.ajax({
		url: encodeURI(api_url + path + resource_id),
		type: 'GET',
		headers: apiHeader,
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',
		success: function(data, status){
			$("#log").empty();
			$("#log").append("<pre><br/>" + JSON.stringify(data, null, 4) + "</pre>");
		},
		error: function(data,status){
		    alert(" Error Status: " + status);
		    $("#log").empty();
		    $("#log").append("Error running request");
		}
	});
};

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

function getSelection() {
    select_id = $("#selection_id").val();
    $("#log").empty();
    $("#log").append("Fetching selection: " + select_id + "...");
    getHttpResource('/savedselections', select_id);
};

function createSelection() {
    var selection_object = selectionA;
    var dt = new Date();
    selection_object.Name ="Selection - (Updated: " + dt.getFullYear() + '/'
	+ (dt.getMonth() + 1) + "/"
	+ dt.getDate() + " at "
	+ dt.getHours() + ":" + dt.getMinutes() + ")";
    $("#log").empty();
    $("#log").append("Creating selection...");
    postHttpResource('/savedselections', selection_object);
};

function updateSelection() {
	$("#log").empty();
	$("#log").append("Loading...");
	var hostname = $("#Hostname").val();
	var select_id = $("#selection_id").val()
	var url = hostname + "/api/savedselections/" + select_id;
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
	    StackedQuarters: "false"
    }
    postHttpResource('/shapeddownload', shapeFormat, selection_id);
};

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
    	success: function(data, status){
    		$("#log").empty();
    		$("#log").append("Success");

		$("#btnCheckQueue").remove();

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
    $("#selection_id").val(SELECTION_ID);
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
        getPagedData(selectionA, 0);
    });
    $("#btnPostA").click(function() {
        getData(selectionA);
    });
    // $("#btnPostQ").click(function() {
    //     getData(selectionQ);
    // });
    // $("#btnPostBoth").click(function() {
    //     var selectionBoth = $.extend({}, selectionQ);
    //     selectionBoth.Frequency = 'Both';
    //     getData(selectionBoth);
    // });
    $("#btnPostCustom").click(function() {
		var selectionCustom = $.extend({}, selectionA);
		selectionCustom.Regions = [{ DatabankCode: 'WDMacro', 
			RegionCode: $("#Location").val()}];
		selectionCustom.Variables = [{ ProductTypeCode: 'WMC', 
			VariableCode: $("#Indicator").val(), MeasureCodes: ['L'] }];
        getData(selectionCustom);
    });
});
