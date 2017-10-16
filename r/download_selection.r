# <copyright file="download_selection.r" company="Oxford Economics">
# Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
# Licensed under the MIT License. See LICENSE file in the
# project root for full license information.
# </copyright>

# this R script shows one way a user might download a saved selection
# from the Oxford Economics Global Data Workstation and convert this
# raw output into a usable dataframe. it contains the following functions...
#
# download_selection_as_dataframe: the functions listed below are called
#								   to produce a usable dataframe
# download_raw_data: the http calls to our API
# get_quarterly_rows: an internal function for splitting quarterly
#				      data into multiple rows
# flatten_dataframe: converts the raw output of the API calls into
#				     a usable dataframe


# import modules
require('httr')
require('jsonlite')

# program constants
# note: API_KEY must be set below
#
BASE <- 'https://services.oxfordeconomics.com/api'
API_KEY <- ''
PAGE_SIZE <- 5
SELECTION_ID <- '/11688972-aff4-4f74-bb13-30b54fd7fab1'


# essentially the main function: takes a saved selection id, downloads
# and processes the raw data from the Oxford Economics API and returns
#
download_selection_as_dataframe <- function(selection_id)
{
	raw_data <- download_raw_data(selection_id)
	df <- flatten_dataframe(raw_data)
	return(df)
}

# this function makes the necessary HTTP calls for downloading
# and combining raw data
# returns: dataframe, where each row is a dataseries. note that
#	     annual and quarterly data are each contained in single
#	     columns at this point (e.g. instead of 2016, 2017,
#	     and 2018, the data column is AnnualData)
#
download_raw_data <- function(selection_id)
{
	# select resource endpoint, in this case /download
	endpoint <- '/download'
	options <- '?includemetadata=true&page='

	# set up and run the request
	raw_data <- data.frame()
	page <- 0
	repeat 
	{
		print(paste('Downloading page', page+1))
	
		http_request <- paste(BASE, endpoint, selection_id, options, page, sep='')

		http_response <- GET(http_request, add_headers('api-key' = API_KEY))

		if (status_code(http_response) > 400) {
			print('Error - couldn\'t download selection')
			print(paste('Status code: ', status_code(http_response)))
			print(paste('Url: ', http_request))
			break
		}

		new_data <- fromJSON(content(http_response, 'text', flatten=TRUE))
	
		raw_data <- c(raw_data, new_data)
	
		# check to see whether there are more pages
		if ( length(new_data) < PAGE_SIZE ) {
			break
		}
	
		page <- page + 1
	}

	print(paste('Downloaded', length(raw_data), 'rows of data'))

	return(raw_data)
}


# this takes a dataframe row with quarterly data and returns a list
# whose elements are [1] the years and [2-] quarterly data values
#
get_quarterly_rows <- function(id, indicator, location, quarterly_data)
{
	df_list <- list()
	q1 <- list(id, indicator, location, 'Q1')
	q2 <- list(id, indicator, location, 'Q2')
	q3 <- list(id, indicator, location, 'Q3')
	q4 <- list(id, indicator, location, 'Q4')
	years <- list()
	
	previous_year <- -1
	for (yq_index in c(1:length(quarterly_data)))
	{
		col_label <- names(quarterly_data)[yq_index]
		year <- substr(col_label, 1, 4)
		period <- substr(col_label, 6, 6)
		val <- quarterly_data[[col_label]]
		
		# add each new year to the list of years
		if (year != previous_year)
		{
			years[[length(years)+1]] <- year
		}
		
		# append the new data value to the appropriate list
		if (period == '1')
		{
			q1[[length(q1)+1]] <- val
		}
		else if (period == '2')
		{
			q2[[length(q2)+1]] <- val
		}
		else if (period == '3')
		{
			q3[[length(q3)+1]] <- val
		}
		else if (period == '4')
		{
			q4[[length(q4)+1]] <- val
		}
		
		previous_year <- year
	}

	return(list(years, q1, q2, q3, q4))
}


# takes a dataframe with, among others, AnnualData and QuarterlyData columns
# and flattens them such that each year has a column and excess metadata is
# removed from the frame
#
flatten_dataframe <- function(raw_data)
{
	# convert raw data into usable dataframes
	row_list <- list()
	years <- list()
	for (i in c(1:length(raw_data$DatabankCode))) 
	{
		# pull the necessary values from raw data
		# create a unique id based on the variable mnemonic and sector
		id <- paste(raw_data$VariableCode[i], raw_data$LocationCode[i], raw_data$MeasureCode[i], sep='_')
		indicator <- raw_data$Metadata$IndicatorName[i]
		location <- raw_data$Metadata$Location[i]
		annual_data <- raw_data$AnnualData[i,]
		quarterly_data <- raw_data$QuarterlyData[i,]
	
		print(paste('Adding ', id, '...', sep=''))

		# add quarterly rows
		if (length(quarterly_data) > 0)
		{
			quarterly_rows <- get_quarterly_rows(id, indicator, location, quarterly_data)
			for (i in c(2:length(quarterly_rows)))
			{
				row_list[[length(row_list)+1]] <- quarterly_rows[i]
			}
			if (length(years) == 0)
			{
				years <- quarterly_rows[[1]]
			}
		}

		# add annual row
		if (length(annual_data) > 0)
		{
			if (length(years) == 0)
			{
				years <- names(annual_data)
			}

			row_list[[length(row_list)+1]] <- append(list(id, indicator, location, 'A'), annual_data)
		}
	}

	# cast list to dataframe
	dim_names <- append(list('id', 'variable', 'location', 'period'), years)
	df <- as.data.frame(t(matrix(unlist(row_list), nrow=length(unlist(row_list[1])))))
	names(df) <- dim_names

	return(df)
}


# generate a dataframe from the saved selection id listed in the constants
#
df <- download_selection_as_dataframe(SELECTION_ID)
print(df)

