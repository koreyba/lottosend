using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestFramework.FrontEndObj.Common;

namespace TestFramework.FrontEndObj.MyAccount
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

        [FindsBy(How = How.CssSelector, Using = "#new_deposit div.row > div.bg-primary.amount input")]
        private IList<IWebElement> _depositOptions;

        [FindsBy(How = How.CssSelector, Using = "div.other")]
        private IWebElement _otherRadioButton;

        [FindsBy(How = How.CssSelector, Using = "input#deposit_total")]
        private IWebElement _amountInput;

        [FindsBy(How = How.CssSelector, Using = "#account-detail > p > span")]
        private IWebElement _balance;

        /// <summary>
        /// Current Balance
        /// </summary>
        public double Balance
        {
            get { return StringExtention.ParseDouble(_balance.Text); }
        }

        /// <summary>
        /// Returns currently selected value of standard deposit
        /// </summary>
        /// <returns></returns>
        public string GetSelectedAmount()
        {
            foreach (var el in _depositOptions)
            {
                if (el.GetAttribute("checked") != null && el.GetAttribute("checked").Equals("true"))
                {
                    return el.GetAttribute("value");
                }
            }

            return "";
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

            foreach (var option in _depositOptions)
            {
                if (option.GetAttribute("value").Contains(amount.ToString(System.Globalization.CultureInfo.InvariantCulture)))
                {
                    isFound = true;
                    option.FindElement(By.XPath("../strong")).Click();
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
            _amountInput.SendKeys(amount.ToString(System.Globalization.CultureInfo.InvariantCulture));
            //_otherRadioButton.Click();
            _balance.Click();

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
