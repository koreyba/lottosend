using System;
using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestFramework.FrontEndObj.Common;

namespace TestFramework.FrontEndObj.Cart
{
    /// <summary>
    /// Page Object of the cart page (front)
    /// </summary>
    public class CartObj : DriverCover
    {
        public CartObj(IWebDriver driver) : base(driver)
        {
            if (Driver.FindElements(By.CssSelector("table.table.table-striped")).Count <= 0)
            {
                throw new Exception("It's not the cart page");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "div.control > a.btn.btn-lg.btn-success.btn-xl.btn-block")]
        private IWebElement _proceedToCheckoutButton;

        [FindsBy(How = How.CssSelector, Using = "#order_item_bulk_id")]
        private IList<IWebElement> _numberOfDrawsSelects;

        [FindsBy(How = How.CssSelector, Using = "table.table-striped > tbody")]
        protected IWebElement _cartTable;

        /// <summary>
        /// Gets total price of the order (mobile)
        /// </summary>
        public virtual double TotalPrice { get; }


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

        /// <summary>
        /// Returns number of tickets in the cart
        /// </summary>
        public virtual int NumberOfTickets
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
        /// Changes number of draws to play for a ticket in the cart (changes value in select)
        /// </summary>
        /// <param name="ticketNumber">Number of the ticket in the cart (from 1)</param>
        /// <param name="drawsToPlay">Number of draws to play</param>
        public void ChangeNumberOfDraws(int ticketNumber, int drawsToPlay)
        {
            IWebElement selector = _numberOfDrawsSelects[ticketNumber - 1];

            ChooseElementInSelect(drawsToPlay.ToString(CultureInfo.InvariantCulture), selector, SelectBy.Text);
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
        /// Removes all ticket from the cart (clears the cart)
        /// </summary>
        public void DeleteAllTickets()
        {
            int n = NumberOfTickets;
            for (int i = 0; i < n; ++i)
            {
                _cartTable.FindElement(By.CssSelector("tr > td > form > input.btn.btn-info.btn-block")).Click();
                WaitAjax();
                WaitForPageLoading();
                WaitAjax();
            }  
        }

        /// <summary>
        /// Edits selected ticket (clicks "Edit" button)
        /// </summary>
        /// <param name="lottery"></param>
        public virtual void EditTicket(string lottery)
        {

        }

        /// <summary>
        /// Removes ticket from the cart found by name of a lottery
        /// </summary>
        /// <param name="lottery"></param>
        public virtual void DeleteTicket(string lottery)
        {
     
        }

        public virtual void DeleteRaffleTicket(string lottery)
        {
            
        }

        /// <summary>
        /// Edits selected ticket (clicks "Edit" button)
        /// </summary>
        /// <param name="lottery"></param>
        public virtual void EditRaffleTicketMobile(string lottery)
        {
            
        }

        /// <summary>
        /// Returns number of all ticket in the cart
        /// </summary>
        /// <returns>number of tickets</returns>
        public int CounNumberOfTickets()
        {
            return _cartTable.FindElements(By.TagName("tr")).Count; 
        }

        /// <summary>
        /// Search for a lottery ticket in the cart
        /// </summary>
        /// <param name="lottery">Name of a lottery</param>
        /// <returns>true/false</returns>
        public bool IsTicketInCart(string lottery)
        {
            IList<IWebElement> namesOfLottery =  _cartTable.FindElements(By.CssSelector("tr > td.text-center > img"));
            bool ticket = false;

            foreach(IWebElement name in namesOfLottery)
            {
                if (name.GetAttribute("alt").Equals(lottery))
                {
                    ticket = true;
                    break;
                }
            }

            return ticket; 
        }
    }
}
