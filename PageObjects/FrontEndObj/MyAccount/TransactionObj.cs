using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.MyAccount
{
    /// <summary>
    /// Front end - my account - transaction history page
    /// </summary>
    public class TransactionObj : DriverCover 
    {
        public TransactionObj(IWebDriver driver) : base(driver)
        {
            if(Driver.FindElements(By.Id("account-detail")).Count != 1)
            {
                throw new Exception("It's not Transaction History page ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "table.table > tbody > tr.even.text-center:nth-child(2)")]
        private IWebElement _secondRecord;

        [FindsBy(How = How.CssSelector, Using = "table.table > tbody > tr.odd.text-center:nth-child(1)")]
        private IWebElement _firstRecord;

        /// <summary>
        /// Returns amount (of money) in the first record
        /// </summary>
        public double FirstRecordAmount
        {
            get
            {
                return Convert.ToDouble(StringExtention.ParseDouble(_firstRecord.FindElement(By.CssSelector("td:nth-child(3)")).Text));
            }
        }

        /// <summary>
        /// Returns amount (of money) in the second record
        /// </summary>
        public double SecondRecordAmount
        {
            get
            {
                return Convert.ToDouble(StringExtention.ParseDouble(_secondRecord.FindElement(By.CssSelector("td:nth-child(3)")).Text));
            }
        }

        /// <summary>
        /// Returns type of the first record
        /// </summary>
        public string SecondRecordType
        {
            get { return _secondRecord.FindElement(By.CssSelector("td:nth-child(2)")).Text; }
        }

        /// <summary>
        /// Returns type of the first record
        /// </summary>
        public string FirstRecordType
        {
            get { return _firstRecord.FindElement((By.CssSelector("td:nth-child(2)"))).Text; }
        }

        /// <summary>
        /// Returns Data of the second record
        /// </summary>
        public string SecondRecordDate
        {
            get { return _secondRecord.FindElement(By.CssSelector("td:nth-child(1)")).Text; }
        }

        /// <summary>
        /// Returns Data of the first record
        /// </summary>
        public string FirstRecordDate
        {
            get { return _firstRecord.FindElement(By.CssSelector("td:nth-child(1)")).Text; }
        }

        /// <summary>
        /// Returns Lottery of the second record
        /// </summary>
        public string SecondRecordLottery
        {
            get { return _secondRecord.FindElement(By.CssSelector("td:nth-child(4)")).Text; }
        }

        /// <summary>
        /// Returns Lottery of the first record
        /// </summary>
        public string FirstRecordLottery
        {
            get { return _firstRecord.FindElement(By.CssSelector("td:nth-child(4)")).Text; }
        }
    }
}
