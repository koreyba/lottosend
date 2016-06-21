using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.RegularTicketsPages
{
    /// <summary>
    /// Page Object of a regulaer ticket's package creation/editing page
    /// </summary>
    public class NewPackagePageObj : DriverCover
    {
        public NewPackagePageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("packages"))
            {
                throw new Exception("Sorry it must be not a package creation/editing page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//*[@id='package_template_id']")]
        private IWebElement _template;

        [FindsBy(How = How.XPath, Using = "//*[@id='package_kind']")]
        private IWebElement _kind;

        [FindsBy(How = How.XPath, Using = "//*[@id='package_number_of_lines']")]
        private IWebElement _numberOfLines;

        [FindsBy(How = How.XPath, Using = "//*[@id='package_single_buy']")]
        private IWebElement _single;

        [FindsBy(How = How.XPath, Using = "//*[@id='package_bulk_buy']")]
        private IWebElement _bulkbuy;

        [FindsBy(How = How.XPath, Using = "//*[@id='package_visible']")]
        private IWebElement _visible;

        [FindsBy(How = How.XPath, Using = "//*[@id='package_active']")]
        private IWebElement _active;

        [FindsBy(How = How.XPath, Using = "//*[@class='choices-group']//label")]
        private IList<IWebElement> _sites;

        [FindsBy(How = How.XPath, Using = "//*[@id='package_submit_action']/input")]
        private IWebElement _createPackageButton;

        [FindsBy(How = How.XPath, Using = " //a[@class='button']")]
        private IWebElement _newBulkBuyButotn;

        /// <summary>
        /// Activates a specific site and updates the package
        /// </summary>
        /// <param name="siteName"></param>
        public void ActivateSite(string siteName)
        {
            foreach (var name in _sites)
            {
                if (name.Text.Equals(siteName))
                {
                    if (!IfCheckBoxIsChecked(name.FindElement(By.XPath("..//input"))))
                    {
                        name.Click();
                    }
                }
            }

            _createPackageButton.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// Deactivates a specific site and updates the package
        /// </summary>
        /// <param name="siteName"></param>
        public void DeactivateSite(string siteName)
        {
            foreach (var name in _sites)
            {
                if (name.Text.Equals(siteName))
                {
                    if (IfCheckBoxIsChecked(name.FindElement(By.XPath("..//input"))))
                    {
                        name.Click();
                    }
                }
            }

            _createPackageButton.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// Adds a new bulk-buy for the package and updates it
        /// </summary>
        /// <param name="numberOfDraws"></param>
        /// <param name="discount"></param>
        /// <param name="active"></param>
        /// <param name="siteNames"></param>
        public void AddBulkBuyToThePackage(int numberOfDraws, double discount, bool active, IList<string> siteNames)
        {
            _newBulkBuyButotn.Click();
            WaitAjax();

            NewBulkRegularTicketForm bulkForm = new NewBulkRegularTicketForm(Driver);
            bulkForm.AddNewBulkBuy(numberOfDraws, discount, active, siteNames);

            _createPackageButton.Click();
            WaitAjax();
            WaitForPageLoading();
        }

        /// <summary>
        /// Creates a new package with set parameters
        /// </summary>
        /// <param name="template"></param>
        /// <param name="numberOfLines"></param>
        /// <param name="singleBuy"></param>
        /// <param name="bulkBuy"></param>
        /// <param name="visible"></param>
        /// <param name="active"></param>
        /// <param name="site"></param>
        public void CreatePackage(string template, int numberOfLines, bool singleBuy, bool bulkBuy, bool visible,
            bool active, string site)
        {
            ChooseElementInSelect(template, _template, SelectBy.Text);
            ChooseElementInSelect("lines", _kind, SelectBy.Value);
            _numberOfLines.SendKeys(numberOfLines.ToString());

            if (singleBuy)
            {
                if(!IfCheckBoxIsChecked(_single))
                _single.Click();
            }

            if (bulkBuy)
            {
                if (!IfCheckBoxIsChecked(_bulkbuy))
                    _bulkbuy.Click();
            }

            if (visible)
            {
                if (!IfCheckBoxIsChecked(_visible))
                    _visible.Click();
            }

            if (active)
            {
                if (!IfCheckBoxIsChecked(_active))
                    _active.Click();
            }

            ClickCheckboxInList(_sites, site);

            _createPackageButton.Click();
            WaitAjax();
            WaitForPageLoading();
        }
    }
}
