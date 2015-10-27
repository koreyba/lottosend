using System.Threading;
using LottoSend.com.FrontEndObj.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj.GamePages
{
    /// <summary>
    /// Page Object of a regular ticket game page
    /// </summary>
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
                return (int)span.Text.ParseDouble();
            }
        }

        [FindsBy(How = How.CssSelector, Using = "")]
        private IWebElement _discount;

        /// <summary>
        /// Gets discount for the order
        /// </summary>
        public double Discount
        {
            get { return _discount.Text.ParseDouble(); }
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
                //First find a visible table and then search price in this table
                IWebElement table = GetFirstVisibleElementFromList(By.CssSelector("div.groups-prices > div.bet-resume > table.table"));
                IWebElement price = table.FindElement(By.CssSelector("tbody > tr:nth-child(3) > td:nth-child(2)"));
                return price.Text.ParseDouble(); 
            }
        }

        [FindsBy(How = How.CssSelector, Using = "#single-tab > a")]
        private IWebElement _singleGame;

        /// <summary>
        /// Click on "Single game" tab
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
            Thread.Sleep(200);
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
