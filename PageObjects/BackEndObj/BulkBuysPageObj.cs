using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj
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

        [FindsBy(How = How.CssSelector, Using = ".index_table > tbody > tr")]
        private IList<IWebElement> _records;

        [FindsBy(How = How.CssSelector, Using = ".not_completed_not_deleted .count")]
        private IWebElement _numberOfElements;

        [FindsBy(How = How.CssSelector, Using = ".last > a")]
        private IWebElement _lastButton;


        public int NumberOfPages
        {
            get
            {
                Regex re = new Regex(@"\d+");
                Match m = re.Match(_lastButton.GetAttribute("href"));
                return (int) m.Value.ParseDouble();
            }
        }

        public int NumberOfRecordsOnPage
        {
            get { return _records.Count; }
        }

        /// <summary>
        /// Returns draws date of a specific record on current page
        /// </summary>
        /// <param name="recordNumber"></param>
        /// <returns></returns>
        public DateTime DrawDate (int recordNumber)
        {
           return  DateTime.ParseExact(_records[recordNumber - 1].FindElement(By.CssSelector("tr > td:nth-child(6)")).Text.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns web user email in the first record
        /// </summary>
        public string WebUser (int recordNumber)
        {
            return _records[recordNumber - 1].FindElement(By.CssSelector("tr > td:nth-child(3)")).Text; 
        }

        /// <summary>
        /// Returns lottery name in the first record
        /// </summary>
        public string Lottery (int recordNumber)
        {
            return _records[recordNumber - 1].FindElement(By.CssSelector("tr > td:nth-child(5)")).Text; 
        }

        /// <summary>
        /// Returns amount in the first record
        /// </summary>
        public double Amount (int recordNumber)
        {
            return _records[recordNumber - 1].FindElement(By.CssSelector("tr > td:nth-child(9)")).Text.ParseDouble(); 
        }

        /// <summary>
        /// Returns draws played in the first record
        /// </summary>
        public int DrawsPlayed (int recordNumber)
        {
            return (int)_records[recordNumber - 1].FindElement(By.CssSelector("tr > td:nth-child(11)")).Text.ParseDouble();
        }

        /// <summary>
        /// Returns draws to play in the first record
        /// </summary>
        public int DrawsToPlay(int recordNumber)
        {
            return (int)_records[recordNumber - 1].FindElement(By.CssSelector("tr > td:nth-child(12)")).Text.ParseDouble(); 
        }

        /// <summary>
        /// Click to the "count" page in pagination (only if the page is visible)
        /// </summary>
        public void NavigateToPage(int index)
        {
            NavigateToUrl(BaseAdminUrl + "admin/bulk_buys?order=id_desc&page=" + index + "&scope=not_completed_not_deleted");
        }
    }
}
