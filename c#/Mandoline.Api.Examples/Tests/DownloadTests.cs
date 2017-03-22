namespace Tests
{
    using System;
    using Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DownloadTests
    {
        // gets a sample data download from the macro databank
        // expected: count of dataseries > 0
        [TestMethod]
        public void DownloadTest()
        {
            var output = new TestOutput();
            Download.RunDownloadAsync(output).RunSync();
            Assert.IsTrue(output.ReturnValueInt > 0);
        }

        // gets a sample data download from the macro databank
        // expected: count of variables > 0
        [TestMethod]
        public void DownloadRequestTest()
        {
            var output = new TestOutput();
            Download.RunRequestDownloadAsync(output).RunSync();
            Assert.AreNotEqual(output.ReturnValueStr, string.Empty);
        }

        // gets the full list of variables for the macro databank
        // expected: count of variables > 0
        [TestMethod]
        public void DownloadFileTest()
        {
            var output = new TestOutput();
            Download.RunDownloadFileAsync(output).RunSync();
            Assert.AreNotEqual(output.ReturnValueStr, string.Empty);
        }
    }
}
