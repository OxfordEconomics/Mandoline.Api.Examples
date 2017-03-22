using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;

namespace Tests
{
    [TestClass]
    public class DownloadShapedTests
    {
        // gets a sample data download from the macro databank
        // expected: count of data rows > 0
        [TestMethod]
        public void DownloadShapedTest()
        {
            var output = new TestOutput();
            DownloadShaped.RunDownloadShapedAsync(output).RunSync();
            Assert.IsTrue(output.ReturnValueInt > 0);
        }

        // gets a sample data download from the macro databank
        // expected: count of data rows > 0
        [TestMethod]
        public void DownloadShapedStreamTest()
        {
            var output = new TestOutput();
            DownloadShaped.RunDownloadShapedStreamAsync(output).RunSync();
            Assert.IsTrue(output.ReturnValueInt > 0);
        }

    }
}
