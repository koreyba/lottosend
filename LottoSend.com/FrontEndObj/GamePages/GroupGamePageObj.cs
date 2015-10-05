using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using LottoSend.com.FrontEndObj.Common;

namespace LottoSend.com.FrontEndObj.GamePages
{
    /// <summary>
    /// Page object of a group ticket page
    /// </summary>
    public class GroupGamePageObj : DriverCover
    {
        public GroupGamePageObj(IWebDriver driver) : base(driver)
        {
            if(Driver.FindElements(By.CssSelector("#group > div.bet-pane > div.group-header > img")).Count == 0)
            {
                throw new Exception("Sorry, it must be not group game page ");
            }

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
                return (int) span.Text.ParseDouble();
            }
        }

        [FindsBy(How = How.CssSelector, Using = "div.col-sm-7.groups-prices > div.bet-resume > table > tbody > tr:nth-child(3) > td:nth-child(2)")]
        private IWebElement _discount;

        /// <summary>
        /// Gets discount for the order
        /// </summary>
        public double Discount
        {
            get { return _discount.Text.ParseDouble(); }
        }

        [FindsBy(How = How.CssSelector, Using = "div.groups-prices > div.bet-resume > table > tbody > tr:nth-child(4) > td:nth-child(2)")]
        private IWebElement _totalPrice;

        /// <summary>
        /// Gets total price of the order
        /// </summary>
        public double TotalPrice
        {
            get { return _totalPrice.Text.ParseDouble(); }
        }

        /// <summary>
        /// Adds shares to selected ticket (from 1 to 5)
        /// </summary>
        /// <param name="numberOfShares">How many shares to add</param>
        public void AddShares(int numberOfShares)
        {
            IList<IWebElement> tickets = Driver.FindElements(By.CssSelector("div.row > div.col-sm-offset-4.text-center"));

            IWebElement plus = tickets[0].FindElement(By.CssSelector("div.group > div.shadow > a.plus"));

            for (int i = 0; i < numberOfShares; ++i)
            {
                plus.Click();
            }
            
        }

        /// <summary>
        /// Clicks on "One-Time Entry" radiobutton 
        /// </summary>
        public void SelectOneTimeEntryGame()
        {
            GetFirstVisibleElementFromList(By.CssSelector("#bet_order_item_attributes_buy_option_single + label")).Click();
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
            return GetFirstVisibleElementFromList(By.CssSelector("#add-group"));
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
            return GetFirstVisibleElementFromList(By.CssSelector(".btn.btn-warning.play-now"));
        }
    }
}
