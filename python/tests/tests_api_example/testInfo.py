import unittest
import sys
import os
sys.path.append(os.path.abspath('../..'))
import api_example
from api_client import Client, StagingClient
from requests import exceptions

SELECTION_ID = api_example.SELECTION_ID
API_KEY = api_example.API_KEY

class TestCase(unittest.TestCase):
    def setUp(self):
        self.client = Client(API_KEY)


class DatabankListCheckIdBlank(TestCase):
    def runTest(self):
        test_result = api_example.databank_test(self.client)
        self.assertNotEqual(test_result, None)

class DatabankListCheckIdValid(TestCase):
    def runTest(self):
        test_result = api_example.databank_test(self.client, 'WDMacro')
        self.assertNotEqual(test_result, None)

class DatabankListCheckIdInvalid(TestCase):
    def runTest(self):
        self.assertRaises(exceptions.HTTPError, api_example.databank_test, self.client, 'AWDMacro') 


class VariableListCheck(TestCase):
    def runTest(self):
        test_result = api_example.get_variables_test(self.client)
        self.assertNotEqual(test_result, None)


class RegionsListCheck(TestCase):
    def runTest(self):
        test_result = api_example.get_regions_test(self.client)
        self.assertNotEqual(test_result, None)


class UserGetValid(TestCase):
    def runTest(self):
        test_result = api_example.get_user_test(self.client)
        self.assertNotEqual(test_result, None)

class LocationTreeGetValid(TestCase):
    def runTest(self):
        test_result = api_example.location_tree_test(self.client)
        self.assertNotEqual(test_result, None)

class IndicatorTreeGetValid(TestCase):
    def runTest(self):
        test_result = api_example.indicator_tree_test(self.client)
        self.assertNotEqual(test_result, None)


if __name__ == '__main__':
    unittest.main()
