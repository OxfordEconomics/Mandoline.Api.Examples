// <copyright file="api_examples.js" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>


// makes a simple get request to any of our API endpoints
// takes an optional id as input, which is appended to the end
// of the request url appropriately
function deleteHttpResource(path, selection_id, callback) {
	resource_id = selection_id || "";

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
		type: 'DELETE',
		headers: apiHeader,
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',
		success: function(data, status){
			$("#log").empty();
			callback(selection_id);
		},
		error: function(data, textStatus, errorThrown){
		    $("#log").empty();
		    $("#log").append("Error running request");
		    $("#log").append("<br />Resource id: " + resource_id);
		    $("#log").append("<br />Response headers: " + request.getAllResponseHeaders());
		}
	});
};

// post log-in credentials to get back user data
// sets api field to the one returned
function postLogin(path, resource, selection_callback) {
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
			$("#ApiKey").val(data["ApiKey"]);
			selection_callback(data['SavedSelections']);
		},
		error: function(data,status){
			alert(" Error Status: " + status);
			$("#log").empty();
		}
	});
};


// takes a selection id and attempts to make a DELETE request to
// the api and then removing from the table on success
function deleteSelection(selection_id)
{
	deleteHttpResource('/savedselections', selection_id, function(selection_id)
	{
		$("#" + selection_id).remove();
	});
}


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
    postLogin('/users', user_data, function(selections_list){
	$("#selections_div").empty();
	var new_table = $('<table />', 
	{
		id: "selections_table",
		style: "margin-top: 15px;"
	});
	$("#selections_div").append(new_table);

	var table = $("#selections_table");
	for (i = 0; i < selections_list.length; i++)
	    {
		    var new_row = $('<tr/>',
		    {
			    id: selections_list[i]['Id']
		    });

		    var new_col = $('<td />');
		    new_col.append(selections_list[i]['Name']);
		    new_row.append(new_col);
		    
		    var new_col = $('<td />',
		    {
			    padding: '0px 0px 0px 15px'
		    }); 
		    new_col.append(selections_list[i]['Id']);
		    new_row.append(new_col);

		    var new_col = $('<td />',
		    {
			    padding: '0px 0px 0px 15px'
		    }); 
		    var new_button = $('<button />');
		    new_button.click(function(selection_id)
			    {
				    return function()
				    {
					    $("#" + selection_id).css("background-color", "red");
					    deleteSelection(selection_id);
				    }
		    }(selections_list[i]['Id']));
		    new_button.append("Delete");
		    new_col.append(new_button);
		    new_row.append(new_col);

		    table.append(new_row);
	    }
	
	$("#selections_div").css("display", "block");
    });
    $("#user_pass").val("")
};

// add event handlers to the various buttons on our page
$(document).ready(function(){
    $.support.cors = true;

    $("#btnLogin").click(function() {
    	login();
    });
});
