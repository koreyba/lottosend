using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;

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

        [FindsBy(How = How.CssSelector, Using = "table.index_table > tbody > tr:nth-child(1)")]
        private IWebElement _firstRecord;

        [FindsBy(How = How.CssSelector, Using = "table.index_table > tbody > tr:nth-child(2)")]
        private IWebElement _secondRecord;

        /// <summary>
        /// Returns transaction type of the first record
        /// </summary>
        public string SecondRecordTransactionType
        {
            get { return _secondRecord.FindElement(By.CssSelector("td:nth-child(5)")).Text; }
        }

        /// <summary>
        /// Returns transaction type of the first record
        /// </summary>
        public string FirstRecordTransactionType
        {
            get { return _firstRecord.FindElement((By.CssSelector("td:nth-child(5)"))).Text; }
        }

        /// <summary>
        /// Returns play type of the first record
        /// </summary>
        public string SecondRecordPlayType
        {
            get { return _secondRecord.FindElement(By.CssSelector("td:nth-child(6)")).Text; }
        }

        /// <summary>
        /// Returns play type of the first record
        /// </summary>
        public string FirstRecordPlayType
        {
            get { return _firstRecord.FindElement((By.CssSelector("td:nth-child(6)"))).Text; }
        }

        /// <summary>
        /// Returns amount (of money) in the first record
        /// </summary>
        public double FirstRecordAmount
        {
            get
            {
                return Convert.ToDouble(_firstRecord.FindElement(By.CssSelector("td:nth-child(8)")).Text.ParseDouble());
            }
        }

        /// <summary>
        /// Returns amount (of money) in the second record
        /// </summary>
        public double SecondRecordAmount
        {
            get
            {
                return Convert.ToDouble(_secondRecord.FindElement(By.CssSelector("td:nth-child(8)")).Text.ParseDouble());
            }
        }

        /// <summary>
        /// Returns state of the first transaction
        /// </summary>
        /// <returns></returns>
        public string GetFirstTransactionState()
        {
            IWebElement status = _firstRecord.FindElement(By.CssSelector("td:nth-child(7)"));
            return status.Text;
        }

        /// <summary>
        /// Returns email of the first transaction
        /// </summary>
        /// <returns></returns>
        public string GetFirstTransactionEmail()
        {
            IWebElement emailTD = _firstRecord.FindElement(By.CssSelector("td:nth-child(3)"));
            return emailTD.Text;
        }

        /// <summary>
        /// Returns name of the merchant in transaction
        /// </summary>
        /// <returns></returns>
        public string GetFirstTransactionMerchant()
        {
            IWebElement merchantTD = _firstRecord.FindElement(By.CssSelector("td:nth-child(4)"));
            return merchantTD.Text;
        }

        /// <summary>
        /// Returns the date of the last (the first at the top) transaction
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetFirstTransactionDate()
        {
            IWebElement dateTD = _firstRecord.FindElement(By.CssSelector("td:nth-child(15)"));

            string date = dateTD.Text;
            TimeSpan timeSpan = date.ParseTimeSpan();

            return timeSpan;
        }

        /// <summary>
        /// Returns state of the second record in transactions
        /// </summary>
        /// <returns></returns>
        public string GetSecondTransactionState()
        {
            IWebElement status = _secondRecord.FindElement(By.CssSelector("td:nth-child(7)"));
            return status.Text;
        }

        /// <summary>
        /// Returns email of the second record in transactions
        /// </summary>
        /// <returns></returns>
        public string GetSecondTransactionEmail()
        {
            IWebElement emailTD = _secondRecord.FindElement(By.CssSelector("td:nth-child(3)"));
            return emailTD.Text;
        }

        /// <summary>
        /// Returns name of the merchant in the second record in transaction
        /// </summary>
        /// <returns></returns>
        public string GetSecondTransactionMerchant()
        {
            IWebElement merchantTD = _secondRecord.FindElement(By.CssSelector("td:nth-child(4)"));
            return merchantTD.Text;
        }

        /// <summary>
        /// Returns the date of the second record in transactions
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetSecondTransactionDate()
        {
            IWebElement dateTD = _secondRecord.FindElement(By.CssSelector("td:nth-child(15)"));

            string date = dateTD.Text;
            TimeSpan timeSpan = date.ParseTimeSpan();

            return timeSpan;
        }
    }
}
