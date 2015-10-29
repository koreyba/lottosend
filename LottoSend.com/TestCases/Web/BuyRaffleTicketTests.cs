using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace LottoSend.com.TestCases.Web
{
    /// <summary>
    /// Buys a raffle ticket on web
    /// </summary>
    /// <typeparam name="TWebDriver"></typeparam>
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
    public class BuyRaffleTicketTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private double _totalPrice;
        private OrderVerifications _orderVerifications;
        private CartVerifications _cartVerifications;
        private CommonActions _commonActions;
        private WayToPay _merchant;
        

        public BuyRaffleTicketTests(WayToPay merchant)
        {
            _merchant = merchant;

            SetUp();
            Buy_Raffle_Ticket(_merchant);
            CleanUp();
        }

        /// <summary>
        /// Checks if after buying a ticket (after payment) there are no items in the cart
        /// </summary>
        [Test]
        public void Check_If_There_Is_No_Ticket_In_Cart()
        {
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
            _cartVerifications.CheckNumberOfTicketsInCart(0);
        }

        /// <summary>
        /// Checks an amount in the first record in transactions (front)
        /// </summary>
        [Test]
        public void Check_Amount_In_Transaction_Front()
        {
            _orderVerifications.CheckAmountInTransactionFront(_totalPrice, _driverCover.Login, _driverCover.Password, 1);
        }

        /// <summary>
        /// Checks a type of the first record in transactions (front)
        /// </summary>
        [Test]
        public void Check_Type_Of_Transaction_Front()
        {
            _orderVerifications.CheckTypeOfTransactionFront("Play - Raffle", _driverCover.Login, _driverCover.Password);
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_Email_In_Transactions()
        {
            _orderVerifications.CheckTransactionsEmailInTransactions(_driverCover.Login);
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

        /// <summary>
<<<<<<< HEAD
=======
        /// Checks if the transaction has correct status on "Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_State_In_Transactions()
        {
            _orderVerifications.CheckTransactionsStateInTransactions("TODO/");
        }

        /// <summary>
>>>>>>> parent of 3f4f35e... daily
        /// Performs once before all other tests. Buys a raffle ticket
        /// </summary>
        public void Buy_Raffle_Ticket(WayToPay merchant)
        {
            // Log in     
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);
            _totalPrice = rafflePage.TotalPrice;

            CartObj cart = rafflePage.ClickBuyNowButton();
            MerchantsObj merchants = cart.ClickProceedToCheckoutButton();

            merchants.Pay(merchant);
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
            if (_orderVerifications.Errors.Length > 0)
            {
                Assert.Fail(_orderVerifications.Errors.ToString());
            }
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _orderVerifications = new OrderVerifications(_driver);
            _commonActions = new CommonActions(_driver);
            _cartVerifications = new CartVerifications(_driver);
        }
    }
}
