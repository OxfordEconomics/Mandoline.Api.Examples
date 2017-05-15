# <copyright file="databank_download.py" company="Oxford Economics">
# Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
# Licensed under the MIT License. See LICENSE file in the 
# project root for full license information.
# </copyright> 

import json
import requests

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

def databank_download(selection_dictionary):
    headers = {'Accept': 'application/json',
        'Api-Key': API_KEY,
        'Content-Type': 'application/json; charset=utf8' }

    def _download_url(base_url, page, pagesize):
        url = base_url + '/api/download?includemetadata=true'
        return url + '&page={1}+&pagesize={2}'.format(base_url, page, page_size)

    page = 0
    page_size = 5000
    data = None

    while True:
        # note: _download_url returns a link of the form 
        # ../api/download?includemetadata=true&page={page}+&page_size={page_size}
        response = requests.post(_download_url(BASE_URL, page, page_size), 
            headers=headers, 
            data=json.dumps(selection_dictionary))

        new_data = response.json()
        page += 1

        if data is None:
            data = new_data
        else:
            data = data.append(new_data)

        if len(new_data) != page_size:
            break

    return data

if __name__ == '__main__':
    data = databank_download(sample_selection)
    data_file = open('data_file.tmp', 'w')
    json.dump(data, data_file)
