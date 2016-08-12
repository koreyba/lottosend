using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;

namespace LottoSend.com.TestCases
{
    /// <summary>
    /// Runs once before all the test in order to config the environment 
    /// </summary>
    [SetUpFixture]
    public class ConfigTests
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            IWebDriver driver = new ChromeDriver();
            try
            {
                CommonActions actions = new CommonActions(driver);
                actions.SwitchOffCombinedPaymentPage();
                driver.Dispose();
            }
            catch (Exception)
            {
               driver.Dispose();
            }
        }
    }
}
