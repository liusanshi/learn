using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;  
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using Selenium;

namespace AutomatedTesting
{
    [TestClass]
    public class UnitChromeDriver
    {
        private ISelenium selenium;
        private StringBuilder verificationErrors;

        [TestInitialize]
        public void init()
        {
            selenium = new DefaultSelenium("127.0.0.1", 4444, "*chrome", "http://www.google.com/");
            selenium.Start();
            verificationErrors = new StringBuilder();
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                selenium.Stop();
                //selenium.Close();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [TestMethod]
        public void TheNewTest()
        {

            Assert.IsTrue(true);
            //selenium.Open("/");
            //selenium.Type("q", "selenium rc");
            //selenium.Click("btnG");
            //selenium.WaitForPageToLoad("30000");
            //Assert.AreEqual("selenium rc - Google Search", selenium.GetTitle());
        }
    }
}
