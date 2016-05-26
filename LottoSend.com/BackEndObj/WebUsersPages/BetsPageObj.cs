using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.WebUsersPages
{
    /// <summary>
    /// Page Object of back/web_users/view bets page
    /// </summary>
    public class BetsPageObj : DriverCover
    {
        public BetsPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("bets"))
            {
                throw new Exception("Sorry it must be no back/web_users/bets page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#index_table_bets >tbody > tr:nth-child(1)")]
        private IWebElement _firstRecord;

        /// <summary>
        /// Gets price (amount) of the bet in the first record
        /// </summary>
        public double Amount
        {
            get { return _firstRecord.FindElement(By.CssSelector("td:nth-child(4)")).Text.ParseDouble(); }
        }

        /// <summary>
        /// Gets type of the bet in the first record
        /// </summary>
        public string Type
        {
            get { return _firstRecord.FindElement(By.CssSelector("td:nth-child(5)")).Text; }
        }

        /// <summary>
        /// Gets lottery name of the first record
        /// </summary>
        public string LotteryName
        {
            get { return _firstRecord.FindElement(By.CssSelector("td:nth-child(1)")).Text; }
        }
    }
}
