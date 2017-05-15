### Copyright 2017 OXFORD ECONOMICS LTD.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

# Mandoline.Api.Examples/Python

### Contents:
0. Configuration
1. Dependencies
2. Scripts and files

### 0. Configuration:

  - copy AppSettings.config.example -> AppSettings.config
  - insert corresponding values for API_TOKEN
  - if using a base url other than the default, insert this as well for BASE_URL

### 1. Dependencies:
api_client.py: SDK for making Mandoline API calls

  - *Requests*: the Python HTTP request library
  - *Json*, for de-/serializing Json objects

### 2. Scripts and files:

api_client.py: describes the Client object with methods for making Mandoline API calls
  - constructor takes user API key as well as a base URL for making calls (with default to services.oxfordeconomics)
  - all methods return a json dictionary or list of json dictionaries of the requested data
  - all methods raise exceptions based on status e.g. for resource not found

api_example.py: 

  - all example calls commented out by default, can be found near the end of the file to reactivate
