using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LottoSend.com.FrontEndObj.GamePages
{
    public class GroupGamePageObj : DriverCover
    {
        public GroupGamePageObj(IWebDriver driver) : base(driver)
        {
            if(Driver.FindElements(By.CssSelector("#group > div.row.bet-pane > div.row.group-header > img")).Count == 0)
            {
                throw new Exception("Sorry, it must be not group game page ");
            }

            PageFactory.InitElements(Driver, this);
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
