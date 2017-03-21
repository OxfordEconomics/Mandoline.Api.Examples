using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;

namespace Tests
{
    [TestClass]
    public class InfoTests
    {
        // gets the full list of available databanks
        // expected: count of databanks > 0
        [TestMethod]
        public void TestGetDatabanks()
        {
            var output = new TestOutput();
            Info.RunGetDatabanksAsync(output).RunSync();
            Assert.IsTrue(output.returnValueInt > 0);
        }

        // gets the full list of variables for the macro databank 
        // expected: count of variables > 0
        [TestMethod]
        public void TestGetVariables()
        {
            var output = new TestOutput();
            Info.RunGetVariablesAsync(output).RunSync();
            Assert.IsTrue(output.returnValueInt > 0);
        }
    }
}
