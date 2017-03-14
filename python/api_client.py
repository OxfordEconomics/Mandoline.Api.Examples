# filename: api_client.py
# date: mar 13, 2017
# author: james mills
# description: simple examples for use with the
#              mandonline/databank API

import requests
import json
import datetime


DEFAULT_BASE = 'https://services.oxfordeconomics.com/api'

class Client(object):

    def __init__(self, api_key = "", base_url = DEFAULT_BASE):
        self.api_key = api_key
        self._base_url = base_url


    # returns: dictionary of headers for use with http requests
    def _header(self):
        headers = {'Accept':'application/json',
                   'API-Key': self.api_key,
                   'Content-Type': 'application/json; charset=utf8' }
        return headers


    # build api url using base url and path
    # takes: one or more strings and appends each to the base url
    # returns: url string
    def _url(self, *path):
        url = self._base_url
        for q in path:
            url += '/' + q
        return url 


    # get a full list of the databanks available as well as the number of countries for each
    # returns: list of json dictionaries of Databanks 
    def get_databanks(self):
        try:
            r = requests.get(self._url('databank'), headers=self._header())
            r.raise_for_status()

        except requests.exceptions.HTTPError as e:
            print(e)
            return {}

        except requests.exceptions.RequestException as e:
            print(e)
            return {}

        return r.json()


    # get a full list of a databank's available regions
    # takes: databank id
    # returns: json dictionary of Regions 
    def get_databank_regions(self, id):
        try:
            r = requests.get(self._url('region', id), headers=self._header())

        except requests.exceptions.HTTPError as e:
            print(e)
            return {}

        except requests.exceptions.RequestException as e:
            print(e)
            return {}

        return r.json()


    # takes: username, pass
    # returns: json dictionary of validated User 
    def user_login(self, username, passwd):
        payload = { 'Username': username,
                    'Password': passwd }
        try:
            r = requests.post(self._url('users'), headers=self._header(), data=json.dumps(payload) )

        except requests.exceptions.HTTPError as e:
            print(e)
            return {}

        except requests.exceptions.RequestException as e:
            print(e)
            return {}

        return r.json()


    # takes: selection id
    # returns: json dictionary of Selection 
    def get_selection(self, id):
        try:
            r = requests.get(self._url('savedselections', id), headers=self._header())

        except requests.exceptions.HTTPError as e:
            print(e)
            return {}

        except requests.exceptions.RequestException as e:
            print(e)
            return {}

        return r.json()


    # gets saved selection with given selection id (see above)
    # returns: json dictinoary of new Selection
    def create_selection(self, selection_object):
        try:
            r = requests.post(self._url('savedselections'), headers=self._headers(), 
                data=json.dumps(selection_object) )

        except requests.exceptions.HTTPError as e:
            print(e)
            return {}

        except requests.exceptions.RequestException as e:
            print(e)
            return {}

        return r.json()


    # updates a saved selection with given selection id (see above)
    # takes: selection object with id that corresponds to already existing non-temporary saved selection
    # returns: blank json dictionary
    def update_selection(self, selection_object):
        try:
            r = requests.put(self._url('savedselections', selection_object['Id']), 
                    headers=self._header(), data=json.dumps(selection_object) )

        except requests.exceptions.HTTPError as e:
            print(e)
            return {}

        except requests.exceptions.RequestException as e:
            print(e)
            return {}

        return {}


    # returns: well-formed download url based on page and pagesize 
    def _download_url(self, page, pagesize):
        return self._url('download?includeMetadata=true&page={0}+&pagesize={1}').format(page,pagesize)


    # returns: single page of downloaded data as json dictionary
    def _get_download(self, selection, page, pagesize=5):
        r = requests.post(self._download_url(page, pagesize), headers=self._header(), json=selection)
        r.raise_for_status() # in case of bad request: pass http exceptions up to calling func
        return r.json()

  
    # downloads multiple pages of data based on provided Selection
    # returns: json dictionary of Download data
    def get_download(self, selection, pagesize=5):
        page = 0

        try:
            new_data = self._get_download(selection, page, pagesize)
            data = new_data
            while len(new_data) == pagesize:
                page += 1
                new_data = self._get_download(selection, page, pagesize)
                data.append(new_data)

        except requests.exceptions.HTTPError as e:
            print(e)
            return {}

        except requests.exceptions.RequestException as e:
            print(e)
            return {}

        return data
