$header = @"
# <copyright file="" company="Oxford Economics">
# Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
# Licensed under the MIT License. See LICENSE file in the project 
# root for full license information.
# </copyright> 


"@



$file_list = Get-ChildItem -Recurse -Include *.py

foreach ($file in $file_list)
{
	$content = Get-Content $file
	Write-Host "Modifying $file"
	Set-Content $file -value ($header + $content)
	
}
