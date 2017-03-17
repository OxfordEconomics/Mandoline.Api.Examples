project: ExampleMandolineAPI.sln 
date: mar 6, 2017
author: james mills
description: simple examples demonstrating use of
			 mandonline/databank API, covering all
			 public functions

--requirements--
	Visual Studio 2010 Tools for Office Runtime
	http://www.microsoft.com/en-us/download/details.aspx?id=39290


--contents--
	User.cs: user api calls including login
	Info.cs: api calls relating to meta data e.g. variables, available databanks
	SavedSelection.cs: api calls relating to making and updating saved selections
	DownloadShaped.cs: api calls relating to shaped download requests
	Download.cs: demonstrates each of the remaining download api calls


--additional important files--
	Settings.cs: where default API_TOKEN and sample selection are set
	Output.cs, Table.cs: relating to how downloaded data is processed on return