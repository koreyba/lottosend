using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace LottoSend.com.TestCases.Web.Regular_tickets
{
    /// <summary>
    /// Buys a regular one-draw ticket and performs all needed assertations 
    /// </summary>
    [TestFixture(typeof(ChromeDriver), WayToPay.Neteller)]
    [TestFixture(typeof(FirefoxDriver), WayToPay.Neteller)]
    [TestFixture(typeof(InternetExplorerDriver), (WayToPay.Neteller))]
    [TestFixture(typeof(ChromeDriver), WayToPay.Offline)]
    [TestFixture(typeof(FirefoxDriver), WayToPay.Offline)]
    [TestFixture(typeof(InternetExplorerDriver), WayToPay.Offline)]
    [TestFixture(typeof(ChromeDriver), WayToPay.TrustPay)]
    [TestFixture(typeof(FirefoxDriver), WayToPay.TrustPay)]
    [TestFixture(typeof(InternetExplorerDriver), WayToPay.TrustPay)]
    [TestFixture(typeof(ChromeDriver), WayToPay.Skrill)]
    [TestFixture(typeof(FirefoxDriver), WayToPay.Skrill)]
    [TestFixture(typeof(InternetExplorerDriver), WayToPay.Skrill)]
    public class BuyRegularOneDrawTicketTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private double _totalPrice;
        private OrderVerifications _verifications;
        private CommonActions _commonActions;
        private WayToPay _merchant;

        public BuyRegularOneDrawTicketTests(WayToPay merchant)
        {
            _merchant = merchant;

            SetUp();
            Buy_Regular_One_Draw_Ticket(merchant);
            CleanUp();
        }

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
        [TestCase(1)]
        [TestCase(2)]
        public void Check_Type_Of_Transaction_Front(int numberOfRecordToCheck)
        {
            if (numberOfRecordToCheck == 1)
            {
                _verifications.CheckTypeOfTransactionFront("Play - Single", _driverCover.Login, _driverCover.Password);
            }

            if (numberOfRecordToCheck == 2)
            {
                _verifications.CheckTypeOfTransactionFront("Deposit and play", _driverCover.Login, _driverCover.Password, 2);
            }
        }

        /// <summary>
        /// Checks date of the first and seconds records in users account - transactions (front-end)
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        public void Check_Transaction_Date_Front(int numberOfRecordToCheck)
        {
            _verifications.CheckTransactionDateFront(_driverCover.Login, _driverCover.Password, numberOfRecordToCheck);
        }

        /// <summary>
        /// Checks lottery name of the first and the second records in user's account - transactions in the front-end
        /// </summary>
        [Test]
        public void Check_Transaction_Lottery_Name_Front()
        {
            _verifications.CheckTransactionLotteryNameFront("EuroJackpot", _driverCover.Login, _driverCover.Password, 2);
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
            _verifications.CheckTransactionMerchantInTransactions(WayToPay.Offline);
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
        /// Checks a time when the last bet (in the first record) was made
        /// </summary>
        [Test]
        public void Check_Record_Time_In_Draw()

        {
            _verifications.CheckRecordTimeInDraw("EuroJackpot");
        }

        /// <summary>
        /// Checks an email in the first record (the last bet) 
        /// </summary>
        [Test]
        public void Check_Record_Email_In_Draw()
        {
            _verifications.CheckRecordEmailInDraw("EuroJackpot", _driverCover.Login);
        }

        /// <summary>
        /// Checks type of the ticket in the first record (must be Single)
        /// </summary>
        [Test]
        public void Check_Record_Type_In_Draw()
        {
            _verifications.CheckRecordBetTypeInDraw("Single", "EuroJackpot");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end
        /// </summary>
        [Test]
        public void Check_Record_Price_In_Draw()
        {
            _verifications.CheckRecordPriceInDraw(_totalPrice);
        }

        /// <summary>
        /// Performs once before all other tests. Buys a regular single ticket 
        /// </summary>
        public void Buy_Regular_One_Draw_Ticket(WayToPay merchant)
        {
            // Log in     
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/plays/eurojackpot/");

            //Pay for tickets
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);
            
            //go to single tab
            regularGame.ClickStandartGameButton();

            //Select single draw
            regularGame.SelectOneTimeEntryGame();

            _totalPrice = regularGame.TotalPrice;

            MerchantsObj merchants = regularGame.ClickBuyTicketsButton();
            merchants.Pay(merchant);
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
            //var mobileEmulation = new Dictionary<string, string>
            //{
            //    { "deviceName", "Apple iPhone 6" }
            //};

            //var options = new ChromeOptions();
            //options.AddAdditionalCapability("mobileEmulation", mobileEmulation);

            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _verifications = new OrderVerifications(_driver);
            _commonActions = new CommonActions(_driver);
        }
    }
}
