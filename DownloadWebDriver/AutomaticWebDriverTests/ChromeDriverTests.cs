using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomaticWebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticWebDriver.Tests
{
    [TestClass()]
    public class ChromeDriverTests
    {
        string pathChromeDrive = @"C:\Users\MACHINE\Downloads\chromedriver.exe";
        string path = @"C:\Users\MACHINE\Downloads\";

        [TestMethod()]
        public void VersionTest()
        {
            Assert.IsTrue(!string.IsNullOrEmpty(ChromeDriver.Version()));
        }
        [TestMethod()]
        public void VersionTestFalse()
        {
            string version = ChromeDriver.Version();
                Assert.IsNotNull(version);
        }

        [TestMethod()]
        public void DownloadTest()
        {
            if (File.Exists(pathChromeDrive))
                File.Delete(pathChromeDrive);
            Assert.IsTrue(ChromeDriver.Download(path));
        }

        [TestMethod()]
        public void DownloadTestIsFalse()
        {
                Assert.IsFalse(ChromeDriver.Download(path));
        }

        [TestMethod()]
        public void ExistsTest()
        {
            Assert.IsTrue(ChromeDriver.Exists(path));
        }

        [TestMethod()]
        public void ExistsTestFalse()
        {
            if (File.Exists(pathChromeDrive))
                File.Delete(pathChromeDrive);
            Assert.IsFalse(ChromeDriver.Exists(path));
        }

        [TestMethod()]
        public void OverwriteTest()
        {
            if (File.Exists(pathChromeDrive))
                Assert.IsTrue(ChromeDriver.Overwrite(path));
            else
            {
                ChromeDriver.Download(path);
                Assert.IsTrue(ChromeDriver.Overwrite(path));
            }
        }

        [TestMethod()]
        public void OverwriteTestFalse()
        {
            if (File.Exists(pathChromeDrive))
                File.Delete(pathChromeDrive);
            Assert.IsFalse(ChromeDriver.Overwrite(path));
        }
    }
}