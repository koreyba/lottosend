using System;
using System.Collections.Generic;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Helpers;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
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
    [TestFixture("Apple iPhone 4", WayToPay.InternalBalance)]
   //[TestFixture("Samsung Galaxy S4", WayToPay.Skrill)]
    public class BuyRegularMultiDrawTicketTests 
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private double _totalPrice;
        private int _numberOfDraws;
        private OrderVerifications _orderVerifications;
        private CommonActions _commonActions;
        private WayToPay _merchant;
        private readonly string _device;
        private CartVerifications _cartVerifications;
        private TestsSharedCode _sharedCode;
        private bool _setUpFailed = false;

        public BuyRegularMultiDrawTicketTests(string device, WayToPay merchant)
        {
            _device = device;
            _merchant = merchant;

            SetUp(CreateOptions(_device));
            _sharedCode = new TestsSharedCode(_driver);
            
            try
            {
                Buy_Regular_Multi_Draw_Ticket(_merchant);
            }
            catch (Exception e)
            {
                _setUpFailed = true;
                CleanUp();

                if (_merchant == WayToPay.InternalBalance)
                {
                    _sharedCode.CleanCartIfTestWasFailed(_driverCover.LoginTwo, _driverCover.Password);
                }
                else
                {
                    _sharedCode.CleanCartIfTestWasFailed(_driverCover.Login, _driverCover.Password);
                }

                throw new Exception("Exception was thrown while executing: " + e.Message + " ");
            }
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
            // Log in     
            if (_merchant != WayToPay.InternalBalance)
            {
                _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
            }
            else
            {
                //If pay with internal balance we need to log in with different user
                _commonActions.Log_In_Front(_driverCover.LoginTwo, _driverCover.Password);
            }
            _cartVerifications.CheckNumberOfTicketsInCart_Front(0);
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_Email_In_Transactions()
        {
            SetUp();
            if (_merchant != WayToPay.InternalBalance)
            {
                _orderVerifications.CheckTransactionsEmailInTransactions_Back(_driverCover.Login);
            }
            else
            {
                _orderVerifications.CheckTransactionsEmailInTransactions_Back(_driverCover.LoginTwo);
            }
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Merchant_In_Transactions()
        {
            SetUp();
            _orderVerifications.CheckTransactionMerchantInTransactions_Back(_merchant);
        }

        /// <summary>
        /// Checks if the transaction has correct status on "Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_State_In_Transactions()
        {
            SetUp();
            _orderVerifications.CheckTransactionsStateInTransactions_Back("succeed");
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
       // [Category("Critical")]
        public void a_Check_Transaction_Time_In_Transactions()
        {
            SetUp();
            _orderVerifications.CheckTransactionTimeInTransactions_Back();
        }

        /// <summary>
        /// Checks an amount of money in the first record in transactions (back)
        /// </summary>
        //[Test]
        [Category("Critical")]
        public void Check_Amount_In_Transaction_Back()
        {
            SetUp();
            _orderVerifications.CheckAmountInTransactions_Back(_totalPrice, 1);
        }

        /// <summary>
        /// Checks a play type of the first record in transactions (back)
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        public void Check_PlayType_In_Transactions_Back(int numberOfRecordToCheck)
        {
            SetUp();
            if (numberOfRecordToCheck == 1)
            {
                _orderVerifications.CheckPlayTypeInTransactions_Back("Bulk buy");
            }

            if (_merchant != WayToPay.InternalBalance)
            {
                if (numberOfRecordToCheck == 2)
                {
                    _orderVerifications.CheckPlayTypeInTransactions_Back("N/A", 2);
                }
            }
        }

        /// <summary>
        /// Checks a transaction type of the first record in transactions (back)
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        public void Check_TransactionType_In_Transactions_Back(int numberOfRecordToCheck)
        {
            SetUp();
            if (numberOfRecordToCheck == 1)
            {
                _orderVerifications.CheckTransactionTypeInTransactions_Back("Play from real money");
            }

            if (_merchant != WayToPay.InternalBalance)
            {
                if (numberOfRecordToCheck == 2)
                {
                    _orderVerifications.CheckTransactionTypeInTransactions_Back("Credit to real money", 2);
                }
            }
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
            if (_merchant != WayToPay.InternalBalance)
            {
                _orderVerifications.CheckRecordEmailInDraw("Eurojackpot", _driverCover.Login);
            }
            else
            {
                _orderVerifications.CheckRecordEmailInDraw("Eurojackpot", _driverCover.LoginTwo);
            }
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
            if (merchant != WayToPay.InternalBalance)
            {
                _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
            }
            else
            {
                //If pay with internal balance we need to log in with different user
                _commonActions.Log_In_Front(_driverCover.LoginTwo, _driverCover.Password);
            }

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/eurojackpot/");

            //Pay for tickets
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            //Go to single tab
            regularGame.ClickStandartGameButton();

            _totalPrice = regularGame.TotalPrice;
            _numberOfDraws = regularGame.NumberOfDraws;


            MerchantsObj merchants = regularGame.ClickBuyTicketsButton();

            if (merchant != WayToPay.InternalBalance)
            {
                merchants.Pay(merchant);
            }
            else
            {
                CheckoutObj checkout = new CheckoutObj(_driver);
                checkout.ClickCompleteYourOrderButton();
            }
        }

        /// <summary>
        /// Creates and returns ChromeOptions for a mobile device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public ChromeOptions CreateOptions(string device)
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
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
            {
                _driverCover.TakeScreenshot();
            }
            _sharedCode.CleanUp(ref _driver);
        }

        //[SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driverCover = new DriverCover(_driver);
            _orderVerifications = new OrderVerifications(_driver);
            _commonActions = new CommonActions(_driver);
            _cartVerifications = new CartVerifications(_driver);
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
