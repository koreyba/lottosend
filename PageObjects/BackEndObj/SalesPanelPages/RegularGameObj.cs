using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj.SalesPanelPages
{
    public class RegularGameObj : DriverCover
    {
        public RegularGameObj(IWebDriver driver) : base(driver)
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

        [FindsBy(How = How.CssSelector, Using = "input[id*=bet_order_item_attributes_bulk]")]
        private IList<IWebElement> _bulkOptions;

        /// <summary>
        /// Switches from Single to Groups tab
        /// </summary>
        public void SwitchToGroupTab()
        {
            _groupTab.Click();
            WaitjQuery();
        }

        /// <summary>
        /// Selects first available multi-draw options. Returns number of draws that was selected
        /// </summary>
        /// <returns>Selected number of draws</returns>
        public int SelectMultiDraw()
        {
            IWebElement bulk = GetFirstVisibleElementFromList(_bulkOptions);
            bulk.Click();

            int numberOfDraws = (int)bulk.FindElement(By.XPath("../small")).Text.ParseDouble();

            return numberOfDraws;
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
