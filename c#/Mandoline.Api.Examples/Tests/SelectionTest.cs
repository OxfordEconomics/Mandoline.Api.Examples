namespace Tests
{
    using System;
    using Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SelectionTest
    {
        // retrieves saved selection object based on selection id in settings
        // expected: selection object id matches provided selection id
        [TestMethod]
        public void SelectionGetTest()
        {
            var output = new TestOutput();
            SavedSelection.RunGetSavedSelection(AppConstants.SavedSelectionId, output).RunSync();
            Assert.AreEqual(AppConstants.SavedSelectionId.ToString(), output.ReturnValueStr);
        }

        // updates a saved selection
        // expected: selection's last update is > before test run
        [TestMethod]
        public void SelectionUpdateTest()
        {
            var preTestTime = DateTime.Now;
            var output = new TestOutput();
            SavedSelection.RunUpdateSavedSelection(output).RunSync();
            Assert.IsTrue(output.ReturnValueDate > preTestTime);
        }

        // creates a new non-temp saved selection
        // expected: id of newly created selection exists
        [TestMethod]
        public void SelectionCreateTest()
        {
            var output = new TestOutput();
            SavedSelection.RunGetSavedSelection(AppConstants.SavedSelectionId, output).RunSync();
            Assert.AreNotEqual(string.Empty, output.ReturnValueStr);
        }
    }
}
