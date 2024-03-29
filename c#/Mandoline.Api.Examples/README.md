### Copyright 2017 OXFORD ECONOMICS LTD.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

# Mandoline.Api.Examples/C#

### Contents:
0. Configuration
1. Dependencies
2. Projects and files
3. Running the Examples

### 0. Configuration:

  - copy the AppSettings.config.example template -> AppSettings.config in both Client.Repl & Client.Gui
  - insert value for API_TOKEN, which is your API Key
  - insert value for SELECTION_ID that is either in the user's selection list or has accessibility set to public
  - insert value for BASE_URL, which should be https://services.oxfordeconomics.com

### 1. Dependencies:
Core: implementation of Mandoline API examples

- *Mandoline.Api.Client*: the .Net SDK for making calls to the Oxford Economics API, includes...
  - *ApiClient.cs*: the main interface for making API calls
  - *ServiceModels*: contains domain object definitions e.g. DatabankDto, DataseriesDto
- *Newton.Json*, *System.Xml*: for easy de-/serializing Json, XML objects
- *System.Data*: for setting up DataTables corresponding to response data
- Note: DLL files have been included for Mandoline.Api.Client and can be found in the 'packages' directory

Client.Gui: windows form demonstration of Mandoline API calls

- *System.Windows.Forms*, *System.Drawing*, *etc.*: packages necessary to setting up Windows Forms client

Client.Repl: command line demonstration of Mandoline API calls

- *Sharprompt*: the command-line interfaced used by the client

### 2. Projects and files:

Core: implementation of Mandoline API examples

- *Domain/*: 
  - *Download.cs*: functions for paged download, download request, and queue check
  - *DownloadShaped.cs*: functions for dataseries downloads shaped into tables
  - *Info.cs*: functions for listing databanks available and their respective variables and regions
  - *SavedSelection.cs*: functions for getting, creating, and updating saved selections
  - *User.cs*: functions for getting user information and logging in
- *AppConstants.cs*: draws constants from AppSettings.config, namely the user API token and the example Selection Id
- *Output.cs*: describes the interface which must be implemented by client programs
- *Table.cs*: provides several generic DataTables for the various types of data

Client.Gui: Windows Form demonstration of Mandoline API calls

- *GridOutput*: implementation of the output interface for displaying example data in a Windows Form grid view
- *Program.cs, Form1.cs*: the mechanisms for loading the Windows Form and handling events
- *AppSettings.config*: user-supplied file based on AppSettings.config.example, which contains user values for api token and sample selection id

Client.Repl: command line demonstration of Mandoline API calls

- *Command.cs*: the various commands for running corresponding examples in the Repl command line
- *AppSettings.config*: user-supplied file based on AppSettings.config.example, which contains user values for api token and sample selection id

### 3. Running the Examples:
To get started running the examples, please ensure that all the steps under step 0 have been completed. Following this, Assuming the project
is loaded in Visual Studio, you can directly run either the Client.Repl or Cliet.Gui projects and will be presented with a list of CLI commands
or a graphical interface respectivley to execute API calls. 
