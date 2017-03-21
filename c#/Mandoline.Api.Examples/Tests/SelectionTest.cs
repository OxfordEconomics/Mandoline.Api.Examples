using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;

namespace Tests
{
    [TestClass]
    public class SelectionTest
    {
        // retrieves saved selection object based on selection id in settings
        // expected: selection object id matches provided selection id
        [TestMethod]
        public void SelectionGetTest()
        {
            var output = new TestOutput();
            SavedSelection.RunGetSavedSelection(AppConstants.SAVED_SELECTION_ID, output).RunSync();
            Assert.AreEqual(AppConstants.SAVED_SELECTION_ID.ToString(), output.returnValueStr);
        }

        // updates a saved selection
        // expected: selection's last update is > before test run
        [TestMethod]
        public void SelectionUpdateTest()
        {
            var preTestTime = DateTime.Now;
            var output = new TestOutput();
            SavedSelection.RunUpdateSavedSelection(output).RunSync();
            Assert.IsTrue(output.returnValueDate > preTestTime);
        }

        // creates a new non-temp saved selection
        // expected: id of newly created selection exists
        [TestMethod]
        public void SelectionCreateTest()
        {
            var output = new TestOutput();
            SavedSelection.RunGetSavedSelection(AppConstants.SAVED_SELECTION_ID, output).RunSync();
            Assert.AreNotEqual(string.Empty, output.returnValueStr);
        }
    }
}
