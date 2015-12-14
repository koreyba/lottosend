using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.CMS
{
    public class NewPageObj : DriverCover
    {
        public NewPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("cms/sites/1/pages/new"))
            {
                throw new Exception("Sorry but it must be not 'new page creation' page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#page_label")]
        private IWebElement _labelInput;

        [FindsBy(How = How.CssSelector, Using = "#locale_en_slug")]
        private IWebElement _slugEnInput;

        [FindsBy(How = How.CssSelector, Using = "#locale_en_title")]
        private IWebElement _titleEnInput;

        [FindsBy(How = How.CssSelector, Using = "#locale_en_banner")]
        private IWebElement _bannerEnInput;

        //[FindsBy(How = How.CssSelector, Using = "div.CodeMirror-scroll")] 
       // private IWebElement _contentEnInput;

        [FindsBy(How = How.CssSelector, Using = "div.form-group.form-actions input.btn-primary")]
        private IWebElement _createButton;

        /// <summary>
        /// Creates a new page and returns it's key name
        /// </summary>
        /// <returns></returns>
        public string CreateKey()
        {
            string label = RandomGenerator.GenerateRandomString(10);

            _labelInput.SendKeys(label);
            _slugEnInput.SendKeys(label);
            _titleEnInput.SendKeys(label);
            //_contentEnInput.SendKeys(label); doesn't work

            _createButton.Click();
            WaitForPageLoading();
            WaitAjax();

            return label;
        }
    }
}
