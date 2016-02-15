using System;
using System.Collections.Generic;
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

        private string _modalBulkPopUP = ".delete-bet-modal.bet-numbers-box";
        private string _modalSinglePopUp = ".delete-bet-email-modal.bet-numbers-box";

        /// <summary>
        /// Gets <tr> tag of the first record
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "table.index_table > tbody > tr:nth-child(2)")]
        private IWebElement _firstRecord;

        [FindsBy(How = How.CssSelector, Using = ".index_table tr > td:nth-child(21)  > a.button")]
        private IList<IWebElement> _delete; 

        /// <summary>
        /// Returns value from "Bet Amount" column (price of the ticket) 
        /// </summary>
        public double BetAmount
        {
            get { return _firstRecord.FindElement(By.CssSelector("td:nth-child(8)")).Text.ParseDouble(); }
        }

        /// <summary>
        /// Returns Type of bet from the first record
        /// </summary>
        public string Type
        {
            get { return _firstRecord.FindElement(By.CssSelector("td:nth-child(11)")).Text; }
        }

        /// <summary>
        /// Removes a specific bet
        /// </summary>
        /// <param name="betNumber"></param>
        /// <param name="isBulk">if bet is bulk one or not</param>
        public void DeleteBet(int betNumber, bool isBulk)
        {
            _delete[betNumber - 1].Click();
            WaitAjax();

            if (isBulk)
            {
                IWebElement modalPopUp = GetFirstVisibleElementFromList(By.CssSelector(_modalBulkPopUP));
                modalPopUp.FindElement(By.LinkText("Bet")).Click();
                AcceptAlert();
            }
            else
            {
                IWebElement modalPopUp = GetFirstVisibleElementFromList(By.CssSelector(_modalSinglePopUp));
                modalPopUp.FindElement(By.LinkText("Yes")).Click();
                AcceptAlert();
            }
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
    }
}
