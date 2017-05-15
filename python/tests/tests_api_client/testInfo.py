import unittest
import sys
import os
sys.path.append(os.path.abspath('../..'))
import api_example
from api_client import Client, StagingClient
from requests import exceptions

SELECTION_ID = api_example.SELECTION_ID

class TestCase(unittest.TestCase):
    def setUp(self):
        self.client = Client()

class DatabankIdBlank(TestCase):
    def runTest(self):
        test_result = self.client.get_databanks()
        self.assertNotEqual(test_result, None)

class DatabankIdValid(TestCase):
    def runTest(self):
        test_result = self.client.get_databanks('WDMacro')
        self.assertNotEqual(test_result, None)

class DatabankIdInvalid(TestCase):
    def runTest(self):
        self.assertRaises(exceptions.HTTPError, self.client.get_databanks, 'WDMacrom')

class VariablesValid(TestCase):
    def runTest(self):
        test_result = self.client.get_databank_variables('WDMacro')
        self.assertNotEqual(test_result, None)

class VariablesInvalid(TestCase):
    def runTest(self):
        test_result = self.client.get_databank_variables('WDMacrom')
        self.assertTrue(not test_result['Variables'])

class RegionsValid(TestCase):
    def runTest(self):
        test_result = self.client.get_databank_regions('WDMacro')
        self.assertNotEqual(test_result, None)

class RegionsInvalid(TestCase):
    def runTest(self):
        test_result = self.client.get_databank_regions('WDMacrom')
        self.assertNotEqual(test_result, None)

class UserGetBlank(TestCase):
    def runTest(self):
        test_result = self.client.get_user()
        self.assertNotEqual(test_result, None)

class UserGetValid(TestCase):
    def runTest(self):
        test_result = self.client.get_user( self.client.get_user()['ContactId'] )
        self.assertNotEqual(test_result, None)

class UserGetInvalid(TestCase):
    def runTest(self):
        self.assertRaises(exceptions.HTTPError, self.client.get_user, self.client.api_key + 'm')

class LocationTreeGetValid(TestCase):
    def runTest(self):
        test_result = self.client.get_location_tree('WDMacro')
        self.assertNotEqual(test_result, None)

class LocationTreeGetInvalid(TestCase):
    def runTest(self):
        self.assertRaises(exceptions.HTTPError, self.client.get_location_tree, 'WDMacrom')

class IndicatorTreeGetValid(TestCase):
    def runTest(self):
        test_result = self.client.get_indicators_tree('WDMacro')
        self.assertNotEqual(test_result, None)

class IndicatorTreeGetInvalid(TestCase):
    def runTest(self):
        self.assertRaises(exceptions.HTTPError, self.client.get_indicators_tree, 'WDMacrom')


if __name__ == '__main__':
    unittest.main()
