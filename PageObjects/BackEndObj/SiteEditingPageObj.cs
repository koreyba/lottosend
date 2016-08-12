using System;
using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj
{
    /// <summary>
    /// Page object of site's editing/creating page
    /// </summary>
    public class SiteEditingPageObj : DriverCover
    {
        public SiteEditingPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("sites") || !Driver.Url.Contains("dit"))
            {
                throw new Exception("Sorry but it must be not site ediging page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//input[@id='site_combined_page']")]
        private IWebElement _combinedPageCheckBox;

        [FindsBy(How = How.CssSelector, Using = "#site_default_deposit_input li input")]
        private IList<IWebElement> _defaultdDepositAmounts;

        [FindsBy(How = How.CssSelector, Using = "form[id*=edit_site] > input[type='submit']")]
        private IWebElement _updateSiteButton;

        /// <summary>
        /// Switches on the "Combined Page" checkbox
        /// </summary>
        public void SwitchCombinedPageOn()
        {
            if (!IfCheckBoxIsChecked(_combinedPageCheckBox))
            {
                _combinedPageCheckBox.Click();
            }
            _updateSiteButton.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// Switches off the "Combined Page" checkbox
        /// </summary>
        public void SwitchCombinedPageOff()
        {
            if (IfCheckBoxIsChecked(_combinedPageCheckBox))
            {
                _combinedPageCheckBox.Click();
            }
            _updateSiteButton.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// Selects default deposit amound and updates the page
        /// </summary>
        /// <param name="amount"></param>
        public void SelectDefaultDepositAmount(string amount)
        {
            bool isFound = false;

            foreach (var el in _defaultdDepositAmounts)
            {
                if (el.GetAttribute("value").Equals(amount.ToString(CultureInfo.InvariantCulture)))
                {
                    el.Click();
                    isFound = true;
                }
            }

            if (isFound)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
            else
            {
                throw new Exception("Value that was expected to be selected as a default for deposit was not selected. Maybe there is no such value. Curren URL is: " + Driver.Url + " ");
            }
        }
    }
}
