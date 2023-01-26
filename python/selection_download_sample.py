# 
# Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
# Licensed under the MIT License. See LICENSE file in the
# project root for full license information.
# 

import sys
import json
import requests 
import time
  
# WARNING: In production these values should be provided via environment or command line args
API_KEY = '<<ENTER API KEY HERE>>'
SELECTION = {'ID': '<<ENTER SELECTION ID HERE>>', 'NAME': '<<ENTER NAME OF SELECTION HERE>>'}

PAGE_SIZE = 20000   # set to number of records per page (20k is the max allowed by API)
MAX_PAGES = None    # set to None for as many pages as possible
CHECKPOINT = 5      # checkpoint (save) after this many pages

def databank_download(f, selection_id, show_progress=True):

    BASE_URL = 'https://services.oxfordeconomics.com'
    INCLUDE_METADATA='false' # or set = 'true'

    headers = { 'Accept': 'application/json',
                'Api-Key': API_KEY,
                'Content-Type': 'application/json; charset=utf8' }

    def _download_url(base_url, page, pagesize):
        return  f'{base_url}/api/download/{selection_id}?includemetadata={INCLUDE_METADATA}&page={page}&pagesize={page_size}'

    page = 0
    page_size = PAGE_SIZE
    max_pages = MAX_PAGES
    data = None

    api_request_time_total = 0.0
    disk_write_time_total = 0.0

    while True:

        # download selection_id with some escalating wait retry logic 
        max_retries = 5
        retries = 0
        while retries < max_retries:
            try:
                start = time.time()

                response = requests.get(_download_url(BASE_URL, page, page_size), headers=headers)
                
                end = time.time()
                api_request_time_total += (end - start)
                
                break
            except ConnectionError as msg:
                print(f'ConnectionError Exception!\n{msg}')
                retries += 1
                print(f'Waiting to retry ({retries}/5) in {retries*5} seconds...\n{msg}')
                time.sleep(retries*5)
                pass
            except TimeoutError as msg:
                print(f'ConnectionError Exception!\n{msg}')
                retries += 1
                print(f'Waiting to retry ({retries}/5) in {retries*5} seconds...\n{msg}')
                time.sleep(retries*5)
                pass
            except Exception as msg:
                print(f'Exception!\n{msg}')
                raise RuntimeError('Something bad happened!')

        if retries == max_retries:
            return page, api_request_time_total, disk_write_time_total

        page_data = response.json()
        page += 1

        if show_progress:
            sys.stdout.write('#')
            sys.stdout.flush()

        # accumulate data
        if data is None:
             data = page_data
        else:
             data.extend(page_data)

        # dump to file every so many pages
        if page % CHECKPOINT == 0 and data != None:
            sys.stdout.write('+')
            sys.stdout.flush()

            start = time.time()

            json.dump(data, f)

            end = time.time()
            disk_write_time_total += (end - start)

            data = None

        # get all pages or limit to max_pages
        if max_pages == None:
            pass
        elif page == max_pages:
            break

        # have we got to the end
        if len(page_data) < page_size:
            break

    # dump the last chunk of data, if any
    if not data is None:
        start = time.time()

        json.dump(data, f)

        end = time.time()
        disk_write_time_total += (end - start)

    return page, api_request_time_total, disk_write_time_total


if __name__ == '__main__':

    SELECTION_ID = SELECTION['ID']
    DATA_FILE = SELECTION['NAME'] + '.tmp'

    start = time.asctime(time.localtime(time.time()))
    print(f'Started @ {start} | DATA_FILE={DATA_FILE}, PAGE_SIZE={PAGE_SIZE}, MAX_PAGES={MAX_PAGES}, CHECKPOINT={CHECKPOINT}')

    with open(DATA_FILE, 'w') as f:
        pages, api_request_time_total, disk_write_time_total = databank_download(f, SELECTION_ID)

    end = time.asctime(time.localtime(time.time()))

    print(f'\nCOMPLETE! Downloaded {pages} pages')
    print(f'Started @ {start}')
    print(f'Ended @ {end}')
    print('Total API Request Time: %.1f secs, %.1f mins' % (api_request_time_total, api_request_time_total/60))
    print('Total Disk Write Time: %.1f secs, %.1f mins' % (disk_write_time_total, disk_write_time_total/60))

'''
# NOTE
# If you're using vscode to develop/debug this program and it encounters an SSL error, then do the following:

#     mklink "<YOUR ANACONDA LOCATION>\Anaconda3\DLLs\libcrypto-1_1-x64.dll" "<YOUR ANACONDA LOCATION>\Anaconda3\Library\bin\libcrypto-1_1-x64.dll"
#     mklink "<YOUR ANACONDA LOCATION>\Anaconda3\DLLs\libssl-1_1-x64.dll" "<YOUR ANACONDA LOCATION>\Anaconda3\Library\bin\libssl-1_1-x64.dll"

# (or you can physically copy them over)

# The exception you may see will be something like this:

#     HTTPSConnectionPool(host='services.oxfordeconomics.com', port=443):
#     Max retries exceeded with url: /api/download/...?includemetadata=false&page=0+&pagesize=2000
#     (Caused by SSLError("Can't connect to HTTPS URL because the SSL module is not available."))
'''
