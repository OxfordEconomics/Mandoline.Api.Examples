# <copyright file="api_client.py" company="Oxford Economics">
# Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
# Licensed under the MIT License. See LICENSE file in the 
# project root for full license information.
# </copyright> 


import requests
import json
import datetime
import xml.etree.ElementTree


STAGING_BASE = 'http://oeweu3w-pwb-001.cloudapp.net/api'
DEFAULT_BASE = 'https://services.oxfordeconomics.com/api'
CONFIG_FILENAME = 'AppSettings.config'

class Client(object):

    def __init__(self, api_key = "", base_url = DEFAULT_BASE):
        self.api_key = api_key
        self._base_url = base_url
        if api_key == '':
            self.import_constants()


    # imports app constants using local configuration file at CONFIG_FILENAME
    def import_constants(self):
       root_element = xml.etree.ElementTree.parse(CONFIG_FILENAME).getroot() 
       for element in root_element.findall('add'):
            if (element.get('key') == 'API_TOKEN'):
                self.api_key = element.get('value')
            elif (element.get('key') == 'BASE_URL'):
                xml_base_url = element.get('value')
                if xml_base_url != '':
                    self._base_url = element.get('value') + '/api'


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
            if q != "":
                url += '/' + q
        return url 


    # takes: (optional) Databank id
    # return: json dictionary of available databanks or the one that matches the
    #         given id
    def get_databanks(self, db_id=""):
        r = requests.get(self._url('databank', db_id), headers=self._header())
        r.raise_for_status()
        return r.json()


    # get a full list of a databank's available regions
    # takes: databank id
    # returns: json dictionary of Regions 
    def get_databank_regions(self, db_id):
        r = requests.get(self._url('region', db_id), headers=self._header())
        r.raise_for_status()
        return r.json()


    # get a full list of a databank's available variables
    # takes: databank id
    # returns: json dictionary of Variables
    def get_databank_variables(self, db_id):
        r = requests.get(self._url('variable', db_id), headers=self._header())
        r.raise_for_status()
        return r.json()


    # takes: username, pass
    # returns: json dictionary of validated User 
    def user_login(self, username, passwd):
        payload = { 'Username': username,
                    'Password': passwd }
        r = requests.post(self._url('users'), headers=self._header(), data=json.dumps(payload) )
        r.raise_for_status()
        return r.json()


    # takes: (optional) user id 
    # returns: json dictionary of User with given id. if id is left blank,
    #          then the User data corresponding to the current active
    #          api_token
    def get_user(self, u_id="me"):
        r = requests.get(self._url('users', u_id), headers=self._header() )
        r.raise_for_status()
        return r.json()


    # takes: selection id
    # returns: json dictionary of Selection 
    def get_selection(self, s_id):
        r = requests.get(self._url('savedselections', s_id), headers=self._header())
        r.raise_for_status()
        return r.json()


    # takes: selection id
    def delete_selection(self, s_id):
        r = requests.delete(self._url('savedselections', s_id), headers=self._header())
        r.raise_for_status()


    # gets saved selection with given selection id (see above)
    # returns: json dictinoary of new Selection
    def create_selection(self, selection_object):
        r = requests.post(self._url('savedselections'), headers=self._headers(), 
            data=json.dumps(selection_object) )
        r.raise_for_status()
        return r.json()


    # updates a saved selection with given selection id (see above)
    # takes: selection object with id that corresponds to already existing non-temporary saved selection
    def update_selection(self, selection_object):
        r = requests.put(self._url('savedselections', selection_object['Id']), 
                headers=self._header(), data=json.dumps(selection_object) )
        r.raise_for_status()



    # takes: page index and page size (i.e. number of download elements per page)
    #        and a selection, either in the form of a dictioanry or id string
    # returns: well-formed download url based on page and pagesize 
    def _download_url(self, page, pagesize, s_id='?'):
        if s_id != '?':
            s_id = '/' + s_id + '?'
        return self._url('download{0}includeMetadata=true&page={1}+&pagesize={2}').format(s_id, 
                page, pagesize)


    # takes: page index and page size (i.e. number of download elements per page)
    #        and a selection, either in the form of a dictioanry or id string
    # returns: single page of downloaded data as json dictionary
    def _get_download(self, selection, page, pagesize=5):
        if isinstance(selection, str):
            r = requests.get(self._download_url(page, pagesize, selection), 
                    headers=self._header(), json=selection)
            r.raise_for_status() # in case of bad request: pass http exceptions up to calling func
            return r.json()
        else:
            r = requests.post(self._download_url(page, pagesize), 
                    headers=self._header(), json=selection)
            r.raise_for_status() # in case of bad request: pass http exceptions up to calling func
            return r.json()

  
    # takes: page size (i.e. number of download elements per page)
    #        and a selection in the form of a dictioanry or id string
    # downloads multiple pages of data based on provided Selection
    # returns: json dictionary of Download data
    def get_download(self, selection, pagesize=5):

        page = 0
        new_data = self._get_download(selection, page, pagesize)
        data = new_data

        while len(new_data) == pagesize:
            page += 1
            new_data = self._get_download(selection, page, pagesize)
            data.append(new_data)

        return data


    # creates new saved selection 
    # takes: Selection dictionary to be created
    # returns: json dictionary of newly created Selection 
    def create_selection(self, selection_object):
        r = requests.post(self._url('savedselections'), 
                headers=self._header(), data=json.dumps(selection_object) )
        r.raise_for_status()

        return r.json()


    # downloads data set to file based on selection provided 
    # takes: Selection dictionary list or id string to be created, and file format and name
    #        note, however, that file_format and name are only used when selection
    #        is sent as POST i.e. when it is provided as a dictionary instead of by
    #        id number
    # returns: ...
    def queue_download(self, selection, file_format="csv", name="databank_download"):
        if isinstance(selection, str):
            r = requests.get(self._url('queuedownload', selection), headers=self._header() )
        else:
            payload = {
                    "format": file_format,
                    "name": name, 
                    "selections": selection
                    }
            r = requests.post(self._url('queuedownload'), headers=self._header(), data=json.dumps(payload) )

        r.raise_for_status()
        return r.json()
       
    
    # downloads data set to file based on selection provided 
    # takes: Selection dictionary list or id string to be created, and file format and name
    #        note, however, that file_format and name are only used when selection
    #        is sent as POST i.e. when it is provided as a dictionary instead of by
    #        id number
    # returns: ...
    def download_file(self, selection, file_format="csv", name="databank_download"):
        if isinstance(selection, str):
            r = requests.get(self._url('filedownload', selection), headers=self._header() )
        else:
            payload = {
                    "format": file_format,
                    "name": name, 
                    "selections": selection
                    }
            r = requests.post(self._url('filedownload'), headers=self._header(), data=json.dumps(payload) )

        r.raise_for_status()
        return { 'data': str(r.content) }

    
    # takes: download id, returned from queue download method
    # returns: boolean indicating readiness
    def check_queue(self, ready_url):
        r = requests.get(ready_url, headers=self._header() )
        r.raise_for_status()
        return r.json()

    
    # takes: selection id and dictionary holding configuration
    # returns: a dataseries shaped based on the provided configuration as a 2-dimensional 
    #          list of json dictionaries (i.e. one for each cell in the table)
    def get_shaped_download(self, s_id, config_dict):
        r = requests.post(self._url('shapeddownload', s_id), headers=self._header(), data=json.dumps(config_dict))
        r.raise_for_status()
        return r.json()

    
    # takes: databank code string
    # returns: a json dictionary representing full tree of locations available in databank
    def get_location_tree(self, databank_code):
        r = requests.get(self._url('tree/Locations_' + databank_code), headers=self._header())
        r.raise_for_status()
        return r.json()

    
    # takes: databank code string
    # returns: a json dictionary representing full tree of indicators available in databank
    def get_indicators_tree(self, databank_code):
        r = requests.get(self._url('tree/Indicators_' + databank_code), headers=self._header())
        r.raise_for_status()
        return r.json()


# same as above; uses staging base url for testing
class StagingClient(Client):

    def __init__(self, api_key = "", base_url = STAGING_BASE):
        self.api_key = api_key
        self._base_url = base_url
