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

SELECTION_ID = '2c140fbb-4624-4004-927e-621734f3cb93'
API_KEY = '566dbac8-d0c2-4248-a0ed-ca3a8ce4df5c'
BAD_URL_BASE = 'https://services.oxfordeconomics.com/api-error'

# deprecated sample selection
# sampleSelect = {
#                 'DatabankCode': 'WDMacro',
#                 'MeasureCode': 'L',
#                 'StartYear': 2015,
#                 'EndYear': 2021,
#                 'StackedQuarters': 'false',
#                 'Frequency': 'Annual',
#                 'Sequence': 'EarliestToLatest',
#                 'Precision': 1,
#                 'TransposeColumns': 'false',
#                 'IsTemporarySelection': 'False',
#                 'Order': 'IndicatorLocation',
#                 'GroupingMode': 'false',
#                 'Regions': [{'DatabankCode': 'WDMacro',
#                              'RegionCode': 'GBR'},
#                             {'DatabankCode': 'WDMacro',
#                              'RegionCode': 'USA'}],
#                 'Variables': [{'ProductTypeCode': 'WMC',
#                                'VariableCode': 'GDP$',
#                                'MeasureCodes': ['L','PY','DY']},
#                               {'ProductTypeCode': 'WMC',
#                                'VariableCode': 'CPI',
#                                'MeasureCodes': ['L']}]}


# sample selection for post body
sampleSelect = {
    "ContactId": "jamesmills@oxfordeconomics.com",
    "DatabankCode": "WDMacro",
    "EndYear": 2021,
    "Format": 0,
    "Frequency": "Annual",
    "GroupingMode": 'false',
    "Id": "2c140fbb-4624-4004-927e-621734f3cb93",
    "IndicatorSortOrder": "AlphabeticalOrder",
    "IsDatafeed": 'false',
    "IsTemporarySelection": 'false',
    "LastUpdate": "2017-03-16T15:01:01.557",
    "LegacyDatafeedFileStructure": 'false',
    "ListingType": "Private",
    "LocationSortOrder": "AlphabeticalOrder",
    "MeasureCode": "L",
    "Name": "Selection - Updated: 3/16/2017 11:00:50 AM",
    "Order": "IndicatorLocation",
    "Precision": 1,
    "Regions": [
         {
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
    "SelectionType": "QuerySelection",
    "Sequence": "EarliestToLatest",
    "StackedQuarters": 'false',
    "StartYear": 2015,
    "Variables": [
        {
            "MeasureCodes": [
                "L"
            ],
            "ProductTypeCode": "WMC",
            "VariableCode": "CPI"
        },
        {
            "MeasureCodes": [
                "L",
                "PY",
                "DY"
            ],
            "ProductTypeCode": "WMC",
            "VariableCode": "GDP$"
        }
    ]
}


# demos the Databank and Region endpoints
def databank_test(client, db_id=''):
    print('\n\n--Demo: get databanks / regions--')
    databank_list = client.get_databanks(db_id)

    try:
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



# demos the GET functionality of the Selection endpoint 
def get_selection_test(client, s_id):
    print('\n\n--Demo: get selection--')

    selection = client.get_selection(s_id)
    print(json.dumps(selection, indent=3, sort_keys=True))


# demos the POST functionality of the User endpoint 
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
def get_user_test(client):
    print('\n\n--Demo: get user--')

    user = client.get_user()
    print('  {0} {1} - Saved selections:'.format(user['FirstName'], user['LastName']))
    for selection in user['SavedSelections']:
        print('\t{0} - Id:{1}'.format(selection['Name'], selection['Id']))


# demos the PUT functionality of the Selection endpoint 
def update_selection_test(client, s_id):
    print('\n\n--Demo: put selection--')

    selection = client.get_selection(s_id)
    print('\tOld selection: {0}'.format(selection['Name']))

    selection['Name'] = ('SampleSelection - (Updated: ' + 
        '{:%Y/%m/%d %H:%M}'.format(datetime.datetime.now()) + ')')
    selection = client.update_selection(selection)
    selection = client.get_selection(s_id)
    print('\tUpdated selection: {0}'.format(selection['Name']))



# demos the POST functionality of the Download endpoint 
def download_test(client, s_id):
    print('\n\n--Demo: download Selection--')

    # download by id
    #r = client.get_download(s_id)

    # download by selection
    r = client.get_download(client.get_selection(s_id))
    print('\nFirst entry in response:')
    print(json.dumps(r[0], indent=3, sort_keys=True))


# demos the POST functionality of the Selection endpoint, creating a new selection
def create_selection_test(client):
    print('\n\n--Demo: create selection--')
    sampleSelect['Name'] = ('SampleSelection - (Created: ' + 
            '{:%Y/%m/%d %H:%M}'.format(datetime.datetime.now()) + ')')
    r = client.create_selection(sampleSelect)
    print(json.dumps(r, indent=3, sort_keys=True))


# demos the GET functionality of the Variable endpoint
def get_variables_test(client):
    print('\n\n--Demo: get variables: {}--'.format(sampleSelect['DatabankCode']))
    var_list = client.get_databank_variables(sampleSelect['DatabankCode'])
    for var in var_list['Variables']:
        print('\t{}'.format(var['VariableName']))


# demos the GET functionality of the Region endpoint
def get_regions_test(client):
    print('\n\n--Demo: get regions: {}--'.format(sampleSelect['DatabankCode']))
    reg_list = client.get_databank_regions(sampleSelect['DatabankCode'])
    for reg in reg_list['Regions']:
        print('\t{}'.format(reg['Name']))


# demos the DELETE functionality of the Selection endpoint, creating a new selection
# and then deleting it
def delete_selection_test(client):
    print('\n\n--Demo: delete selection--')
    sampleSelect['Name'] = ('SampleSelection - (Created: ' + 
            '{:%Y/%m/%d %H:%M}'.format(datetime.datetime.now()) + ')')
    r = client.create_selection(sampleSelect)
    s_id = r['Id']
    print('\tSelection created with id: {}'.format(s_id))

    print('\tDeleting selection with id: {}...'.format(s_id))
    r = client.delete_selection(s_id)

    print('\tAttempting to get selection with id: {}...'.format(s_id))
    try:
        r = client.get_selection(s_id)
        print('\tSelection still exists with id:{}'.format(r['Id']))
    except exceptions.HTTPError: 
        print('\tSelection deleted successfully')


# demos the queue download endpoint
def queue_download_test(client, selection):
    print('\n\n--Demo: queue download--')
    q_download = client.queue_download([selection])
    print(json.dumps(q_download, indent=3, sort_keys=True))

    while client.check_queue(q_download['ReadyUrl']) == False:
        time.sleep(5)
        print('Checking to see whether download is ready...')

    print('Download ready at: {}'.format(q_download['Url']))



# demos the file download endpoint
def file_download_test(client, selection):
    print('\n\n--Demo: file download--')
    f_download = client.download_file([selection])
    print(f_download['data'])



# demos the queue download endpoint
def shaped_download_test(client, s_id):
    print('\n\n--Demo: shaped download--')
    config = {
            'pivot': 'true',
            'StackedQuarters': 'true',
            'ShowAnnual': 'true' 
            }
    s_download = client.get_shaped_download(s_id, config)
    print('Results, first three rows')
    print(json.dumps(s_download[:3], indent=3, sort_keys=True))


# demos the tree endpoint for locations
def location_tree_test(client):
    print('\n\n--Demo: location tree--')
    l_tree = client.get_location_tree('WDMacro')
    for q in l_tree:
        for c in q['Children']:
            print('\t{0} - direct nodes: {1}'.format(c['Name'], len(c['Children']) if 'Children' in c else 'NA'))


# demos the tree endpoint for indicators
def indicator_tree_test(client):
    print('\n\n--Demo: indicator tree--')
    i_tree = client.get_indicators_tree('WDMacro')
    for q in i_tree:
        print('  {}'.format(q['Name']))
        for child in q['Children']:
            print('\t{0} - direct nodes: {1}'.format(child['Name'], len(child['Children']) if 'Children' in child else 'NA'))


if __name__ == '__main__':
    # baseline functinoality demo
    client = StagingClient(API_KEY)

    # login_test(client)
    # create_selection_test(client)
    # download_test(client, SELECTION_ID)
    # update_selection_test(client, SELECTION_ID)
    # databank_test(client, sampleSelect['DatabankCode'])
    # get_selection_test(client, SELECTION_ID)
    get_user_test(client)
    # delete_selection_test(client)
    # get_variables_test(client)
    # get_regions_test(client)
    # queue_download_test(client, sampleSelect)
    # file_download_test(client, sampleSelect)
    # shaped_download_test(client, SELECTION_ID)
    # indicator_tree_test(client)
    # location_tree_test(client)
