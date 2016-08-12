using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj.CMS
{
    /// <summary>
    /// Page objct of "mass edit" page (for snippets/translations/pages)
    /// </summary>
    public class MassEditPageObj : EditingPages
    {
        public MassEditPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("mass_edit"))
            {
                throw new Exception("Sorry but it must be not 'mass edit' page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        /// <summary>
        /// The first 'Update' button (for the first language on the page. The first one from the top)
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "div.form-group > div:nth-child(6)  input.btn-primary")]
        private IWebElement _updateButtonFirst;

        [FindsBy(How = How.CssSelector, Using = ".search-input")]
        private IWebElement _searchInputField;

        [FindsBy(How = How.CssSelector, Using = "form.form-inline > div > input[type=submit]")]
        private IWebElement _searchKeyButton;

        /// <summary>
        /// Searches for the key
        /// </summary>
        /// <param name="key"></param>
        public void SearchKey(string key)
        {
            _searchInputField.SendKeys(key);
            _searchKeyButton.Click();
            WaitAjax();
            WaitForPageLoading();
        }
    }
}
