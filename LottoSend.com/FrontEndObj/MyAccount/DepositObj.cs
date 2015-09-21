using System;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.TestCases;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj.MyAccount
{
    /// <summary>
    /// Page Object of front - my account - deposit page
    /// </summary>
    public class DepositObj : DriverCover
    {
        public DepositObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("deposits"))
            {
                throw new Exception("It's mot \"Deposit\" page, please check ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#deposit-options > div.text-center.grey.other > div > label > strong:nth-child(1)")]
        private IWebElement _otherRadioButton;

        [FindsBy(How = How.CssSelector, Using = "#deposit-options > div.text-center.grey.other > div > label > input#deposit_amount")]
        private IWebElement _amountInput;

        [FindsBy(How = How.Id, Using = "account-detail")]
        private IWebElement _balance;

        /// <summary>
        /// Current Balance
        /// </summary>
        public double Balance
        {
            get { return _balance.Text.ParceDouble(); }
        }

        /// <summary>
        /// Deposit other amount of money (pays with Neteller)
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="merchant">Merchant to pay</param>
        /// <param name="ifProcess">Tells if to process the payment of leave it pended</param>
        /// <param name="isFailed">To fail payment of not</param>
        public void DepositOtherAmount(double amount, WaysToPay merchant, bool ifProcess = true, bool isFailed = false)
        {
            _otherRadioButton.Click();
            _amountInput.SendKeys(amount.ToString());
           // _balance.Click();
            MerchantsObj merchantsObj = new MerchantsObj(Driver);

            if (merchant == WaysToPay.Neteller)
            {
                merchantsObj.PayWithNeteller();
            }

            if (merchant == WaysToPay.Offline)
            {
                merchantsObj.PayWithOfflineCharge();

                if (ifProcess)
                {
                    CommonActions commonActions = new CommonActions(Driver);

                    commonActions.Authorize_in_admin_panel();
                    commonActions.Authorize_the_first_payment();
                    if (!isFailed)
                    {
                        commonActions.Approve_offline_payment();
                    }
                    else
                    {
                        commonActions.Fail_offline_payment();
                    }
                }

            }
        }
    }
}
