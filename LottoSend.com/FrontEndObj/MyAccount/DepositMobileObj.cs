using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.TestCases;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj.MyAccount
{
    public class DepositMobileObj : DriverCover
    {
        public DepositMobileObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("deposits"))
            {
                throw new Exception("It's not \"Deposit\" page, please check ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#new_deposit")]
        private IWebElement _depositOptions;

        [FindsBy(How = How.CssSelector, Using = "#new_deposit> div.row > div.other > strong:nth-child(1)")]
        private IWebElement _otherRadioButton;

        [FindsBy(How = How.CssSelector, Using = "#new_deposit> div.row > div.other > input#deposit_amount")]
        private IWebElement _amountInput;

        [FindsBy(How = How.CssSelector, Using = "#account-detail > p > span")]
        private IWebElement _balance;

        /// <summary>
        /// Current Balance
        /// </summary>
        public double Balance
        {
            get { return _balance.Text.ParceDouble(); }
        }

        /// <summary>
        /// Deposits standard amount of money (selects among available) 
        /// </summary>
        /// <param name="amount">How much to deposit</param>
        /// <param name="merchant">How to pay</param>
        /// <param name="ifProcess">To process payment or not (for offline payment)</param>
        /// <param name="isFailed">To faild payment or not</param>
        public void DepositStandardAmount(int amount, WayToPay merchant, bool ifProcess = true, bool isFailed = false)
        {
            SelectStandardAmount(amount);

            PayForDeposit(merchant, ifProcess, isFailed);
        }

        /// <summary>
        /// Selects standard amount of deposit
        /// </summary>
        /// <param name="amount"></param>
        private void SelectStandardAmount(int amount)
        {
            bool isFound = false;

            IList<IWebElement> options = _depositOptions.FindElements(By.CssSelector("div.row > div.bg-primary.amount > strong"));
            foreach (var option in options)
            {
                if (option.Text.Contains(amount.ToString()))
                {
                    isFound = true;
                    option.Click();
                }
            }

            if (!isFound)
            {
                throw new Exception("Sorry but " + amount + " were not found as an options. Page: " + Driver.Url + " ");
            }
        }

        /// <summary>
        /// Deposit other amount of money (pays with Neteller)
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="merchant">Merchant to pay</param>
        /// <param name="ifProcess">Tells if to process the payment of leave it pendant</param>
        /// <param name="isFailed">To fail payment of not</param>
        public void DepositOtherAmount(double amount, WayToPay merchant, bool ifProcess = true, bool isFailed = false)
        {
            _otherRadioButton.Click();
            _amountInput.SendKeys(amount.ToString());
            _otherRadioButton.Click();

            PayForDeposit(merchant, ifProcess, isFailed);
        }

        /// <summary>
        /// Pays for deposit (standard or other amount)
        /// </summary>
        /// <param name="merchant">How to pay</param>
        /// <param name="ifProcess">To process payment (if it was offline)</param>
        /// <param name="isFailed">To fail or not</param>
        private void PayForDeposit(WayToPay merchant, bool ifProcess = true, bool isFailed = false)
        {
            MerchantsObj merchantsObj = new MerchantsObj(Driver);
            merchantsObj.Pay(merchant, ifProcess, isFailed); 
        }
    }
}
