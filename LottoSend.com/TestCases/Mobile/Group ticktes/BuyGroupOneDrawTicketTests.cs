using System.Collections.Generic;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Mobile.Group_ticktes
{
    /// <summary>
    /// Includes tests for buying a group one draw ticket on mobile site
    /// </summary>
    [TestFixture("Apple iPhone 4", WayToPay.Neteller)]
    [TestFixture("Apple iPhone 6", WayToPay.Offline)]
    [TestFixture("Apple iPhone 5", WayToPay.TrustPay)]
    [TestFixture("Samsung Galaxy S4", WayToPay.Skrill)]
    public class BuyGroupOneDrawTicketTests 
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private double _totalPrice;
        private OrderVerifications _verifications;
        private CommonActions _commonActions;
        private WayToPay _merchant;
        private string _device;
        private CartVerifications _cartVerifications;

        public BuyGroupOneDrawTicketTests(string device, WayToPay merchant)
        {
            _device = device;
            _merchant = merchant;

            SetUp(CreateOptions(device));
            Buy_Group_One_Draw_Ticket(_merchant);
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
            _cartVerifications.CheckNumberOfTicketsInCart(0);
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
            _verifications.CheckTransactionMerchantInTransactions(_merchant);
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
        /// Checks a time when the last bet (in the first record) was made
        /// </summary>
        [Test]
        public void Check_Record_Time_In_Draw()
        {
            SetUp();
            _verifications.CheckRecordTimeInDraw("Eurojackpot");
        }

        /// <summary>
        /// Checks an email in the first record (the last bet) 
        /// </summary>
        [Test]
        public void Check_Record_Email_In_Draw()
        {
            SetUp();
            _verifications.CheckRecordEmailInDraw("Eurojackpot", _driverCover.Login);
        }

        /// <summary>
        /// Checks type of the ticket in the first record (must be Single)
        /// </summary>
        [Test]
        public void Check_Record_Type_In_Draw()
        {
            SetUp();
            _verifications.CheckRecordBetTypeInDraw("Single", "Eurojackpot");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end
        /// </summary>
        [Test]
        public void Check_Record_Price_In_Draw()
        {
            SetUp();
            _verifications.CheckRecordPriceInDraw(_totalPrice) ;
        }

        /// <summary>
        /// Performs once before all other tests. Buys a group single ticket 
        /// </summary>
        //[TestFixtureSetUp]
        public void Buy_Group_One_Draw_Ticket(WayToPay merchant)
        {
            // Log in     
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/eurojackpot/");

            //Pay for tickets
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);  

            //Select single draw
            groupGame.SelectOneTimeEntryGame();

            _totalPrice = groupGame.TotalPrice;

            MerchantsObj merchants = groupGame.ClickBuyTicketsButton();
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
            _cartVerifications = new CartVerifications(_driver);
        }
    }
}
