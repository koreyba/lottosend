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

        [FindsBy(How = How.XPath, Using = "//input[@id='site_new_sign_up']")]
        private IWebElement _newSignUpCheckBox;

        [FindsBy(How = How.XPath, Using = "//input[@id='site_address']")]
        private IWebElement _addressCheckBox;

        [FindsBy(How = How.XPath, Using = "//input[@id='site_add_to_cart']")]
        private IWebElement _addToCartCheckBox;

        [FindsBy(How = How.XPath, Using = "//input[@id='site_group_game']")]
        private IWebElement _groupGameCheckBox;

        [FindsBy(How = How.XPath, Using = "//input[@id='site_single_game']")]
        private IWebElement _singleGameCheckBox;

        [FindsBy(How = How.XPath, Using = "//input[@id='site_country']")]
        private IWebElement _countryCheckBox;

        [FindsBy(How = How.XPath, Using = "//input[@id='site_one_time_entry']")]
        private IWebElement _oneTimeEntryCheckBox;

        [FindsBy(How = How.XPath, Using = "//input[@id='site_combined_page']")]
        private IWebElement _combinedPageCheckBox;

        [FindsBy(How = How.CssSelector, Using = "#site_default_deposit_input li input")]
        private IList<IWebElement> _defaultdDepositAmounts;

        [FindsBy(How = How.CssSelector, Using = "form[id*=edit_site] > input[type='submit']")]
        private IWebElement _updateSiteButton;

        /// <summary>
        /// Switches on the "Combined Page" checkbox
        /// </summary>
        public void SwitchNewSignUpOn(bool updatePage = false)
        {
            if (!IfCheckBoxIsChecked(_newSignUpCheckBox))
            {
                _newSignUpCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches off the "Combined Page" checkbox
        /// </summary>
        public void SwitchNewSignUpOff(bool updatePage = false)
        {
            if (IfCheckBoxIsChecked(_newSignUpCheckBox))
            {
                _newSignUpCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches on the "Combined Page" checkbox
        /// </summary>
        public void SwitchAddressOn(bool updatePage = false)
        {
            if (!IfCheckBoxIsChecked(_addressCheckBox))
            {
                _addressCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches off the "Combined Page" checkbox
        /// </summary>
        public void SwitchAddressOff(bool updatePage = false)
        {
            if (IfCheckBoxIsChecked(_addressCheckBox))
            {
                _addressCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches on the "Combined Page" checkbox
        /// </summary>
        public void SwitchAddToCartOn(bool updatePage = false)
        {
            if (!IfCheckBoxIsChecked(_addToCartCheckBox))
            {
                _addToCartCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches off the "Combined Page" checkbox
        /// </summary>
        public void SwitchAddToCartOff(bool updatePage = false)
        {
            if (IfCheckBoxIsChecked(_addToCartCheckBox))
            {
                _addToCartCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches on the "Combined Page" checkbox
        /// </summary>
        public void SwitchGroupGameOn(bool updatePage = false)
        {
            if (!IfCheckBoxIsChecked(_groupGameCheckBox))
            {
                _groupGameCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches off the "Combined Page" checkbox
        /// </summary>
        public void SwitchGroupGameOff(bool updatePage = false)
        {
            if (IfCheckBoxIsChecked(_groupGameCheckBox))
            {
                _groupGameCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches on the "Combined Page" checkbox
        /// </summary>
        public void SwitchSingleGameOn(bool updatePage = false)
        {
            if (!IfCheckBoxIsChecked(_singleGameCheckBox))
            {
                _singleGameCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches off the "Combined Page" checkbox
        /// </summary>
        public void SwitcSingleGameOff(bool updatePage = false)
        {
            if (IfCheckBoxIsChecked(_singleGameCheckBox))
            {
                _singleGameCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches on the "Combined Page" checkbox
        /// </summary>
        public void SwitchCountryOn(bool updatePage = false)
        {
            if (!IfCheckBoxIsChecked(_countryCheckBox))
            {
                _countryCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches off the "Combined Page" checkbox
        /// </summary>
        public void SwitchCountryOff(bool updatePage = false)
        {
            if (IfCheckBoxIsChecked(_countryCheckBox))
            {
                _countryCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches on the "Combined Page" checkbox
        /// </summary>
        public void SwitchOneTimeEntryOn(bool updatePage = false)
        {
            if (!IfCheckBoxIsChecked(_oneTimeEntryCheckBox))
            {
                _oneTimeEntryCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches off the "Combined Page" checkbox
        /// </summary>
        public void SwitchOneTimeEntryOff(bool updatePage = false)
        {
            if (IfCheckBoxIsChecked(_oneTimeEntryCheckBox))
            {
                _oneTimeEntryCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches on the "Combined Page" checkbox
        /// </summary>
        public void SwitchCombinedPageOn(bool updatePage = false)
        {
            if (!IfCheckBoxIsChecked(_combinedPageCheckBox))
            {
                _combinedPageCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
        }

        /// <summary>
        /// Switches off the "Combined Page" checkbox
        /// </summary>
        public void SwitchCombinedPageOff(bool updatePage = false)
        {
            if (IfCheckBoxIsChecked(_combinedPageCheckBox))
            {
                _combinedPageCheckBox.Click();
            }
            if (updatePage)
            {
                _updateSiteButton.Click();
                WaitForPageLoading();
            }
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
