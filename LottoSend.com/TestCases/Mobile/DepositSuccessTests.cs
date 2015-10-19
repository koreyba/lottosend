using System.Collections.Generic;
using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace LottoSend.com.TestCases.Mobile
{
    /// <summary>
    /// Includes tests to check depositing process 
    /// </summary>
    [TestFixture("Apple iPhone 4", WayToPay.Neteller)]
    [TestFixture("Apple iPhone 6", WayToPay.Offline)]
    [TestFixture("Apple iPhone 5", WayToPay.TrustPay)]
    [TestFixture("Samsung Galaxy S4", WayToPay.Skrill)]
    public class DepositSuccessTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private OrderVerifications _orderVerifications;
        private CommonActions _commonActions;
        private string _email;
        private WayToPay _merchant;
        private double _balanceBeforePayment;
        private double _depositAmount;
        private string _device; 

        public DepositSuccessTests(string device, WayToPay merchant)
        {
            _device = device;
            _merchant = merchant;
            _depositAmount = 30;

            SetUp();
            Deposit_Money(merchant);
            CleanUp();
        }

        /// <summary>
        /// Deposits money to the user's balance
        /// </summary>
        /// <param name="merchant"></param>
        private void Deposit_Money(WayToPay merchant)
        {
            _email = _commonActions.Log_In_Front("selenium2@gmail.com", _driverCover.Password);
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/deposits/new/");

            DepositMobileObj deposit = new DepositMobileObj(_driver);
            _balanceBeforePayment = deposit.Balance;
            deposit.DepositOtherAmount(_depositAmount, merchant);
        }

        /// <summary>
        /// Checks user's current balance
        /// </summary>
        [Test]
        public void Check_User_Balance()
        {
            _orderVerifications.CheckUserBalance_Front(_balanceBeforePayment, _depositAmount, "selenium2@gmail.com", _driverCover.Password);
        }

        /// <summary>
        /// Checks if the transaction has correct status on "Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_State_In_Transactions()
        {
            _orderVerifications.CheckTransactionsStateInTransactions("TODO/");
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_Email_In_Transactions()
        {
            _orderVerifications.CheckTransactionsEmailInTransactions(_email);
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Merchant_In_Transactions()
        {
            _orderVerifications.CheckTransactionMerchantInTransactions(_merchant);
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Time_In_Transactions()
        {
            _orderVerifications.CheckTransactionTimeInTransactions();
        }

        private ChromeOptions CreateOptions(string device)
        {
            var mobileEmulation = new Dictionary<string, string>
            {
                {"deviceName", device}
            };

            ChromeOptions options = new ChromeOptions();
            options.AddAdditionalCapability("mobileEmulation", mobileEmulation);
            return options;
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver(CreateOptions(_device));
            _driverCover = new DriverCover(_driver);
            _orderVerifications = new OrderVerifications(_driver);
            _commonActions = new CommonActions(_driver);
        }
    }
}
