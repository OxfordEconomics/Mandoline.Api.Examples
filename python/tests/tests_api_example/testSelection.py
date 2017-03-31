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

class SelectionGetValid(TestCase):
    def runTest(self):
        test_result = api_example.get_selection_test(self.client, SELECTION_ID)
        self.assertNotEqual(test_result, None)

class SelectionGetInvalid(TestCase):
    def runTest(self):
        self.assertRaises(exceptions.HTTPError, api_example.get_selection_test, self.client, SELECTION_ID + "broken")


class SelectionUpdateValid(TestCase):
    def runTest(self):
        initial_selection = api_example.get_selection_test(self.client, SELECTION_ID)
        initial_name = initial_selection['Name']

        updated_selection = api_example.update_selection_test(self.client, SELECTION_ID)
        updated_name = updated_selection['Name']

        self.assertNotEqual(initial_name, updated_name) 

class SelectionUpdateInvalid(TestCase):
    def runTest(self):
        self.assertRaises(exceptions.HTTPError, api_example.update_selection_test, self.client, SELECTION_ID + "broken")


class SelectionGetValid(TestCase):
    def runTest(self):
        test_result = api_example.create_selection_test(self.client)
        self.assertNotEqual(test_result, None)


if __name__ == '__main__':
    unittest.main()
