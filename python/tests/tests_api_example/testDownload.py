import unittest
import sys
import os
sys.path.append(os.path.abspath('../..'))
import api_example
from api_client import Client, StagingClient
from requests import exceptions
import multiprocessing

SELECTION_ID = api_example.SELECTION_ID
API_KEY = api_example.API_KEY
TIMEOUT = 60 # seconds

class TestCase(unittest.TestCase):
    def setUp(self):
        self.client = Client(API_KEY)

class DownloadValid(TestCase):
    def runTest(self):
        test_result = api_example.download_test(self.client, SELECTION_ID)
        self.assertNotEqual(test_result, None)

class DownloadInvalid(TestCase):
    def runTest(self):
        self.assertRaises(exceptions.HTTPError, api_example.download_test, self.client, SELECTION_ID + "broken")

class QueueTestValid(TestCase):
    def runTest(self):
        self.assertTrue(api_example.queue_download_test(self.client, api_example.sampleSelect, 5))

# runs the correct queue download query and then runs it again a second time with a minor
# error that would otherwise complete in the same amount of time
# expected: second_timeout is larger than the first because error queries will always return
# true when checked
class QueueTestInvalid(TestCase):
    def runTest(self):
        first_timeout = api_example.queue_download_test(self.client, api_example.sampleSelect)

        selection = api_example.sampleSelect
        selection['DatabankCode'] = 'WDMacro-broken'
        selection['Id'] = ''
        second_timeout = api_example.queue_download_test(self.client, selection, first_timeout + 3)

        self.assertTrue(first_timeout < second_timeout)

class FileDownloadTestValid(TestCase):
    def runTest(self):
        test_result = api_example.file_download_test(self.client, api_example.sampleSelect)
        self.assertNotEqual(test_result, None)

# class FileDownloadTestInvalid(TestCase):
#     def runTest(self):
#         selection = api_example.sampleSelect
#         selection['DatabankCode'] = 'WDMacro-broken'
#         selection['Id'] = ''
#         test_result = api_example.file_download_test(self.client, selection)
#         self.assertNotEqual(test_result, None)


if __name__ == '__main__':
    unittest.main()
