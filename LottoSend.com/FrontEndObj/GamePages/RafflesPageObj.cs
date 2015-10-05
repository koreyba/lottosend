using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;

namespace LottoSend.com.FrontEndObj.GamePages
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

        [FindsBy(How = How.CssSelector, Using = "table.table.resume > tbody > tr.total > td.text-right")]
        private IWebElement _totalPrice;

        /// <summary>
        /// Gets total price of added raffle tickets
        /// </summary>
        public double TotalPrice
        {
            get { return Convert.ToDouble(_totalPrice.Text.ParseDouble()); }
        }

        [FindsBy(How = How.CssSelector, Using = "input#raffle_submit")]
        private IWebElement _buyNowButton;

        [FindsBy(How = How.CssSelector, Using = "table.table.raffle > tbody")]
        private IWebElement _rafflesTable;

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
        public CartObj ClickBuyNowButton()
        {
            _buyNowButton.Click();
            WaitjQuery();
            WaitForPageLoading();

            return new CartObj(Driver);
        }
    }
}
