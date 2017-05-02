# <copyright file="api_example.py" company="Oxford Economics">
# Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
# Licensed under the MIT License. See LICENSE file in the
# project root for full license information.
# </copyright>

import sys
import os
sys.path.append(os.getcwd())
from api_client import Client, StagingClient
import getpass
import datetime
from requests import exceptions
import time
import json

# a sample selection id for running downloads and
# get, delete, and update example functions
SELECTION_ID = ''
API_KEY = ''

# a sample selection dictionary for running downloads 
# and get, delete, and update example functions
sample_selection = {
    # these fields are generated automatically.
    # it is not necessary to supply these
    # "ContactId": "jamesmills@oxfordeconomics.com",
    # "LastUpdate": "2017-03-16T15:01:01.557",
    # "Id": "2c140fbb-4624-4004-927e-621734f3cb93",
    "SelectionType": "QuerySelection",
    "LegacyDatafeedFileStructure": 'false',
    "IsDatafeed": 'false',
    "Format": 0,

    # these fields are not generated automatically
    # generated for the user
    "DatabankCode": "WDMacro",
    "StartYear": 2015,
    "EndYear": 2021,
    "Frequency": "Annual",
    "GroupingMode": 'false',
    "IndicatorSortOrder": "AlphabeticalOrder",

    # setting this to true will make this accessible
    # either by id via the API or among the user's
    # saved selections
    "IsTemporarySelection": 'false',

    # options are Hidden, Private, Company, Public
    # and Shared
    "ListingType": "Private",

    # options are AlphabeticalOrder and TreeOrder
    "LocationSortOrder": "AlphabeticalOrder",

    # -- available measure codes --
    #   L:  level values
    #   PY: percentage year/year
    #   DY: difference year to year
    #   P: percentage quarter/quarter
    #   D: difference quarter to quarter 
    #   GR: annualized
    "MeasureCode": "L",
    "Name": "Sample selection - To be deleted",

    # sort indicator first. other option is
    # LocationIndicator
    "Order": "IndicatorLocation", 
    "Precision": 1,
    "Regions": [
         {
             # a full list of region and databank codes
             # can be generated using the /region and
             # /databank endpoints
             "DatabankCode": "WDMacro",
             "RegionCode": "DEU"
         },
         {
             "DatabankCode": "WDMacro",
             "RegionCode": "FRA"
         },
         {
             "DatabankCode": "WDMacro",
             "RegionCode": "GBR"
         },
         {
             "DatabankCode": "WDMacro",
             "RegionCode": "USA"
         }
    ],
    "Sequence": "EarliestToLatest",
    "StackedQuarters": 'false',
    "Variables": [
        {
            # -- available measure codes --
            #   L:  level values
            #   PY: percentage year/year
            #   DY: difference year to year
            "MeasureCodes": ["L"],
            "ProductTypeCode": "WMC",

             # a full list of region and databank codes
             # can be generated using the /region and
             # /databank endpoints
            "VariableCode": "CPI"
        },
        {
            "MeasureCodes": ["L","PY","DY"],
            "ProductTypeCode": "WMC",
            "VariableCode": "GDP$"
        }
    ]
}


# demos the Databank and Region endpoints
# polls the /databank endpoint for the list of databanks
# and then uses each id to check the region endpoint
# for the count of available locations; prints in list form
# (takes an optional databank id to get the count for just one)
def databank_test(client, db_id=''):
    print('\n\n--Demo: get databanks / regions--')

    # returns the databanks list
    databank_list = client.get_databanks(db_id)

    try:
        # for each databank, get the full list of regions and count
        for db in databank_list:
            print('\t{}'.format(db['Name']), end=' - ')
            regions = client.get_databank_regions(db['DatabankCode'])
            print(len(regions['Regions']))

    # this exception handles the case that an id is provided and only one
    # databank is returned from get_databanks()
    except TypeError:
            print('\t{}'.format(databank_list['Name']), end=' - ')
            regions = client.get_databank_regions(databank_list['DatabankCode'])
            print(len(regions['Regions']))

    return databank_list



# demos the GET functionality of the Selection endpoint 
# gets the selection with id = s_id and prints it to screen
# in indented json form
def get_selection_test(client, s_id):
    print('\n\n--Demo: get selection--')

    selection = client.get_selection(s_id)
    print(json.dumps(selection, indent=3, sort_keys=True))

    return selection


# demos the POST functionality of the User endpoint 
# prompts for user name and password and then prints
# a couple of user details and sets the current client
# api key to the returned one
def login_test(client):
    print('\n\n--Demo: user login--')

    user = input('User name: ')
    passwd = getpass.getpass('Password: ')
    
    user = client.user_login(user, passwd)
    print('  {0} {1} - Saved selections:'.format(user['FirstName'], user['LastName']))
    for selection in user['SavedSelections']:
        print('\t{0} - Id:{1}'.format(selection['Name'], selection['Id']))

    client.api_key = user['ApiKey']


# demos the POST functionality of the User endpoint 
# gets information based on the current client API token
# prints the list of selections available
def get_user_test(client):
    print('\n\n--Demo: get user--')

    user = client.get_user()
    print(json.dumps(user, indent=3, sort_keys=True))

    return user


# demos the PUT functionality of the Selection endpoint 
# takes the selection id, changes the name, and posts the
# changes. it then gets the newly changed selection and
# prints to screen as evidence
def update_selection_test(client, s_id):
    print('\n\n--Demo: put selection--')

    selection = client.get_selection(s_id)
    print('\tOld selection: {0}'.format(selection['Name']))

    # update the name to show when it was updated
    selection['Name'] = ('SampleSelection - (Updated: ' + 
        '{:%Y/%m/%d %H:%M}'.format(datetime.datetime.now()) + ')')
    selection = client.update_selection(selection)
    selection = client.get_selection(s_id)
    print('\tUpdated selection: {0}'.format(selection['Name']))

    return selection



# demos the POST functionality of the Download endpoint 
# downloads the selection with given id and prints to 
# screen in indented json form
def download_test(client, s_id):
    print('\n\n--Demo: download Selection--')

    # download by selection
    download_data = client.get_download(s_id)
    print('\nFirst entry in response:')
    print(json.dumps(download_data[0], indent=3, sort_keys=True))

    return download_data


# demos the POST functionality of the Selection endpoint, creating a new selection
# creates a new selection based on the sample_selection provided in
# this document. prints the returned selection to screen in indented json form
def create_selection_test(client):
    print('\n\n--Demo: create selection--')
    sample_selection['Name'] = ('SampleSelection - (Created: ' + 
            '{:%Y/%m/%d %H:%M}'.format(datetime.datetime.now()) + ')')
    new_selection = client.create_selection(sample_selection)
    print(json.dumps(new_selection, indent=3, sort_keys=True))

    return new_selection


# demos the GET functionality of the Variable endpoint
# gets the available indicators for the databank used in our sample selection
# prints to screen as a list
def get_variables_test(client):
    print('\n\n--Demo: get variables: {}--'.format(sample_selection['DatabankCode']))
    var_list = client.get_databank_variables(sample_selection['DatabankCode'])
    for var in var_list['Variables']:
        print('\t{}'.format(var['VariableName']))

    return var_list


# demos the GET functionality of the Region endpoint
# gets the available locations for the databank used in our sample selection
# prints to screen as a list
def get_regions_test(client):
    print('\n\n--Demo: get regions: {}--'.format(sample_selection['DatabankCode']))
    reg_list = client.get_databank_regions(sample_selection['DatabankCode'])
    for reg in reg_list['Regions']:
        print('\t{}'.format(reg['Name']))

    return reg_list


# demos the DELETE functionality of the Selection endpoint, 
# creates a new selection, then deletes it. it then attempts
# to get that same selection, which should result in an HTTP error
# as the document no longer exists
def delete_selection_test(client):
    print('\n\n--Demo: delete selection--')
    sample_selection['Name'] = ('SampleSelection - (Created: ' + 
            '{:%Y/%m/%d %H:%M}'.format(datetime.datetime.now()) + ')')
    new_selection = client.create_selection(sample_selection)
    s_id = new_selection['Id']
    print('\tSelection created with id: {}'.format(s_id))

    print('\tDeleting selection with id: {}...'.format(s_id))
    delete_response = client.delete_selection(s_id)

    print('\tAttempting to get selection with id: {}...'.format(s_id))
    try:
        initial_selection = client.get_selection(s_id)
        print('\tSelection still exists with id:{}'.format(initial_selection['Id']))
        return False

    except exceptions.HTTPError: 
        print('\tSelection deleted successfully')
        return True

    
# demos the queue download endpoint
# posts a selection for download to the queue download endpoint
# and then polls the resulting ready check URL till timeout
# occurs. timeout is an integer multiple of 5 seconds, defaulting
# to python's maximum integer value
def queue_download_test(client, selection, timeout=sys.maxsize):
    print('\n\n--Demo: queue download--')
    q_download = client.queue_download(selection)
    print(json.dumps(q_download, indent=3, sort_keys=True))

    time_elapsed = 0
    success = client.check_queue(q_download['ReadyUrl'])
    while success == False and time_elapsed < timeout:
        time.sleep(5)
        print('Checking to see whether download is ready...')
        success = client.check_queue(q_download['ReadyUrl'])
        time_elapsed += 1

    if time_elapsed < timeout:
        print('Download ready at: {}'.format(q_download['Url']))

    return time_elapsed



# demos the file download endpoint
# posts given selection to the download file endpoint and prints
# the resulting CSV string to screen
def file_download_test(client, selection):
    print('\n\n--Demo: file download--')
    file_download = client.download_file(selection, "csv")
    print(file_download['data'])

    return file_download


# demos the shaped download endpoint
# passes a shape configuration and selection id to the shaped
# download endpoint and prints a selected number of the resulting
# object in indented json format
def shaped_download_test(client, s_id):
    print('\n\n--Demo: shaped download--')

    # the shape configuration dictinoary, determining whether to
    # pivot, stack quarters, and the frequency (i.e. either or both
    # annual or quarterly data)
    config = {
            'pivot': 'true',
            'StackedQuarters': 'true',
            'Frequency': 'Annual'
            }

    shaped_download = client.get_shaped_download(s_id, config)
    print('Results, first three rows')
    print(json.dumps(shaped_download[:3], indent=3, sort_keys=True))

    return shaped_download


# demos the tree endpoint for locations
# makes a get request to the tree endpoint using the macro databank
# code and then prints a fixed number of lines of the result in
# indented json form
def location_tree_test(client):
    print('\n\n--Demo: location tree--')
    location_tree = client.get_location_tree('WDMacro')
    for node in location_tree:
        for child_node in node['Children']:
            print('\t{0} - direct nodes: {1}'.format(child_node['Name'], 
                len(child_node['Children']) if 'Children' in child_node else 'NA'))

    return location_tree


# demos the tree endpoint for indicators
# makes a get request to the tree endpoint using the macro databank
# code and then prints a fixed number of lines of the result in
# indented json form
def indicator_tree_test(client):
    print('\n\n--Demo: indicator tree--')
    indicator_tree = client.get_indicators_tree('WDMacro')
    for node in indicator_tree:
        print('  {}'.format(node['Name']))
        for child_node in node['Children']:
            print('\t{0} - direct nodes: {1}'.format(child_node['Name'], 
                len(child_node['Children']) if 'Children' in child_node else 'NA'))

    return indicator_tree


# takes a list of possible commands in the form of a dictionary with
# fields for (at least) name and prints each one
def print_command_list(command_options):
    print("\n--Options--")
    for i in range(len(command_options)):
        print("{0}. {1}".format(i, command_options[i]['name'])) 


if __name__ == '__main__':
    # baseline functinoality demo
    client = Client()
    API_KEY = client.api_key

    # initialize new selection for purposes of this demo
    print('Creating new selection...')
    new_selection = client.create_selection(sample_selection)
    SELECTION_ID = new_selection['Id']
    print('New selection created with id: {}'.format(SELECTION_ID))

    command_options = [
            { "name": "Quit" },
            { "name": "Log in",
              "function": login_test,
              "arguments": [client] },
            { "name": "Download selection",
              "function": download_test,
              "arguments": [client, SELECTION_ID] },
            { "name": "Create selection",
              "function": create_selection_test,
              "arguments": [client] },
            { "name": "Update selection",
              "function": update_selection_test,
              "arguments": [client, SELECTION_ID] },
            { "name": "Databanks list",
              "function": databank_test,
              "arguments": [client, sample_selection['DatabankCode']] },
            { "name": "Get selection",
              "function": get_selection_test,
              "arguments": [client, SELECTION_ID] },
            { "name": "Get user",
              "function": get_user_test,
              "arguments": [client] },
            { "name": "Delete selection",
              "function": delete_selection_test,
              "arguments": [client] },
            { "name": "Variables list (Macro databank)",
              "function": get_variables_test,
              "arguments": [client] },
            { "name": "Regions list (Macro databank)",
              "function": get_regions_test,
              "arguments": [client] },
            { "name": "Queue download selection",
              "function": queue_download_test,
              "arguments": [client, sample_selection] },
            { "name": "Download selection to file",
              "function": file_download_test,
              "arguments": [client, sample_selection] },
            { "name": "Download selection to shaped",
              "function": shaped_download_test,
              "arguments": [client, SELECTION_ID] },
            { "name": "Indicator tree (Macro databank)",
              "function": indicator_tree_test,
              "arguments": [client] },
            { "name": "Location tree (Macro databank)",
              "function": location_tree_test,
              "arguments": [client] }
            ]

    while True:
        print_command_list(command_options)

        choice = input('Command to execute: ')

        if choice == '0':
            print('Removing selection with id: {}'.format(SELECTION_ID))
            client.delete_selection(SELECTION_ID)
            break
        else:
            try:
                choice = int(choice)
                if choice < len(command_options) and choice > 0:
                    command = command_options[int(choice)]
                    command['function']( *command['arguments'] )
                else:
                    print("Invalid choice. Please try again.")
            except Exception as err:
                print("Invalid choice. Please try again.")
                print("Error: ", err)

