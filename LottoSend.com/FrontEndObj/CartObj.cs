using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
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

        [FindsBy(How = How.CssSelector, Using = "table.table-striped > tbody")]
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
        /// Removes all ticket from the cart (clears the cart)
        /// </summary>
        public void DeleteAllTickets()
        {
            int n = NumberOfTickets;
            for (int i = 0; i < n; ++i)
            {
                _cartTable.FindElement(By.CssSelector("tr > td > form > div > input.btn.btn-info.btn-block")).Click();
                WaitAjax();
                WaitForPageLoading();
                WaitAjax();
            }  
        }

        /// <summary>
        /// Edits selected ticket (clicks "Edit" button)
        /// </summary>
        /// <param name="lottery"></param>
        public void EditTicketWeb(string lottery)
        {
            IWebElement tr = _findTrOfTicketWeb(lottery);
            tr.FindElement(By.CssSelector("td.text-center > a.btn.btn-info.btn-block.edit")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Edits selected ticket (clicks "Edit" button)
        /// </summary>
        /// <param name="lottery"></param>
        public void EditTicketMobile(string lottery)
        {
            IWebElement tr = _findTrOfTicketMobile(lottery);
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
            IWebElement tr = _findTrOfRaffleTicketMobile(lottery);
            tr.FindElement(By.CssSelector("td > p > a.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Removes ticket from the cart found by name of a lottery
        /// </summary>
        /// <param name="lottery"></param>
        public void DeleteTicketWeb(string lottery)
        {
            IWebElement tr = _findTrOfTicketWeb(lottery);
            tr.FindElement(By.CssSelector("td.text-center > form > div > input.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading(); 
            WaitAjax();
        }

        public void DeleteTicketMobile(string lottery)
        {
            IWebElement tr = _findTrOfTicketMobile(lottery);
            tr.FindElement(By.CssSelector("td > form > div > input.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        public void DeleteRaffleTicketMobile(string lottery)
        {
            IWebElement tr = _findTrOfRaffleTicketMobile(lottery);
            tr.FindElement(By.CssSelector("td > form > div > input.btn.btn-info.btn-block")).Click();
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


        private IWebElement _findTrOfTicketWeb(string lottery)
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

        private IWebElement _findTrOfTicketMobile(string lottery)
        {
            IList<IWebElement> tr = _cartTable.FindElements(By.TagName("tr"));

            foreach (IWebElement t in tr)
            {
                IList<IWebElement> namesOfLottery = t.FindElements(By.CssSelector("td > strong"));

                foreach (IWebElement name in namesOfLottery)
                {
                    if (name.Text.Contains(lottery))
                        return t;
                }
            }

            return null;
        }

        private IWebElement _findTrOfRaffleTicketMobile(string lottery)
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
