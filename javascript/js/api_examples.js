// <copyright file="api_examples.js" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>


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
function postHttpResource(path, resource) {
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
					+ "<pre><br/>" 
					+ JSON.stringify(data, null, 4) 
					+ "</pre>"
					);
		},
		error: function(data,status){
		    alert(" Error Status: " + status);
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
		}
	});
};

// make get request to Mandoline API
function getHttpResource(path) {
	var hostname = $("#Hostname").val();
	var api_url = hostname + "/api";
	var api_key = $("#ApiKey").val();

	var apiHeader = { "Api-Key": api_key };
	
	$.support.cors = true;
	
	$.ajax({
		url: encodeURI(api_url + path),
		type: 'GET',
		headers: apiHeader,
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',
		success: function(data,status){
			$("#log").empty();
			$("#log").append("<pre><br/>" + JSON.stringify(data, null, 4) + "</pre>");
		},
		error: function(data,status){
		    alert(" Error Status: " + status);
		}
	});
};


function login() {
    var user_data = {
    	UserName: $("#user_name").val(),
    	Password: $("#user_pass").val()
    }
    postLogin('/users', user_data);
};

function getUser() {
    getHttpResource('/users/me');
};

function getDatabanks() {
    getHttpResource('/databank');
};

function getVariables() {
    getHttpResource('/variable/WDMacro');
};

function getRegions() {
    getHttpResource('/region/WDMacro');
};

function getData(selection) {
    postHttpResource('/download?includeMetadata=true', selection);
};


// event handlers for the various buttons on our page
$(document).ready(function(){
    $("#btnLogin").click(function() {
    	login()
    });
    $("#btnGetUser").click(function() {
    	getUser()
    });
    $("#btnGetDatabanks").click(function() {
    	getDatabanks()
    });
    $("#btnGetVariables").click(function() {
    	getVariables()
    });
    $("#btnGetRegions").click(function() {
    	getRegions()
    });
    $("#btnGetPaged").click(function() {
        getPagedData(selectionA, 0)
    });
    $("#btnPostA").click(function() {
        getData(selectionA)
    });
    $("#btnPostQ").click(function() {
        getData(selectionQ)
    });
    $("#btnPostBoth").click(function() {
		var selectionBoth = $.extend({}, selectionQ);
		selectionBoth.Frequency = 'Both';
        getData(selectionBoth)
    });
    $("#btnPostCustom").click(function() {
		var selectionCustom = $.extend({}, selectionA);
		selectionCustom.Regions = [{ DatabankCode: 'WDMacro', 
			RegionCode: $("#Location").val()}];
		selectionCustom.Variables = [{ ProductTypeCode: 'WMC', 
			VariableCode: $("#Indicator").val(), MeasureCodes: ['L'] }];
        getData(selectionCustom)
    });
});
