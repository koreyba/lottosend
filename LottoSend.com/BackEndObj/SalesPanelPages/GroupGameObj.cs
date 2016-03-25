using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.SalesPanelPages
{
    /// <summary>
    /// Page Object of a group game page
    /// </summary>
    public class GroupGameObj : DriverCover
    {
        public GroupGameObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "a#send.add-to-cart")]
        private IWebElement _addToCartButton;

        [FindsBy(How = How.CssSelector, Using = "form#bet-group > table > tbody")] 
        private IWebElement _table;

        [FindsBy(How = How.CssSelector, Using = "#games >ul > li:nth-child(1) > a")]
        private IWebElement _singleTab;

        [FindsBy(How = How.CssSelector, Using = "#bet-group > label > #bet_order_item_attributes_buy_option_single")]
        private IWebElement _oneDraw;

        /// <summary>
        /// Selects one-time entry (1 draw)
        /// </summary>
        public void ClickOneDrawRadioButton()
        {
            _oneDraw.Click();
            WaitjQuery();
        }

        /// <summary>
        /// Switches from Groups to Single tab
        /// </summary>
        public void SwitchToSingleTab()
        {
            _singleTab.Click();
            WaitjQuery();
        }

        /// <summary>
        /// Returns number of available shares in the ticket
        /// </summary>
        /// <param name="ticketNumber">number of ticket (from 1)</param>
        /// <returns></returns>
        public int GetNumberOfAvailableShares(int ticketNumber)
        {
            IWebElement ticket = _table.FindElement(By.CssSelector("tr:nth-child(" + ticketNumber + ")"));
            string shares = ticket.FindElement(By.CssSelector("td.group:nth-child(3)")).Text;

            return Convert.ToInt32(shares);
        }

        /// <summary>
        /// Adds number of shares to a ticket
        /// </summary>
        /// <param name="ticketNumber">Number of ticket from 1</param>
        /// <param name="numberOfShares"></param>
        public void AddShareToTicket(int ticketNumber, int numberOfShares)
        {
            IWebElement ticket = _table.FindElement(By.CssSelector("tr:nth-child(" + 3 * ticketNumber + ")")); //TODO: select all and then user foreach
            IWebElement select = ticket.FindElement(By.CssSelector("td:nth-child(6) > select"));
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
