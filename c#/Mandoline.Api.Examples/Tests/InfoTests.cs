namespace Tests
{
    using System;
    using Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class InfoTests
    {
        // gets the full list of available databanks
        // expected: count of databanks > 0
        [TestMethod]
        public void InfoDatabanksTest()
        {
            var output = new TestOutput();
            Info.RunGetDatabanksAsync(output).RunSync();
            Assert.IsTrue(output.ReturnValueInt > 0);
        }

        // gets the full list of variables for the macro databank
        // expected: count of variables > 0
        [TestMethod]
        public void InfoVariablesTest()
        {
            var output = new TestOutput();
            Info.RunGetVariablesAsync(output).RunSync();
            Assert.IsTrue(output.ReturnValueInt > 0);
        }
    }
}
