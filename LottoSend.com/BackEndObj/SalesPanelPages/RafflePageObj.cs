using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.SalesPanelPages
{
    /// <summary>
    /// Raffle page object in the sales panel
    /// </summary>
    public class RafflePageObj : DriverCover
    {
        public RafflePageObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "a#send.add-to-cart")]
        private IWebElement _addToCartButton;

        [FindsBy(How = How.CssSelector, Using = "#bet-raffle > table > tbody > tr.total > td:nth-child(2)")]
        private IWebElement _totalPrice;

        [FindsBy(How = How.CssSelector, Using = "#bet-raffle > table > tbody > tr.subtotal > td:nth-child(2)")]
        private IWebElement _subTotalPrice;

        [FindsBy(How = How.CssSelector, Using = "#bet-raffle > table > tbody > tr.discount > td:nth-child(2)")]
        private IWebElement _discount;

        [FindsBy(How = How.CssSelector, Using = "table.raffle.table > tbody")]
        private IWebElement _raffleTable;

        public double Discount
        {
            get { return _discount.Text.ParseDouble(); }
        }

        public double Total
        {
            get { return _totalPrice.Text.ParseDouble(); }
        }

        public double SubTotal
        {
            get { return _subTotalPrice.Text.ParseDouble(); }
        }

        /// <summary>
        /// Returns number of available shares in the ticket
        /// </summary>
        /// <param name="ticketNumber">number of ticket (from 1)</param>
        /// <returns></returns>
        public int GetNumberOfAvailableShares(int ticketNumber)
        {
            IWebElement ticket = _raffleTable.FindElement(By.CssSelector("tr:nth-child(" + ticketNumber + ")"));
            string shares = ticket.FindElement(By.CssSelector("td.group:nth-child(2)")).Text;

            return Convert.ToInt32(shares);
        }

        /// <summary>
        /// Adds number of shares to a ticket
        /// </summary>
        /// <param name="ticketNumber">Number of ticket from 1</param>
        /// <param name="numberOfShares"></param>
        public void AddShareToTicket(int ticketNumber, int numberOfShares)
        {
            IWebElement ticket = _raffleTable.FindElement(By.CssSelector("tr:nth-child(" + ticketNumber + ")"));
            IWebElement select = ticket.FindElement(By.CssSelector("td.group > select"));
            ChooseElementInSelect(numberOfShares.ToString(System.Globalization.CultureInfo.InvariantCulture), select, SelectBy.Value);
            WaitjQuery();
        }

        /// <summary>
        /// Clicks on "Add to cart" (green) button
        /// </summary>
        public void ClickAddToCartButton()
        {
            _addToCartButton.Click();
            WaitForPageLoading();
            WaitjQuery();
        }
    }
}
