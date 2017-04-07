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

class GetSelectionValid(TestCase):
    def runTest(self):
        test_result = self.client.get_selection(SELECTION_ID)
        self.assertNotEqual(test_result, None)

class GetSelectionInvalid(TestCase):
    def runTest(self):
        self.assertRaises(exceptions.HTTPError, self.client.get_selection, SELECTION_ID + 'm')

class DeleteCreateSelectionValid(TestCase):
    def runTest(self):
        selection_template = self.client.get_selection(SELECTION_ID)
        selection_template['Id'] = ''
        selection = self.client.create_selection(selection_template)
        self.assertNotEqual(selection, None)
        try:
            self.client.get_selection(selection['Id'])
        except:
            self.fail("Selection creation failed unexpectedly...")
        self.client.delete_selection(selection['Id'])
        self.assertRaises(exceptions.HTTPError, self.client.get_selection, selection['Id'])

# class CreateSelectionInvalid(TestCase):

class UpdateSelectionValid(TestCase):
    def runTest(self):
        old_selection = self.client.get_selection(SELECTION_ID)
        old_name = old_selection['Name']
        old_selection['Name'] += 'u'
        self.client.update_selection(old_selection)
        new_selection = self.client.get_selection(old_selection['Id'])
        self.assertEqual(new_selection['Name'], old_name + 'u')

# class UpdateSelectionInvalid(TestCase):


if __name__ == '__main__':
    unittest.main()
