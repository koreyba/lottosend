using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.SalesPanelPages
{
    public class SingleGameObj : DriverCover
    {
        public SingleGameObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.FindElement(By.Id("quick")).Displayed)
            {
                throw new Exception("Sorry but it must be not single game page. Please check it. ");
            }

            PageFactory.InitElements(Driver, this);
        }
        
        [FindsBy(How = How.CssSelector, Using = "#number-selector > div#selected > a#send")]
        private IWebElement _addToCartButton;

        [FindsBy(How = How.CssSelector, Using = "#selected > div > strong.price")]
        private IWebElement _totalPrice;

        [FindsBy(How = How.CssSelector, Using = "#games >ul > li:nth-child(2) > a")]
        private IWebElement _groupTab;

        /// <summary>
        /// Switches from Single to Groups tab
        /// </summary>
        public void SwitchToGroupTab()
        {
            _groupTab.Click();
            WaitjQuery();
        }


        /// <summary>
        /// Clicks "Add To Cart" button (green one)
        /// </summary>
        public void AddToCart()
        {
            _addToCartButton.Click();
            WaitForPageLoading();
            WaitjQuery();
        }
    }
}
