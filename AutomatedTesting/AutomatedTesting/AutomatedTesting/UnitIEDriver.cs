using System;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using Selenium;
using System.Reflection;
using OpenQA.Selenium.Remote;

namespace AutomatedTesting
{
    [TestClass]
    public class UnitIEDriver
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;

        [TestInitialize]
        public void init()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\dll";

            var service = InternetExplorerDriverService.CreateDefaultService(path, "IEDriverServer.exe");
            service.LoggingLevel = InternetExplorerDriverLogLevel.Trace;
            service.LogFile = path + "\\log.txt";

            InternetExplorerOptions option = new InternetExplorerOptions();
            option.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            
            driver = new InternetExplorerDriver(service, option);
            verificationErrors = new StringBuilder();
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                if (driver != null)
                {
                    driver.Quit();
                }
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [TestMethod]
        public void test1()
        {
            var nav = driver.Navigate();
            nav.GoToUrl("https://www.google.com.hk/");
            driver.FindElement(By.Id("lst-ib")).SendKeys("Thread c#");
            driver.FindElement(By.Name("btnK")).Click();


            System.Threading.Thread.Sleep(500);
            //Assert.AreEqual("Google", driver.Title);
            //Assert.AreEqual("Thread c#", driver.Title);
            Assert.IsTrue(driver.Title.Contains("Thread c#"));
        }
    }
}
