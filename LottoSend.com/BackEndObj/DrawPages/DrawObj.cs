using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.DrawPages
{
    /// <summary>
    /// Page object of a draw page
    /// </summary>
    public class DrawObj : DriverCover
    {
        public DrawObj(IWebDriver driver) : base(driver)
        {
            if(!Driver.Url.Contains("bets") || !Driver.Url.Contains("draws"))
            {
                throw new Exception("Sorry, it's not a draw page ");
            }

            PageFactory.InitElements(Driver, this);
        }

        /// <summary>
        /// Returns value from "Bet Amount" column (price of the ticket) 
        /// </summary>
        public double BetAmount
        {
            get { return _firstRecord.FindElement(By.CssSelector("td:nth-child(7)")).Text.ParseDouble(); }
        }

        /// <summary>
        /// Gets <tr> tag of the first record
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "table.index_table > tbody > tr:nth-child(2)")]
        private IWebElement _firstRecord;

        /// <summary>
        /// Returns Type of bet from the first record
        /// </summary>
        public string Type
        {
            get { return _firstRecord.FindElement(By.CssSelector("td:nth-child(10)")).Text; }
        }

        /// <summary>
        /// Returns email of the first record
        /// </summary>
        /// <returns></returns>
        public string GetEmail()
        {
            IWebElement a = _firstRecord.FindElement(By.CssSelector("td:nth-child(6) > a"));

            return a.Text;
        }

        /// <summary>
        /// Returns the date of the last (the first at the top) transaction
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTransactionDate()
        {
            IWebElement dateTD = _firstRecord.FindElement(By.CssSelector("td:nth-child(3)"));

            string date = dateTD.Text;
            TimeSpan timeSpan = date.ParseTimeSpan();

            return timeSpan;
        }

        /// <summary>
        /// Returns UTC date that is displayed in admin panel
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetUtcDate()
        {
            PaneMenuObj panel = new PaneMenuObj(Driver);
            string utcData = panel.GetUTCDate();

            TimeSpan timeSpan = utcData.ParseTimeSpan();

            return timeSpan;
        }
    }
}
