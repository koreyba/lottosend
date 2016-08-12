﻿using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NewCombinedPageConfigTests.TestCases
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
                TestFramework.CommonActions actions = new TestFramework.CommonActions(driver);
                actions.SwitchOnCombinedPaymentPage();
                driver.Dispose();
            }
            catch (Exception)
            {
               driver.Dispose();
            }
        }
    }
}