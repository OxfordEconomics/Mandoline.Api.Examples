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


    def _header(self):
        headers = {'Accept':'application/json',
                   'API-Key': self.api_key,
                   'Content-Type': 'application/json; charset=utf8' }
        return headers


    # build api url using base url and path
    # takes one or more strings and appends each to the base url
    def _url(self, *path):
        url = self._base_url
        for q in path:
            url += '/' + q
        return url 


    # get a full list of the databanks available as well as the number of countries for each
    # returns: http response body, where json is a list of Databank objects 
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
    # returns: http response body, where json is a Region object
    def get_databank_regions(self, id):
        return requests.get(self._url('region', id), headers=self._header())


    # takes username, pass
    # returns: if successful, http response body, where json is a User object
    #          representing validated user
    def user_login(self, username, passwd):
        payload = { 'Username': username,
                    'Password': passwd }
        return requests.post(self._url('users'), headers=self._header(), data=json.dumps(payload) )


    # takes: selection id
    # returns: http response body, where json is a Selection object
    def get_selection(self, id):
        return requests.get(self._url('savedselections', id), headers=self._header())


    # gets saved selection with given selection id (see above)
    # returns: http response body, where json is the new Selection object
    def create_selection(self, selection_object):
        return requests.post(self._url('savedselections'), headers=self._headers(), 
                data=json.dumps(selection_object) )


    # updates a saved selection with given selection id (see above)
    # takes: selection object with id that corresponds to already existing non-temporary saved selection
    # returns: http response body, where json is the updated Selection object
    def update_selection(self, selection_object):
        return requests.put(self._url('savedselections', selection_object['Id']), headers=self._header(), 
                data=json.dumps(selection_object) )


    # returns well-formed download url based on page and pagesize 
    def _download_url(self, page, pagesize):
        return self._url('download?includeMetadata=true&page={0}+&pagesize={1}').format(page,pagesize)


    # download a selection from the macro databank
    def _get_download(self, selection, page, pagesize=5):
        return requests.post(self._download_url(page, pagesize), headers=self._header(), json=selection)

   
    # download a selection from the macro databank, broken into separate http requests with number 
    # of entries in each download equal to pagesize
    # returns: json dictionary 
    def get_download(self, selection, pagesize=5):
        page = 0
        response = self._get_download(selection, page, pagesize)
        new_data = response.json()
        data = new_data
        while len(new_data) == pagesize:
            page += 1
            response = self._get_download(selection, page, pagesize)
            new_data = response.json()
            data.append(new_data)

        return data 
