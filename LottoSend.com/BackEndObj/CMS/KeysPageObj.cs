using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.CMS
{
    /// <summary>
    /// Page object of "Keys" pages in CMS (translations, snippets, pages)
    /// </summary>
    public class KeysPageObj : DriverCover
    {
        public KeysPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("cms/sites/1/"))
            {
                throw new Exception("Sorry but it must be not 'Pages/Snippets/Translations' page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "div.page-header a.btn.btn-default:nth-child(4)")]
        private IWebElement _newButton;

        [FindsBy(How = How.CssSelector, Using = ".search-input.form-control.ui-autocomplete-input")]
        private IWebElement _searchInput;

        [FindsBy(How = How.CssSelector, Using = ".btn.search-submit")]
        private IWebElement _searchKeyButton; //div > .table.table-hover.table-bordered:nth-child(2) > tbody > tr

        [FindsBy(How = How.CssSelector, Using = "div > .table.table-hover.table-bordered:nth-child(2) > tbody > tr")]
        private IList<IWebElement> _records;

        private string _deleteButton = ".btn.btn-small.btn-danger";

        [FindsBy(How = How.CssSelector, Using = "#search_by")]
        private IWebElement _searchBySelect;

        /// <summary>
        /// Click "New" button on "Pages" page
        /// </summary>
        /// <returns>new page creation object</returns>
        public NewPageObj ClickNewButton()
        {
            _newButton.Click();
            WaitForPageLoading();
            WaitAjax();

            return new NewPageObj(Driver);
        }

        /// <summary>
        /// Removes the key. You have to be on "Pages" page
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            if (SearchKey(key, SearchBy.Key) == 0)
            {
                throw new Exception("Sorry but the key '" + key + "' was not found on the current page: " + Driver.Url + " ");
            }

            foreach (IWebElement tr in _records)
            {
                IList<IWebElement> keyNemes = tr.FindElements(By.CssSelector("td:nth-child(3) > a"));
                foreach (IWebElement name in keyNemes)
                {
                    if (name.Text.Equals(key))
                    {
                        break;
                    }
                }

                tr.FindElement(By.CssSelector(_deleteButton)).Click();
                bool w = WaitjQuery();
            }
        }

        /// <summary>
        /// Provide searching of a key and tells how many keys were found. You have to be on "Pages" page
        /// </summary>
        /// <param name="key"></param>
        /// <param name="howToSearch"></param>
        /// <returns>Number of keys found</returns>
        public int SearchKey(string key, SearchBy howToSearch)
        {
            if (howToSearch == SearchBy.Content)
            {
                ChooseElementInSelect("Content", _searchBySelect, SelectBy.Text);
            }

            _searchInput.SendKeys(key);
            _searchKeyButton.Click();
            WaitAjax();

            return Driver.FindElements(By.CssSelector("div > .table.table-hover.table-bordered:nth-child(2) > tbody > tr")).Count;
        }

        /// <summary>
        /// Clicks on the key and opens its editing page
        /// </summary>
        /// <param name="key"></param>
        public void ClickEditKey(string key)
        {
            SearchKey(key, SearchBy.Key);
            Driver.FindElement(By.LinkText(key)).Click();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Clicks on Edit+ button of the key and opens its editing+ page
        /// </summary>
        /// <param name="key"></param>
        public void ClickEditPlusForKey(string key)
        {
            SearchKey(key, SearchBy.Key);
            Driver.FindElement(By.LinkText("Edit +")).Click();
            WaitForPageLoading();
            WaitAjax();
        }
    }
}
