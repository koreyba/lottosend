using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.GamePages
{
    /// <summary>
    /// Page object of a new play page on the site (web)
    /// </summary>
    public class NewGroupGamePageObj : DriverCover
    {
        public NewGroupGamePageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.FindElement(By.XPath("//*[@id='group']")).GetAttribute("class").Equals("active"))
            {
                throw new Exception("Sorry, it must be not new group game page ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//button[contains(@class, 'btn-primary')]")]
        private IList<IWebElement> _addToCartButtons;

        [FindsBy(How = How.XPath, Using = "//a[@class='plus']")]
        private IList<IWebElement> _plusButtons;

        [FindsBy(How = How.XPath, Using = "//*[contains(@id, 'slide')]")]
        private IList<IWebElement> _tickets;

        /// <summary>
        /// Adds shares to selected ticket (from 1 to 5)
        /// </summary>
        /// <param name="numberOfShares">How many shares to add</param>
        public void AddShares(int numberOfShares)
        {
            _tickets[numberOfShares].Click();

            IWebElement plus = GetFirstVisibleElementFromList(_plusButtons);
            for (int i = 0; i < numberOfShares; ++i)
            {
                plus.Click();
            }
        }
    }
}
