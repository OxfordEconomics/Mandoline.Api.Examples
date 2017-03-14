# filename: api_example.py
# date: mar 13, 2017
# author: james mills
# description: simple examples for use with the
#              mandonline/databank API

import sys
import os
sys.path.append(os.getcwd())
from api_client import Client
import getpass
import datetime

SELECTION_ID = 'd581f360-d0fa-40f3-a41e-0735db10f8c6'
API_KEY = '566dbac8-d0c2-4248-a0ed-ca3a8ce4df5c'
BAD_URL_BASE = 'https://services.oxfordeconomics.com/api-error'

# sample selection for post body
sampleSelect = {
                'Name': 'SampleSelection',
                'DatabankCode': 'WDMacro',
                'MeasureCode': 'L',
                'StartYear': 2015,
                'EndYear': 2021,
                'StackedQuarters': 'false',
                'Frequency': 'Annual',
                'Sequence': 'EarliestToLatest',
                'SortedColumnName': '',
                'SortedColumnOrder': '',
                'Precision': 1,
                'LastUpdate': '',
                'TransposeColumns': 'false',
                'IndicatorSortOrder': '',
                'LocationSortOrder': '',
                'Databank': '',
                'DownloadUrl': '',
                'IsTemporarySelection': 'False',
                'Order': 'IndicatorLocation',
                'GroupingMode': 'false',
                'Regions': [{'DatabankCode': 'WDMacro',
                             'RegionCode': 'GBR'},
                            {'DatabankCode': 'WDMacro',
                             'RegionCode': 'USA'},
                            {'DatabankCode': 'WDMacro',
                             'RegionCode': 'DEU'},
                            {'DatabankCode': 'WDMacro',
                             'RegionCode': 'ESP'},
                            {'DatabankCode': 'WDMacro',
                             'RegionCode': 'FRA'}],
                'Variables': [{'ProductTypeCode': 'WMC',
                               'VariableCode': 'GDP$',
                               'MeasureCodes': ['L','PY','DY']},
                              {'ProductTypeCode': 'WMC',
                               'VariableCode': 'CPI',
                               'MeasureCodes': ['L']}]}


# demos the Databank and Region endpoints
def databank_test(client):
    print('\n\n--Demo: get databanks / regions--')

    databank_list = client.get_databanks()

    for db in databank_list:
        print('\t{}'.format(db['Name']), end=' - ')

        regions = client.get_databank_regions(db['DatabankCode'])

        print(len(regions['Regions']))


# demos the GET functionality of the Selection endpoint 
def get_selection_test(client, id):
    print('\n\n--Demo: get selection--')

    selection = client.get_selection(id)
    print('\t{0} - {1}'.format(selection['Name'], selection['Id']))
    print(selection)


# demos the POST functionality of the User endpoint 
def login_test(client):
    print('\n\n--Demo: user login--')

    user = input('User name: ')
    passwd = getpass.getpass('Password: ')
    
    user = client.user_login(user, passwd)
    print('  {0} {1} - Saved selections:'.format(user['FirstName'], user['LastName']))
    for selection in user['SavedSelections']:
        print('\t{0} - Id:{1}'.format(selection['Name'], selection['Id']))


# demos the POST functionality of the User endpoint 
def get_user_test(client):
    print('\n\n--Demo: get user--')

    user = client.get_user()
    print('  {0} {1} - Saved selections:'.format(user['FirstName'], user['LastName']))
    for selection in user['SavedSelections']:
        print('\t{0} - Id:{1}'.format(selection['Name'], selection['Id']))


# demos the PUT functionality of the Selection endpoint 
def update_selection_test(client, id):
    print('\n\n--Demo: put selection--')

    selection = client.get_selection(id)
    print('\tOld selection: {0}'.format(selection['Name']))

    selection['Name'] = ('SampleSelection - (Updated: ' + 
        '{:%Y/%m/%d %H:%M}'.format(datetime.datetime.now()) + ')')
    selection = client.update_selection(selection)
    selection = client.get_selection(id)
    print('\tUpdated selection: {0}'.format(selection['Name']))



# demos the POST functionality of the Download endpoint 
def download_test(client, id):
    r = client.get_download(client.get_selection(SELECTION_ID))
    print(len(r))


# demos the POST functionality of the Selection endpoint, creating a new selection
def create_selection_test(client):
    print('\n\n--Demo: create selection--')
    sampleSelect['Name'] = ('SampleSelection - (Created: ' + 
            '{:%Y/%m/%d %H:%M}'.format(datetime.datetime.now()) + ')')
    r = client.create_selection(sampleSelect)
    print('\t{}'.format(r['Name']), end=' : ')
    print(r['Id'])


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
    id = r['Id']
    print('\tSelection created with id: {}'.format(id))

    print('\tDeleting selection with id: {}...'.format(id))
    r = client.delete_selection(id)

    print('\tAttempting to get selection with id: {}...'.format(id))
    r = client.get_selection(id)
    try:
        print('Selection still exists with id:{}'.format(r['Id']))
    except KeyError: 
        print('\tSelection deleted successfully')



if __name__ == '__main__':
    # baseline functinoality demo
    client = Client(API_KEY)

    #create_selection_test(client)
    #download_test(client, SELECTION_ID)
    #update_selection_test(client, SELECTION_ID)
    #databank_test(client)
    #get_selection_test(client, SELECTION_ID)
    #login_test(client)
    #get_user_test(client)
    #delete_selection_test(client)
    #get_variables_test(client)
    get_regions_test(client)


    # error handling demo
    #client = Client(API_KEY, BAD_URL_BASE)
    #databank_test(client)
