using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.CMS
{
    /// <summary>
    /// Common fields and methods for "Editing" pages in CMS (pages, snippets, translations)
    /// </summary>
    public class EditingPages : DriverCover
    {
        public EditingPages(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("edit"))
            {
                throw new Exception("Sorry but it must be not eddining page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        /// <summary>
        /// The first input field for content (doesn'a matter a language but the first one on the page)
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "#redactor-toolbar-0 + .redactor-editor:nth-child(2)")]
        protected IWebElement _contentInputFirst;

        /// <summary>
        /// The first 'Update' button (for the first language on the page. The first one from the top)
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = ".center-column-content .form-horizontal:nth-child(2)  .btn.btn-primary.pull-right")]
        protected IWebElement _updateButtonFirst;

        [FindsBy(How = How.CssSelector, Using = "div.center-column-content >  form > div.text-center  > input")]
        protected IWebElement _updateAllKeysButton;

        /// <summary>
        /// Gets text of the first "Content" field on the page
        /// </summary>
        public string TextOfFirstContentInput
        {
            get
            {
                if (_contentInputFirst.Text.Length != 0)
                {
                    return _contentInputFirst.Text;
                }
                else
                {
                    return _contentInputFirst.GetAttribute("value");
                }
            }
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

        /// <summary>
        /// Updates the first field "Content" on the page (doesn't matter which language is the first)
        /// </summary>
        /// <param name="content">new content of the field</param>
        public void UpdateFirstLanguageContent_EditPlus(string content)
        {
            _contentInputFirst.Clear();
            _contentInputFirst.SendKeys(content);
            ScrollDown();
            ZoomOut();
            _updateAllKeysButton.Click(); //TODO: check if scrolling is needed
            WaitForPageLoading();
            WaitAjax();
        }
    }
}
