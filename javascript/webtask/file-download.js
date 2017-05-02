// <copyright file="file-download.js" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

var request = require('request@2.67.0')

API_URL = 'https://services.oxfordeconomics.com/api/';

// this function takes a saved selection id and makes a GET
// request to the filedownload endpoint, forwarding the download
// url, which appears as th redirect location in the response headers
// from the api
//
// note: request headers and body are accessible via the context
// object e.g. context.data.*, context.headers.* and the response
// data is simply handed off to the callback function
module.exports = function (context, callback) 
{
    var api_url = API_URL + context.data.api_path;
    var api_key = context.data.api_key;
    var api_resource_id = context.data.api_resource_id || "";

    if (!api_resource_id || !api_key || !api_url)
    {
            callback(400, null);
    }

    api_resource_id = "/" + api_resource_id;
      
    var options = 
    {
      method: 'GET',
      followRedirect: false,
      uri: api_url + api_resource_id,
      headers: 
      {
        'Api-Key': api_key
      }
    };
  	
    request(options, function(error, response, body)
    {
      if (response.statusCode >= 300 && response.statusCode < 400)
      {
        console.log(response.headers.location);
        callback(null, response.headers.location);
      }
      else
      {
        console.log(error);
        console.log(response);
        console.log(body);
        callback(response.statusCode, error);
      }
    });
}
