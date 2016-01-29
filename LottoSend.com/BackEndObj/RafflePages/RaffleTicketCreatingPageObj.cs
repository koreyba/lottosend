using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


namespace LottoSend.com.BackEndObj.RafflePages
{
    /// <summary>
    /// PageObject of a raffle ticket creating page (admin/raffles_tickets/new)
    /// </summary>
    public class RaffleTicketCreatingPageObj : DriverCover
    {
        public RaffleTicketCreatingPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("admin/raffles_tickets/new"))
            {
                throw new Exception("Sorry, but it must be not raffle ticket creating page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#raffles_ticket_raffle_id")]
        private IWebElement _raffleSelect;

        [FindsBy(How = How.CssSelector, Using = "#raffles_ticket_shares")]
        private IWebElement _shares;

        [FindsBy(How = How.CssSelector, Using = "#raffles_ticket_image")]
        private IWebElement _image;

        [FindsBy(How = How.CssSelector, Using = "#lines + .button")]
        private IWebElement _newLineButton;

        [FindsBy(How = How.CssSelector, Using = "#raffles_ticket_submit_action > input")]
        private IWebElement _createRaffleTicketButton;

        [FindsBy(How = How.CssSelector, Using = "#lines li.input.required.stringish > input")]
        private IList<IWebElement> _commonNumbersFields;

        /// <summary>
        /// Creates inactive raffle ticket for an exact lottery with an exact number of shares
        /// </summary>
        /// <param name="raffleName"></param>
        /// <param name="shares"></param>
        public RaffleDashboardPageObj CreateTicket(string raffleName, int shares)
        {
            ChooseElementInSelect(raffleName, _raffleSelect, SelectBy.Text);
            _shares.SendKeys(shares.ToString(System.Globalization.CultureInfo.InvariantCulture));

            var imgPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Assets\scannedimage2.jpg");
            _image.SendKeys(imgPath);

            _newLineButton.Click();
            bool b = WaitAjax();

            foreach(IWebElement field in _commonNumbersFields)
            {
                field.SendKeys(RandomGenerator.GenerateNumber(0, 20) + "," + RandomGenerator.GenerateNumber(0, 20) + ",4,1,2");
            }

            _createRaffleTicketButton.Click();
            WaitForPageLoading();
            WaitAjax();

            return new RaffleDashboardPageObj(Driver);
        }
    }
}
