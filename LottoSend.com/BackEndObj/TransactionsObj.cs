using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoSend.com.BackEndObj
{
    /// <summary>
    /// Page Object of BackOffice/Transactions page
    /// </summary>
    public class TransactionsObj : DriverCover
    {
        public TransactionsObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("transactions"))
            {
                throw new Exception("It's not Transaction page ");
            }
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "table.index_table > tbody > tr:nth-child(2)")]
        private IWebElement _firstRecord;

        /// <summary>
        /// Returns email of the first transaction
        /// </summary>
        /// <returns></returns>
        public string GetTransactionEmail()
        {
            IWebElement emailTD = _firstRecord.FindElement(By.CssSelector("td:nth-child(3)"));
            return emailTD.Text;
        }

        /// <summary>
        /// Returns name of the merchant in transaction
        /// </summary>
        /// <returns></returns>
        public string GetTransactionMerchant()
        {
            IWebElement merchantTD = _firstRecord.FindElement(By.CssSelector("td:nth-child(4)"));
            return merchantTD.Text;
        }

        /// <summary>
        /// Returns the date of the last (the first at the top) transaction
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTransactionDate()
        {
            IWebElement dateTD = _firstRecord.FindElement(By.CssSelector("td:nth-child(12)"));

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
