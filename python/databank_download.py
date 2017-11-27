# <copyright file="databank_download.py" company="Oxford Economics">
# Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
# Licensed under the MIT License. See LICENSE file in the 
# project root for full license information.
# </copyright> 

import json
import requests
import sys

API_KEY = # insert api key 
BASE_URL = 'https://services.oxfordeconomics.com'

sample_selection = {
    'DatabankCode': 'WDMacro',
    'Frequency': 'Annual',
    'GroupingMode': 'false',
    'IndicatorSortOrder': 'AlphabeticalOrder',
    'IsTemporarySelection': 'true',
    'ListingType': 'Private',
    'LocationSortOrder': 'AlphabeticalOrder',
    'Order': 'IndicatorLocation',
    'Precision': 1,
    'Sequence': 'EarliestToLatest',
    'StackedQuarters': 'false',
    'StartYear': 1980,
    'EndYear': 2045,

    # note: the fields below have been assigned empty lists
    'Regions': [
    ],
    'Variables': [
    ]
}

# for debug purposes only. you wouldn't want to limit the number of pages
# if you were attempting to download the entire databank
def _reached_page_limit(page, page_limit):
    return (page_limit != -1 and page >= page_limit)

def _download_url(base_url, page, page_size):
    url = base_url + '/api/download?includemetadata=true'
    return url + '&page={0}+&pagesize={1}'.format(page, page_size)

def databank_download(selection_dictionary, page_size=5000, page_limit=-1):
    headers = {'Accept': 'application/json',
        'Api-Key': API_KEY,
        'Content-Type': 'application/json; charset=utf8' }

    page = 0
    data_list = None

    while not _reached_page_limit(page, page_limit):
        print('Downloading page {0}... '.format(page + 1), end='')
        sys.stdout.flush()

        # note: _download_url returns a link of the form 
        # ../api/download?includemetadata=true&page={page}+&page_size={page_size}
        response = requests.post(
            _download_url(BASE_URL, page, page_size), 
            headers=headers, 
            data=json.dumps(selection_dictionary))

        new_data = response.json()
        page += 1

        if data_list is None:
            data_list = new_data
        else:
            data_list.extend(new_data)

        print('contains {0} series, {1} total'.format(len(new_data), len(data_list)))

        if len(new_data) != page_size:
            break

    return data_list

if __name__ == '__main__':
    data = databank_download(sample_selection)
    print('Finished downloading')
    print('Writing to data_file.out... ', end='')
    sys.stdout.flush()
    data_file = open('data_file.out', 'w')
    json.dump(data, data_file, indent=3)
    print('Done')
