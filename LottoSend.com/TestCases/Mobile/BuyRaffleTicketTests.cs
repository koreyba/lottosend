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
    [TestFixture]
    public class BuyRaffleTicketTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private double _totalPrice;
        private OrderVerifications _verifications;
        private CommonActions _commonActions;

        public BuyRaffleTicketTests()
        {
            Buy_Regular_One_Draw_Ticket();
            Confirn_Payment();
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
            _verifications.CheckTransactionMerchantInTransactions(WaysToPay.Offline);
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
        private void Buy_Regular_One_Draw_Ticket()
        {
            SetUp(CreateOptions());

            // Log in     
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);
            _totalPrice = rafflePage.TotalPrice;

            CartObj cart = rafflePage.ClickBuyNowButton();
            MerchantsObj merchants = cart.ClickProceedToCheckoutButton();
            merchants.PayWithOfflineCharge();

            CleanUp();
        }

        private ChromeOptions CreateOptions()
        {
            var mobileEmulation = new Dictionary<string, string>
            {
                {"deviceName", "Apple iPhone 6"}
            };

            ChromeOptions options = new ChromeOptions();
            options.AddAdditionalCapability("mobileEmulation", mobileEmulation);
            return options;
        }

        private void Confirn_Payment()
        {
            SetUp();

            _commonActions.Authorize_in_admin_panel();

            _commonActions.Authorize_the_first_payment();

            _commonActions.Approve_offline_payment();

            CleanUp();
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

        //[SetUp]
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
