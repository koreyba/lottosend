using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LottoSend.com.FrontEndObj.MyAccount
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

        [FindsBy(How = How.CssSelector, Using = "table.table.balance > tbody > tr.odd.text-center:nth-child(2)")]
        private IWebElement _secondRecord;

        [FindsBy(How = How.CssSelector, Using = "table.table.balance > tbody > tr.odd.text-center")]
        private IWebElement _firstRecord;

        /// <summary>
        /// Returns Data of the second record
        /// </summary>
        public string SecondRecordDate
        {
            get { return _secondRecord.FindElement(By.CssSelector("td:nth-child(1)")).Text; }
        }

        /// <summary>
        /// Returns Lottery of the second record
        /// </summary>
        public string SecondRecordLottery
        {
            get { return _secondRecord.FindElement(By.CssSelector("td:nth-child(4)")).Text; }
        }

        /// <summary>
        /// Returns Data of the first record
        /// </summary>
        public string FirstRecordDate
        {
            get { return _firstRecord.FindElement(By.CssSelector("td:nth-child(1)")).Text; }
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
