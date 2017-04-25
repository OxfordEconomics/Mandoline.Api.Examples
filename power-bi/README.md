### Copyright 2017 OXFORD ECONOMICS LTD.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

# Mandoline.Api.Examples/Power-BI

### Contents:
0. Configuration
1. Dependencies
2. Scripts and files

### 0. Configuration:

For setting up web data connector on localhost:
  - download and install a lightweight http server. python comes bundled with the http.server module, which can be executed as a script. other options include NPM's http-server or Mongoose.
  - start your http server. in the case of python, open a command prompt and change your working directory to where the connector's index.html file is located and run 'python -m http.server 8000 --bind 127.0.0.1'. now the page should be accessible by directing a web browser to 127.0.0.1:8000.

### 2. Scripts and files:

index.html: the web data connector interface, takes input from the user and passes to api_connector.js

js/api_connector.js: 
  - authenticates the user, processes input and settings
  - defines function for building table schema
  - defines function for running http request to populate data tables
  - passes control to tableau

webtask/tableau-wdc.js: 
  - combines all of the above into a Webtask application for hosting your connector remotely
