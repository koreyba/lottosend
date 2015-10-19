using System.Collections.Generic;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Mobile.Regular_tickets
{
    /// <summary>
    /// Includes tests for buying a regular multi draw ticket on mobile site 
    /// </summary>
    [TestFixture("Apple iPhone 4", WayToPay.Neteller)]
    [TestFixture("Apple iPhone 6", WayToPay.Offline)]
    [TestFixture("Apple iPhone 5", WayToPay.TrustPay)]
    [TestFixture("Samsung Galaxy S4", WayToPay.Skrill)]
    public class BuyRegularMultiDrawTicketTests 
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private double _totalPrice;
        private int _numberOfDraws;
        private OrderVerifications _orderVerifications;
        private CommonActions _commonActions;
        private WayToPay _merchant;
        private string _device;
        private CartVerifications _cartVerifications;

        public BuyRegularMultiDrawTicketTests(string device, WayToPay merchant)
        {
            _device = device;
            _merchant = merchant;

            SetUp(CreateOptions(_device));
            Buy_Regular_Multi_Draw_Ticket(_merchant);
            CleanUp();

            //SetUp();
            //Confirn_Payment();
            //CleanUp();
        }

        /// <summary>
        /// Checks if after buying a ticket (after payment) there are no items in the cart
        /// </summary>
        [Test]
        public void Check_If_There_Is_No_Ticket_In_Cart()
        {
            SetUp();
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
            _cartVerifications.CheckNumberOfTicketsInCart_Front(0);
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_Email_In_Transactions()
        {
            SetUp();
            _orderVerifications.CheckTransactionsEmailInTransactions(_driverCover.Login);
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Merchant_In_Transactions()
        {
            SetUp();
            _orderVerifications.CheckTransactionMerchantInTransactions(_merchant);
        }

        /// <summary>
        /// Checks if the transaction has correct status on "Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_State_In_Transactions()
        {
            SetUp();
            _orderVerifications.CheckTransactionsStateInTransactions("TODO/");
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Time_In_Transactions()
        {
            SetUp();
            _orderVerifications.CheckTransactionTimeInTransactions();
        }

        /// <summary>
        /// Checks a time when the last bet (in the first record) was made
        /// </summary>
        [Test]
        public void Check_Record_Time_In_Draw()
        {
            SetUp();
            _orderVerifications.CheckRecordTimeInDraw("Eurojackpot");
        }

        /// <summary>
        /// Checks an email in the first record (the last bet) 
        /// </summary>
        [Test]
        public void Check_Record_Email_In_Draw()
        {
            SetUp();
            _orderVerifications.CheckRecordEmailInDraw("Eurojackpot", _driverCover.Login);
        }

        /// <summary>
        /// Checks type of the ticket in the first record (must be Single)
        /// </summary>
        [Test]
        public void Check_Record_Type_In_Draw()
        {
            SetUp();
            _orderVerifications.CheckRecordBetTypeInDraw("Bulk buy", "Eurojackpot");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end
        /// </summary>
        [Test]
        public void Check_Record_Price_In_Draw()
        {
            SetUp();
            _orderVerifications.CheckRecordPriceInDraw(_totalPrice, _numberOfDraws);
        }

        /// <summary>
        /// Performs once before all other tests. Buys a regular single ticket 
        /// </summary>
      //  [TestFixtureSetUp]
        public void Buy_Regular_Multi_Draw_Ticket(WayToPay merchant)
        {
            // Log in     
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/eurojackpot/");

            //Pay for tickets
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            //Go to single tab
            regularGame.ClickStandartGameButton();

            _totalPrice = regularGame.TotalPrice;
            _numberOfDraws = regularGame.NumberOfDraws;


            MerchantsObj merchants = regularGame.ClickBuyTicketsButton();
            merchants.Pay(merchant);
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
            if (_orderVerifications.Errors.Length > 0)
            {
                Assert.Fail(_orderVerifications.Errors.ToString());
            }
        }

        //[SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driverCover = new DriverCover(_driver);
            _orderVerifications = new OrderVerifications(_driver);
            _commonActions = new CommonActions(_driver);
        }

        public void SetUp(ChromeOptions option)
        {
            _driver = new ChromeDriver(option);
            _driverCover = new DriverCover(_driver);
            _orderVerifications = new OrderVerifications(_driver);
            _commonActions = new CommonActions(_driver);
            _cartVerifications = new CartVerifications(_driver);
        }
    }
}
