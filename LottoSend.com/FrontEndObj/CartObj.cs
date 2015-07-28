﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoSend.com.FrontEndObj
{
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

        [FindsBy(How = How.CssSelector, Using = "table.table.cart.table-striped > tbody")]
        private IWebElement _cartTable;

        /// <summary>
        /// Returns number of tickets in the cart
        /// </summary>
        public int NumberOfTickets
        {
            get { return _counNumberOfTickets(); }
        }

        private int _counNumberOfTickets()
        {
            return _cartTable.FindElements(By.TagName("tr")).Count; 
        }

        /// <summary>
        /// Search for a lottery ticket in the cart
        /// </summary>
        /// <param name="lottery">Name of a lottery</param>
        /// <returns>true/false</returns>
        public bool IfTicketIsAdded(string lottery)
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
