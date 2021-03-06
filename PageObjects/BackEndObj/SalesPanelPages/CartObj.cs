﻿using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj.SalesPanelPages
{
    public class CartObj : TabsObj
    {
        public CartObj(IWebDriver driver) : base(driver)
        {
            //TODO: make validation

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "a#internal-balance")]
        private IWebElement _payWithInternalBalance;

        [FindsBy(How = How.CssSelector, Using = "a#charge")]
        private IWebElement _charge;

        [FindsBy(How = How.CssSelector, Using = "span.internal-balance + div.footer > strong")]
        private IWebElement _total;

        [FindsBy(How = How.CssSelector, Using = "#cart > span.internal-balance")]
        private IWebElement _internalBalace;

        [FindsBy(How = How.CssSelector, Using = "input#code")]
        private IWebElement _couponCode;

        [FindsBy(How = How.CssSelector, Using = "input#code + input")]
        private IWebElement _apply;

        [FindsBy(How = How.CssSelector, Using = "#cart > div.order_item")]
        private IList<IWebElement> _ticktes;

        [FindsBy(How = How.CssSelector, Using = "#cart > a.button")]
        private IWebElement _cancel;


        /// <summary>
        /// Returns number of tickets in the cart
        /// </summary>
        public int NumberOfTickets
        {
            get { return _ticktes.Count; }
        }

        /// <summary>
        /// Returns InternalBalance in the cart
        /// </summary>
        public double InternalBalance
        {
            get { return _internalBalace.Text.ParseDouble(); }
        }

        /// <summary>
        /// Returns total price in the cart
        /// </summary>
        public double TotalPrice
        {
            get { return _total.Text.ParseDouble(); }
        }

        /// <summary>
        /// Removes ticket from the cart found by name of a lottery
        /// </summary>
        /// <param name="lottery"></param>
        public void DeleteTicket(string lottery)
        {
            IWebElement tr = _findDivOfTicket(lottery);
            tr.FindElement(By.CssSelector("div.actions > form > input.button")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Finds HTML tag div of the ticket
        /// </summary>
        /// <param name="lottery"></param>
        /// <returns></returns>
        private IWebElement _findDivOfTicket(string lottery)
        {
            foreach (IWebElement t in _ticktes)
            {

                if (t.Text.Contains(lottery))
                    return t;
            }

            return null;
        }

        /// <summary>
        /// Pays with internal balance (click button)
        /// </summary>
        public void PayWithInternalBalance()
        {
            _payWithInternalBalance.Click();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Charges user with credit card (must be previously inputed), clicks button
        /// </summary>
        public void Charge()
        {
            WaitjQuery();
            _charge.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// Enters and applies a coupon
        /// </summary>
        /// <param name="code"></param>
        public void ApplyCoupon(string code)
        {
            _couponCode.SendKeys(code);
            _apply.Click();
            WaitAjax();
            WaitForPageLoading();
        }

        /// <summary>
        /// Removes applied coupon
        /// </summary>
        public void CancelCoupon()
        {
            _cancel.Click();
            WaitAjax();
            WaitForPageLoading();
        }

        /// <summary>
        /// Removes all ticket from the cart (clears the cart)
        /// </summary>
        public void DeleteAllTickets()
        {
            while (Driver.FindElements(By.CssSelector("div.actions > form > input.button")).Count != 0)
            {
                _ticktes[0].FindElement(By.CssSelector("div.actions > form > input.button")).Click();
                WaitAjax();
                WaitForPageLoading();
                WaitAjax();
            }


            //for (int i = 0; i <= NumberOfTickets; ++i)
            //{
            //    IWebElement el = _ticktes[i].FindElement(By.CssSelector("div.actions > form > input.button"));
            //    el.Click();
            //    WaitAjax();
            //    WaitForPageLoading();
            //    WaitAjax();
            //}
        } 
    }
}
