using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj
{
    /// <summary>
    /// Page object of back/bulk-buys page admin/bulk_buys
    /// </summary>
    public class BulkBuysPageObj : DriverCover
    {
        public BulkBuysPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("bulk_buys"))
            {
                throw new Exception("Sorry but it must be not Back/Bulk-Buys page, current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = ".index_table > tbody > tr:nth-child(1)")]
        private IWebElement _tableFirstRecord;

        /// <summary>
        /// Returns web user email in the first record
        /// </summary>
        public string WebUser
        {
            get { return _tableFirstRecord.FindElement(By.CssSelector("td:nth-child(3)")).Text; }
        }

        /// <summary>
        /// Returns lottery name in the first record
        /// </summary>
        public string Lottery
        {
            get { return _tableFirstRecord.FindElement(By.CssSelector("td:nth-child(5)")).Text; }
        }

        /// <summary>
        /// Returns amount in the first record
        /// </summary>
        public double Amount
        {
            get { return _tableFirstRecord.FindElement(By.CssSelector("td:nth-child(9)")).Text.ParseDouble(); }
        }

        /// <summary>
        /// Returns draws played in the first record
        /// </summary>
        public int DrawsPlayed
        {
            get { return (int)_tableFirstRecord.FindElement(By.CssSelector("td:nth-child(11)")).Text.ParseDouble(); }
        }

        /// <summary>
        /// Returns draws to play in the first record
        /// </summary>
        public int DrawsToPlay
        {
            get { return (int)_tableFirstRecord.FindElement(By.CssSelector("td:nth-child(12)")).Text.ParseDouble(); }
        }
    }
}
