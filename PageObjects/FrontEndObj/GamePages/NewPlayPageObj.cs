using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.GamePages
{
    /// <summary>
    /// Base class page object for new play pages
    /// </summary>
    public class NewPlayPageObj : DriverCover
    {
        public NewPlayPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("play"))
            {
                throw new Exception("It's not a play page for sure. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = ".filter-option.pull-left")]
        protected IList<IWebElement> _drawOptionsSelects;

        [FindsBy(How = How.XPath, Using = "//button[contains(@class, 'btn-primary')]")]
        protected IList<IWebElement> _addToCartButtons;
    }
}
