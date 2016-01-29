using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.RafflePages
{
    /// <summary>
    /// Page Object for backoffice/Raffle-Dashboard page
    /// </summary>
    public class RaffleDashboardPageObj : DriverCover
    {
        public RaffleDashboardPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("admin/raffles"))
            {
                throw new Exception("Sorry but it must be not Back/Raffle-Dashboard page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#selected_raffle")]
        private IWebElement _selectRaffleSelector;

        [FindsBy(How = How.CssSelector, Using = ".index_as_table > .index_table:nth-child(4) > thead")]
        private IWebElement _tableOfTickets;

        [FindsBy(How = How.LinkText, Using = "Add New Raffle Ticket")]
        private IWebElement _addNewRaffleTicketButton;

        /// <summary>
        /// Gets number of created tickets for a selected raffle
        /// </summary>
        public int NumberOfTickets
        {
            get { return _tableOfTickets.FindElements(By.CssSelector("tr")).Count - 1; }
        }

        /// <summary>
        /// Clicks on Add New Raffle Ticket button
        /// </summary>
        /// <returns></returns>
        public RaffleTicketCreatingPageObj ClickAddNewTicketButton()
        {
            _addNewRaffleTicketButton.Click();
            WaitForPageLoading();
            WaitAjax();

            return new RaffleTicketCreatingPageObj(Driver);
        }

        /// <summary>
        /// Selects a raffle in "Select Raffle" selector
        /// </summary>
        /// <param name="raffleName"></param>
        public void SelectRaffle(string raffleName)
        {
            ChooseElementInSelect(raffleName, _selectRaffleSelector, SelectBy.Text);
            WaitForPageLoading();
        }
    }
}
