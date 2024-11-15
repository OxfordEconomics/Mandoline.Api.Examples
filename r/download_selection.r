# import modules
require('httr')
require('jsonlite')

# program constants
API_KEY <- '<< your api key >>'
SELECTION_ID <- '<< selection id >>'
BASE <- 'https://services.oxfordeconomics.com/api'
PAGE_SIZE <- 20000

# Main function: takes a saved selection id, downloads and processes the raw data
download_selection_as_dataframe <- function(selection_id) {
  raw_data <- download_raw_data(selection_id)
  df <- flatten_dataframe(raw_data)
  return(df)
}

# Function for downloading raw data
download_raw_data <- function(selection_id) {
  endpoint <- '/download/'
  options1 <- '?includemetadata=true&page='
  options2 <- '&pagesize='
  raw_data <- data.frame ()
  page <- 0
  repeat {

    print(paste('Downloading page', page + 1))
    
    http_request <- paste(BASE, endpoint, selection_id, options1, page, options2, PAGE_SIZE, sep = '')
    http_response <- GET(http_request, add_headers('api-key' = API_KEY))
    
    if (status_code(http_response) > 400) {
      print('Error - couldn\'t download selection')
      print(paste('Status code: ', status_code(http_response)))
      print(paste('Url: ', http_request))
      break
    }

    new_data_string <- content(http_response, 'text', flatten=TRUE)
    new_data <- fromJSON(new_data_string, simplifyDataFrame = FALSE)    
    raw_data <- append(raw_data, list(new_data))

    # check to see whether there are more pages
    if ( nchar(new_data_string) < 4 ) {
      break
    }
    
    page <- page + 1
  }  

  raw_data <- unlist(raw_data, recursive = FALSE)

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

substitute_na_for_null <- function(data) {
  for (name in names(data)) {
    if (is.null(data[[name]])) {
      data[[name]] <- NA
    }
  }
  return(data)
}

# Function to process the response and convert to a dataframe
flatten_dataframe <- function(raw_data) {
  # Preallocate memory for the vector
  row_list <- list()
  row_index <- 1
  years <- NULL

  for (i in seq_along(raw_data)) {
    # pull the necessary values from raw data
    # create a unique id based on the variable mnemonic and sector
    id <- paste(raw_data[[i]]$VariableCode, raw_data[[i]]$LocationCode, raw_data[[i]]$MeasureCode, sep='_')
    indicator <- raw_data[[i]]$Metadata$IndicatorName
    location <- raw_data[[i]]$Metadata$Location
    annual_data <- substitute_na_for_null(raw_data[[i]]$AnnualData)
    quarterly_data <-  substitute_na_for_null(raw_data[[i]]$QuarterlyData)
    
    # Extract metadata fields
    metadata <- raw_data[[i]]$Metadata

    # Helper function to handle NULL values
    handle_null <- function(x) {
      if (is.null(x)) {
        return("")
      } else {
        return(x)
      }
    }

    # Apply the helper function to each metadata field
    metadata_fields <- c(
      handle_null(metadata$Description), handle_null(metadata$DatabankName), handle_null(metadata$ScaleFactor), 
      handle_null(metadata$AuthorEmail), handle_null(metadata$Author), handle_null(metadata$AuthorTelephone), 
      handle_null(metadata$HistoricalEndYear), handle_null(metadata$HistoricalEndQuarter), handle_null(metadata$ImposedEndYear), 
      handle_null(metadata$ImposedEndQuarter), handle_null(metadata$BaseYearPrice), handle_null(metadata$LastUpdate), 
      handle_null(metadata$SeasonallyAdjusted), handle_null(metadata$SectorCoverage), handle_null(metadata$BaseYearIndex), 
      handle_null(metadata$SourceDetails), handle_null(metadata$Units), handle_null(metadata$Source), 
      handle_null(metadata$AdditionalSourceDetails), handle_null(metadata$MeasureName), handle_null(metadata$AnnualTypeCode), 
      handle_null(metadata$PartnerName), handle_null(metadata$ScenarioName), handle_null(metadata$CommodityName), 
      handle_null(metadata$MarketSectorName), handle_null(metadata$IncomeBandName), handle_null(metadata$HasQuarterly), 
      handle_null(metadata$CategoryDescription)
    )

    print(paste('Adding ', id, ' at index ', i, '...', sep = ''))

    # add quarterly rows
    if (length(quarterly_data) > 0)
    {
      quarterly_rows <- get_quarterly_rows(id, indicator, location, quarterly_data)
      for (i in c(2:length(quarterly_rows)))
      {
        quarterly_row_with_metadata <- c(unlist(quarterly_rows[[i]]), metadata_fields)
        row_list[[length(row_list)+1]] <- quarterly_row_with_metadata
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
      
      annual_row <- c(id, indicator, location, "A", as.numeric(unlist(annual_data)), metadata_fields)
      row_list[[length(row_list)+1]] <- annual_row
    }
  }

  # Define column names
  dim_names <- c('id', 'indicator', 'location', 'period', years, 'Description', 'DatabankName', 'ScaleFactor', 
                 'AuthorEmail', 'Author', 'AuthorTelephone', 'HistoricalEndYear', 'HistoricalEndQuarter', 'ImposedEndYear', 
                 'ImposedEndQuarter', 'BaseYearPrice', 'LastUpdate', 'SeasonallyAdjusted', 'SectorCoverage', 'BaseYearIndex', 
                 'SourceDetails', 'Units', 'Source', 'AdditionalSourceDetails', 'MeasureName', 'AnnualTypeCode', 'PartnerName', 
                 'ScenarioName', 'CommodityName', 'MarketSectorName', 'IncomeBandName', 'HasQuarterly', 'CategoryDescription') 

  # Convert row_list to a dataframe
  df <- as.data.frame(t(matrix(unlist(row_list), nrow=length(unlist(row_list[1])))))
  colnames(df) <- dim_names

  return(df)
}

# Example usage
# Your existing code to download and save the dataframe to CSV
df <- download_selection_as_dataframe(SELECTION_ID)

# Save the dataframe to a file
write.csv(df, "output.csv", row.names = FALSE)

print("Dataframe has been written to output.csv")

