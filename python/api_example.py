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


if __name__ == '__main__':
    client = Client(API_KEY)

    download_test(client, SELECTION_ID)
    update_selection_test(client, SELECTION_ID)
    databank_test(client)
    get_selection_test(client, SELECTION_ID)
    login_test(client)
