using System;
using System.Collections.Generic;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Mobile
{
    /// <summary>
    /// Includes raffle tests on mobile site
    /// </summary>
    [TestFixture("Apple iPhone 4")]
    [TestFixture("Apple iPhone 6")]
    [TestFixture("Apple iPhone 5")]
    [TestFixture("Samsung Galaxy S4")]
    [TestFixture("Samsung Galaxy Note II")]
    public class BuyRaffleTicketTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private double _totalPrice;
        private OrderVerifications _verifications;
        private CommonActions _commonActions;

        public BuyRaffleTicketTests(string device)
        {
            SetUp(CreateOptions(device));
            Buy_Raffle_Ticket();
            CleanUp();
            
            SetUp();
            Confirn_Payment();
            CleanUp();
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_Email_In_Transactions()
        {
            SetUp();
            _verifications.CheckTransactionsEmailInTransactions(_driverCover.Login);
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Merchant_In_Transactions()
        {
            SetUp();
            _verifications.CheckTransactionMerchantInTransactions(WayToPay.Offline);
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Time_In_Transactions()
        {
            SetUp();
            _verifications.CheckTransactionTimeInTransactions();
        }

        /// <summary>
        /// Performs once before all other tests. Buys a raffle ticket
        /// </summary>
        private void Buy_Raffle_Ticket()
        {
            // Log in     
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);
            _totalPrice = rafflePage.TotalPrice;

            CartObj cart = rafflePage.ClickBuyNowButton();
            MerchantsObj merchants = cart.ClickProceedToCheckoutButton();
            merchants.PayWithOfflineCharge();
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

        private void Confirn_Payment()
        {
            _commonActions.Authorize_in_admin_panel();

            _commonActions.Authorize_the_first_payment();

            _commonActions.Approve_offline_payment();
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
            if (_verifications.Errors.Length > 0)
            {
                Assert.Fail(_verifications.Errors.ToString());
            }
        }

        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driverCover = new DriverCover(_driver);
            _verifications = new OrderVerifications(_driver);
            _commonActions = new CommonActions(_driver);
        }

        public void SetUp(ChromeOptions option)
        {
            _driver = new ChromeDriver(option);
            _driverCover = new DriverCover(_driver);
            _verifications = new OrderVerifications(_driver);
            _commonActions = new CommonActions(_driver);
        }
    }
}
