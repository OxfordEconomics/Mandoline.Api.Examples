namespace Tests
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DownloadShapedTests
    {
        // gets a sample data download from the macro databank
        // expected: count of data rows > 0
        [TestMethod]
        public async Task DownloadShapedTest()
        {
            var output = new TestOutput();
            await DownloadShaped.RunDownloadShapedAsync(output);
            Assert.IsTrue(output.ReturnValueInt > 0);
        }

        // gets a sample data download from the macro databank
        // expected: count of data rows > 0
        [TestMethod]
        public async Task DownloadShapedStreamTest()
        {
            var output = new TestOutput();
            await DownloadShaped.RunDownloadShapedStreamAsync(output);
            Assert.IsTrue(output.ReturnValueInt > 0);
        }

    }
}
