using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LottoSend.com.FrontEndObj
{
    public class RegularGamePageObj : DriverCover
    {
        public RegularGamePageObj(IWebDriver driver) : base(driver)
        {
            //TODO make validation

            PageFactory.InitElements(Driver, this);
        }

        /// <summary>
        /// Gets selected amount of draws to play
        /// </summary>
        public int NumberOfDraws
        {
            get
            {
                IWebElement span = GetFirstVisibleElementFromList(By.CssSelector(".filter-option.pull-left"));
                return (int)span.Text.ParceDouble();
            }
        }

        [FindsBy(How = How.CssSelector, Using = "")]
        private IWebElement _discount;

        /// <summary>
        /// Gets discount for the order
        /// </summary>
        public double Discount
        {
            get { return _discount.Text.ParceDouble(); }
        }

        [FindsBy(How = How.CssSelector, Using = "div.col-sm-7.groups-prices > div.bet-resume > table:nth-child(2) > tbody > tr:nth-child(3) > td:nth-child(2)")]
        private IWebElement _totalPrice;

        /// <summary>
        /// Gets total price of the order
        /// </summary>
        public double TotalPrice
        {
            get 
            {
                IWebElement price = GetFirstVisibleElementFromList(By.CssSelector("div.col-sm-7.groups-prices > div.bet-resume > table:nth-child(1) > tbody > tr:nth-child(3) > td:nth-child(2)"));
                return price.Text.ParceDouble(); 
            }
        }

        [FindsBy(How = How.CssSelector, Using = "ul.nav.nav-tabs > li:nth-child(2) > a")]
        private IWebElement _singleGame;

        /// <summary>
        /// Click on "Standart game" button
        /// </summary>
        public void ClickStandartGameButton()
        {
            _singleGame.Click();

            WaitAjax();
            WaitjQuery();
        }

        /// <summary>
        /// Clicks on "One-Time Entry" radiobutton 
        /// </summary>
        public void SelectOneTimeEntryGame()
        {
            GetFirstVisibleElementFromList(By.CssSelector("#bet_order_item_attributes_buy_option_single + label")).Click();
        }

        /// <summary>
        /// Click "Buy Tickets" button
        /// </summary>
        /// <returns></returns>
        public MerchantsObj ClickBuyTicketsButton()
        {
            Thread.Sleep(100);
            _detectBuyTicketsButton().Click();
            bool w = WaitjQuery();

            return new MerchantsObj(Driver);
        }

        /// <summary>
        /// Returns displayed button "Buy Tickets" or null if the button was not found
        /// </summary>
        /// <returns>Button/null</returns>
        private IWebElement _detectBuyTicketsButton()
        {
            return GetFirstVisibleElementFromList(By.CssSelector("button.btn.btn-warning.play-now"));
        }

        /// <summary>
        /// Clicks on "Add to Cart and Play More Games"
        /// </summary>
        public void ClickAddToCartButton()
        {
            Thread.Sleep(100);
            _detectAddToCartButton().Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Returns displayed button "Add To Cart" or null if the button was not found
        /// </summary>
        /// <returns>Button/null</returns>
        private IWebElement _detectAddToCartButton()
        {
            return GetFirstVisibleElementFromList(By.CssSelector("#add-to-cart"));
        }        
    }
}
