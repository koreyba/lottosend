﻿using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.GamePages
{
    /// <summary>
    /// Page Object of Front-Raffles page
    /// </summary>
    public class RafflesPageObj : DriverCover
    {
        public RafflesPageObj(IWebDriver driver) : base(driver)
        {
            if(!Driver.Url.Contains("raffles"))
            {
                throw new Exception("Sorry, it must be not \"Raffles\" page. Please check it ");
            }
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = ".table.raffle tr > td:nth-child(2)")]
        private IList<IWebElement> _sharesLeft;

        [FindsBy(How = How.CssSelector, Using = "table.table > tbody > tr.total > td.text-right")]
        private IWebElement _totalPrice;

        /// <summary>
        /// Gets total price of added raffle tickets
        /// </summary>
        public double TotalPrice
        {
            get { return Convert.ToDouble(StringExtention.ParseDouble(_totalPrice.Text)); }
        }

        [FindsBy(How = How.CssSelector, Using = "input#raffle_submit")]
        private IWebElement _buyNowButton;

        [FindsBy(How = How.CssSelector, Using = "table.table.raffle > tbody")]
        private IWebElement _rafflesTable;

        /// <summary>
        /// Returns number of shares left in a group ticket
        /// </summary>
        /// <param name="ticketNumber">ID of the ticket (1,2,3,4,5 etc)</param>
        /// <returns></returns>
        public int GetNumberOfLeftShares(int ticketNumber)
        {
            return (int)StringExtention.ParseDouble(_sharesLeft[ticketNumber - 1].Text);
        }

        /// <summary>
        /// Adds shares to selected ticket (from 1 to 5)
        /// </summary>
        /// <param name="numberOfShares">How many shares to add</param>
        /// <param name="numberOfTicket">Number of ticket (from 1 to 5)</param>
        public void AddShares(int numberOfShares, int numberOfTicket)
        {
            //Check if numberOfTicket is correct
            if(numberOfShares > 5 || numberOfShares < 1) throw new Exception("numberOfTickets parameter must be within 1-5 but is was: " + numberOfShares + " ");

            IWebElement plus = _rafflesTable.FindElement(By.CssSelector("tr.text-center:nth-child(" + numberOfTicket + ") > td.group > a.plus"));

            for (int i = 0; i < numberOfShares; ++i)
            {
                plus.Click();
            }
            
        }

        /// <summary>
        /// Clicks on "Buy Now" button
        /// </summary>
        public void ClickBuyNowButton()
        {
            _buyNowButton.Click();
            WaitjQuery();
            WaitForPageLoading();

            //return new CartObj(Driver);
        }
    }
}
