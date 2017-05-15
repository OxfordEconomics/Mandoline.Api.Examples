import unittest
import sys
import os
sys.path.append(os.path.abspath('../..'))
import api_example
from api_client import Client, StagingClient
from requests import exceptions
import json
import time

SELECTION_ID = api_example.SELECTION_ID

class TestCase(unittest.TestCase):
    def setUp(self):
        self.client = Client()

class DownloadIdValid(TestCase):
    def runTest(self):
        test_result = self.client.get_download(SELECTION_ID)
        self.assertNotEqual(test_result, None)

class DownloadIdInvalid(TestCase):
    def runTest(self):
        self.assertRaises(exceptions.HTTPError, self.client.get_download, SELECTION_ID + "m")

class DownloadDictionaryValid(TestCase):
    def runTest(self):
        selection = self.client.get_selection(SELECTION_ID)
        test_result = self.client.get_download(selection)
        self.assertNotEqual(test_result, None)

class DownloadDictionaryInvalid1(TestCase):
    def runTest(self):
        selection = [self.client.get_selection(SELECTION_ID)]
        self.assertRaises(exceptions.HTTPError, self.client.get_download, selection)

class DownloadDictionaryInvalid2(TestCase):
    def runTest(self):
        selection = self.client.get_selection(SELECTION_ID)
        selection['DatabankCode'] += 'm'
        self.assertRaises(exceptions.HTTPError, self.client.get_download, selection)

class DownloadDictionaryInvalid3(TestCase):
    def runTest(self):
        selection = json.dumps(self.client.get_selection(SELECTION_ID))
        self.assertRaises(exceptions.HTTPError, self.client.get_download, selection)

class QueueDownloadIdValid(TestCase):
    def runTest(self):
        test_result = self.client.queue_download(SELECTION_ID)
        self.assertNotEqual(test_result, None)

        iterations = 0
        while True:
            success = self.client.check_queue(test_result['ReadyUrl'])
            if success == True or iterations > 7:
                break
            time.sleep(2)
        self.assertTrue(success)

class QueueDownloadIdInvalid(TestCase):
    def runTest(self):
        self.assertRaises(exceptions.HTTPError, self.client.queue_download, SELECTION_ID + 'm')

class QueueDownloadDictionaryValid(TestCase):
    def runTest(self):
        test_result = self.client.queue_download(self.client.get_selection(SELECTION_ID))
        self.assertNotEqual(test_result, None)

        iterations = 0
        while True:
            success = self.client.check_queue(test_result['ReadyUrl'])
            if success == True or iterations > 7:
                break
            time.sleep(2)
        self.assertTrue(success)

# class QueueDownloadIdInvalid(TestCase):

# class QueueDownloadDictionaryInvalid(TestCase):

class DownloadFileIdValid(TestCase):
    def runTest(self):
        test_result = self.client.download_file(SELECTION_ID)
        self.assertNotEqual(test_result, None)

class DownloadFileDictionaryValid(TestCase):
    def runTest(self):
        test_result = self.client.download_file(self.client.get_selection(SELECTION_ID))
        self.assertNotEqual(test_result, None)

# class DownloadFileIdInvalid(TestCase):

# class DownloadFileDictionaryInvalid(TestCase):

class DownloadShapedIdValid(TestCase):
    def runTest(self):
        config_dict = {
                'pivot': 'true',
                'StackedQuarters': 'true',
                'Frequency': 'Annual'
                }
        test_result = self.client.get_shaped_download(SELECTION_ID, config_dict)
        self.assertNotEqual(test_result, None)

class DownloadShapedIdInvalid(TestCase):
    def runTest(self):
        config_dict = {
                'pivot': 'true',
                'StackedQuarters': 'true',
                'Frequency': 'Annual'
                }
        self.assertRaises(exceptions.HTTPError, self.client.get_shaped_download, SELECTION_ID + 'm', config_dict)


if __name__ == '__main__':
    unittest.main()
