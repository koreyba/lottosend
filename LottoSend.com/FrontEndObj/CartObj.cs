using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using LottoSend.com.FrontEndObj.GamePages;

namespace LottoSend.com.FrontEndObj
{
    /// <summary>
    /// Page Object of the cart page (front)
    /// </summary>
    public class CartObj : DriverCover
    {
        public CartObj(IWebDriver driver) : base(driver)
        {
            if (Driver.FindElements(By.CssSelector("table.table.cart.table-striped")).Count <= 0)
            {
                throw new Exception("It's not the cart page");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "div.control > a.btn.btn-lg.btn-success.btn-xl.btn-block")]
        private IWebElement _proceedToCheckoutButton;

        /// <summary>
        /// Clicks on Proceed to Checkout button
        /// </summary>
        /// <returns></returns>
        public MerchantsObj ClickProceedToCheckoutButton()
        {
            _proceedToCheckoutButton.Click();
            WaitjQuery();

            return new MerchantsObj(Driver);
        }

        [FindsBy(How = How.CssSelector, Using = "table.table.cart.table-striped > tbody")]
        private IWebElement _cartTable;

        /// <summary>
        /// Returns number of tickets in the cart
        /// </summary>
        public int NumberOfTickets
        {
            get { return CounNumberOfTickets(); }
        }

        /// <summary>
        /// Returns number of group tickets in the cart
        /// </summary>
        public int NumberOfGroupTickets
        {
            get { return CountNumberOfGroupTickets(); }
        }

        /// <summary>
        /// Returns number of group tickets in the cart
        /// </summary>
        /// <returns></returns>
        public int CountNumberOfGroupTickets()
        {
            return _cartTable.FindElements(By.CssSelector("a.group-lines")).Count;
        }

        /// <summary>
        /// Removes ticket from the cart found by name of a lottery
        /// </summary>
        /// <param name="lottery"></param>
        public void DeleteTicket(string lottery)
        {
            IWebElement tr = _findTrOfTicket(lottery);
            tr.FindElement(By.CssSelector("td.text-center > form > div > input.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Returns number of all ticket in the cart
        /// </summary>
        /// <returns>number of tickets</returns>
        public int CounNumberOfTickets()
        {
            return _cartTable.FindElements(By.TagName("tr")).Count; 
        }

        private IWebElement _findTrOfTicket(string lottery)
        {
            IList<IWebElement> tr = _cartTable.FindElements(By.TagName("tr"));

            foreach(IWebElement t in tr)
            {
                IList<IWebElement> namesOfLottery = t.FindElements(By.CssSelector("td.text-center > img"));

                foreach (IWebElement name in namesOfLottery)
                {
                    if (name.GetAttribute("alt").Equals(lottery))
                        return t;
                }
            }

            return null;
        }

        /// <summary>
        /// Search for a lottery ticket in the cart
        /// </summary>
        /// <param name="lottery">Name of a lottery</param>
        /// <returns>true/false</returns>
        public bool IsTicketInCart(string lottery)
        {
            IList<IWebElement> namesOfLottery =  _cartTable.FindElements(By.CssSelector("td.text-center > img"));
            bool ticket = false;

            foreach(IWebElement name in namesOfLottery)
            {
                if (name.GetAttribute("alt").Equals(lottery))
                    ticket = true; break;
            }

            return ticket; 
        }
    }
}
