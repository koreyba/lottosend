using System;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;

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
            ChromeDriver chrome = new ChromeDriver();
            try
            {
                CommonActions actions = new CommonActions(chrome);
                actions.SwitchOffCombinedPaymentPage();
                chrome.Dispose();
            }
            catch (Exception)
            {
               chrome.Dispose();
            }
        }
    }
}
