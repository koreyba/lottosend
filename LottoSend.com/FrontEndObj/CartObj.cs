using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using LottoSend.com.FrontEndObj.Common;

namespace LottoSend.com.FrontEndObj
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

        [FindsBy(How = How.CssSelector, Using = "#game div > table > tbody > tr.blue > td.text-center > strong")] 
        private IWebElement _totalPrice_Front;

        [FindsBy(How = How.CssSelector, Using = "#game div > div.total >span > strong")]
        private IWebElement _totalPrice_Mobile;

        [FindsBy(How = How.CssSelector, Using = "#order_item_bulk_id")]
        private IList<IWebElement> _numberOfDrawsSelects;

        [FindsBy(How = How.CssSelector, Using = "table.table-striped > tbody")]
        private IWebElement _cartTable;

        /// <summary>
        /// Gets total price of the order (mobile)
        /// </summary>
        public double TotalPrice_Mobile
        {
            get { return _totalPrice_Mobile.Text.ParseDouble(); }
        }
        
        /// <summary>
        /// Gets total price of the order (web-site)
        /// </summary>
        public double TotalPrice_Front
        {
            get { return _totalPrice_Front.Text.ParseDouble(); }
        }

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
        public void EditTicket_Web(string lottery)
        {
            IWebElement tr = _findTrOfTicket_Web(lottery);
            tr.FindElement(By.CssSelector("td.text-center > a.btn.btn-info.btn-block.edit")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Edits selected ticket (clicks "Edit" button)
        /// </summary>
        /// <param name="lottery"></param>
        public void EditTicket_Mobile(string lottery)
        {
            IWebElement tr = _findTrOfTicket_Mobile(lottery);
            ScrollUp();
            tr.FindElement(By.CssSelector("td > p > a.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Edits selected ticket (clicks "Edit" button)
        /// </summary>
        /// <param name="lottery"></param>
        public void EditRaffleTicketMobile(string lottery)
        {
            IWebElement tr = _findTrOfRaffleTicket_Mobile(lottery);
            tr.FindElement(By.CssSelector("td > p > a.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Removes ticket from the cart found by name of a lottery
        /// </summary>
        /// <param name="lottery"></param>
        public void DeleteTicket_Web(string lottery)
        {
            IWebElement tr = _findTrOfTicket_Web(lottery);
            tr.FindElement(By.CssSelector("td.text-center > form > input.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading(); 
            WaitAjax();
        }

        public void DeleteTicket_Mobile(string lottery)
        {
            IWebElement tr = _findTrOfTicket_Mobile(lottery);
            tr.FindElement(By.CssSelector("td > form > input.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        public void DeleteRaffleTicket_Mobile(string lottery)
        {
            IWebElement tr = _findTrOfRaffleTicket_Mobile(lottery);
            tr.FindElement(By.CssSelector("td > form > input.btn.btn-info.btn-block")).Click();
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


        private IWebElement _findTrOfTicket_Web(string lottery)
        {
            IList<IWebElement> tr = _cartTable.FindElements(By.TagName("tr"));

            foreach(IWebElement t in tr)
            {
                IList<IWebElement> namesOfLottery = t.FindElements(By.CssSelector("td.text-center > img"));

                foreach (IWebElement name in namesOfLottery)
                {
                    if (name.GetAttribute("alt").ToLower().Equals(lottery.ToLower()))
                        return t;
                }
            }

            return null;
        }

        private IWebElement _findTrOfTicket_Mobile(string lottery)
        {
            IList<IWebElement> tr = _cartTable.FindElements(By.TagName("tr"));

            foreach (IWebElement t in tr)
            {
                IList<IWebElement> namesOfLottery = t.FindElements(By.CssSelector("td > strong"));

                foreach (IWebElement name in namesOfLottery)
                {
                    if (name.Text.ToLower().Contains(lottery.ToLower()))
                        return t;
                }
            }

            return null;
        }

        private IWebElement _findTrOfRaffleTicket_Mobile(string lottery)
        {
            IList<IWebElement> tr = _cartTable.FindElements(By.TagName("tr"));

            foreach (IWebElement t in tr)
            {
                IList<IWebElement> namesOfLottery = t.FindElements(By.CssSelector("td > strong > img"));

                foreach (IWebElement name in namesOfLottery)
                {
                    if (name.GetAttribute("alt").Contains(lottery))
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
