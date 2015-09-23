using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases
{
    [TestFixture]
    public class BuyRaffleTicket
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private double _totalPrice;
        private OrderVerifications _verifications;
        private CommonActions _commonActions;

        /// <summary>
        /// Checks an amount in the first record in transactions (front)
        /// </summary>
        [Test]
        public void Check_Amount_In_Transaction_Front()
        {
            _verifications.CheckAmountInTransactionFront(_totalPrice, _driverCover.Login, _driverCover.Password, 1);
        }

        /// <summary>
        /// Checks a type of the first record in transactions (front)
        /// </summary>
        [Test]
        public void Check_Type_Of_Transaction_Front()
        {
            _verifications.CheckTypeOfTransactionFront("Play - Raffle", _driverCover.Login, _driverCover.Password);
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_Email_In_Transactions()
        {
            _verifications.CheckTransactionsEmailInTransactions(_driverCover.Login);
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Merchant_In_Transactions()
        {
            _verifications.CheckTransactionMerchantInTransactions(WaysToPay.Offline);
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Time_In_Transactions()
        {
            _verifications.CheckTransactionTimeInTransactions();
        }

        /// <summary>
        /// Performs once before all other tests. Buys a raffle ticket
        /// </summary>
        [TestFixtureSetUp]
        public void Buy_Regular_One_Draw_Ticket()
        {
            SetUp();

            // Log in     
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);
            _totalPrice = rafflePage.TotalPrice;

            CartObj cart = rafflePage.ClickBuyNowButton();
            MerchantsObj merchants = cart.ClickProceedToCheckoutButton();

            merchants.PayWithOfflineCharge();
            
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

        [SetUp]
        public void SetUp()
        {
             //= new FirefoxDriver();

           // DesiredCapabilities capabilities = new DesiredCapabilities();
            //capabilities = DesiredCapabilities.Firefox();
            //capabilities.SetCapability(CapabilityType.BrowserName, "firefox");
            //capabilities.SetCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));

            //_driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability); 


            //var mobileEmulation = new Dictionary<string, string>
            //    {
            //        {"deviceName", "Apple iPhone 6"}
            //    };

            //var options = new ChromeOptions();
            //options.AddAdditionalCapability("mobileEmulation", mobileEmulation);

            _driver = new ChromeDriver();
            _driverCover = new DriverCover(_driver);
            _verifications = new OrderVerifications(_driver);
            _commonActions = new CommonActions(_driver);

           
        }
    }
}
