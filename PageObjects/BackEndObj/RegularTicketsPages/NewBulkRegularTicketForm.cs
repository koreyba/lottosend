using System;
using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj.RegularTicketsPages
{
    /// <summary>
    /// Page Object of a form on a regulaer package editing page that is invoked by clicking "New Bulk Buy" button
    /// </summary>
    public class NewBulkRegularTicketForm : DriverCover
    {
        public NewBulkRegularTicketForm(IWebDriver driver) : base(driver)
        {
            if (!Driver.FindElement(
                    By.XPath(
                        "//*[@class='ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-draggable ui-resizable']")).Displayed)
            {
                throw new Exception("Sorry, it must be not NewBulk form. Check if it is displyaed on current URL: " + Driver.Url + " ");
            }
        }

        [FindsBy(How = How.XPath, Using = "//*[@id='bulk_amount_of_draws']")]
        private IWebElement _amountOfDraws;

        [FindsBy(How = How.XPath, Using = "//*[@id='bulk_discount']")]
        private IWebElement _discount;

        [FindsBy(How = How.XPath, Using = "//*[@id='bulk_active']")]
        private IWebElement _active;

        [FindsBy(How = How.XPath, Using = "//*[@class='choices-group']//label")]
        private IList<IWebElement> _sites;

        [FindsBy(How = How.XPath, Using = "//div[@class='actions']/input[@type='submit']")]
        private IWebElement _createBulkButton;

        /// <summary>
        /// Creates a bulk buy for regular game with specific parameters
        /// </summary>
        /// <param name="numberOfDraws">Number of draws to play</param>
        /// <param name="discount">Amount of discount</param>
        /// <param name="active">Is active?</param>
        /// <param name="siteNames">List of strings (site names)</param>
        public void AddNewBulkBuy(int numberOfDraws, double discount, bool active, IList<string> siteNames)
        {
            _amountOfDraws.SendKeys(numberOfDraws.ToString());
            _discount.Clear();
            _discount.SendKeys(discount.ToString(CultureInfo.InvariantCulture));

            if (active)
            {
                if (!IfCheckBoxIsChecked(_active))
                {
                    _active.Click();
                }
            }

            foreach (var name in siteNames)
            {
                ClickCheckboxInList(_sites, name);
            }

            _createBulkButton.Click();
            WaitAjax();
        }
    }
}
