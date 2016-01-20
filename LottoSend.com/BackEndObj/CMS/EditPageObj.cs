using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.CMS
{
    /// <summary>
    /// Page object of CMS > Pages > edit page
    /// </summary>
    public class EditPageObj : DriverCover
    {
        public EditPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.FindElement(By.CssSelector("div.page-header > h2")).Text.Equals("Edit Page"))
            {
                throw new Exception("Sorry but it must be not 'Edit Page' page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        /// <summary>
        /// The first input field for content (doesn'a matter a language but the first one on the page)
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "#redactor-toolbar-0 + .redactor-editor:nth-child(2)")]
        private IWebElement _contentInputFirst;

        /// <summary>
        /// The first 'Update' button (for the first language on the page. The first one from the top)
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = ".center-column-content .form-horizontal:nth-child(2)  .btn.btn-primary.pull-right")] 
        private IWebElement _updateButtonFirst;

        /// <summary>
        /// Gets text of the first "Content" field on the page
        /// </summary>
        public string TextOfFirstContentInput
        {
            get { return _contentInputFirst.Text; }
        }

        /// <summary>
        /// Updates the first field "Content" on the page (doesn't matter which language is the first)
        /// </summary>
        /// <param name="content">new content of the field</param>
        public void UpdateFirstLanguageContent(string content)
        {
            _contentInputFirst.Clear();
            _contentInputFirst.SendKeys(content);
            _updateButtonFirst.Click();
            WaitForPageLoading();
            WaitAjax();
        }
    }
}
