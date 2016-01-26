using System;
using System.Collections.Generic;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Helpers;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Mobile
{
    /// <summary>
    /// Includes raffle tests on mobile site
    /// </summary>
    [TestFixture("Apple iPhone 4", WayToPay.Neteller)]
    [TestFixture("Apple iPhone 6", WayToPay.Offline)]
    [TestFixture("Apple iPhone 5", WayToPay.TrustPay)]
    [TestFixture("Apple iPhone 4", WayToPay.InternalBalance)]
   //[TestFixture("Samsung Galaxy S4", WayToPay.Skrill)]
    public class BuyRaffleTicketTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private double _totalPrice;
        private OrderVerifications _orderVerifications;
        private CommonActions _commonActions;
        private WayToPay _merchant;
        private string _device;
        private CartVerifications _cartVerifications;
        private TestsSharedCode _sharedCode;
        private bool _setUpFailed = false;

        public BuyRaffleTicketTests(string device, WayToPay merchant)
        {
            _device = device;
            _merchant = merchant;

            SetUp(CreateOptions(_device));
            _sharedCode = new TestsSharedCode(_driver);

            try
            {
                Buy_Raffle_Ticket(_merchant);
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
            SetUp(CreateOptions(_device));
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
        [Category("Critical")]
        public void Check_Transaction_Merchant_In_Transactions()
        {
            SetUp();
            _orderVerifications.CheckTransactionMerchantInTransactions_Back(_merchant);
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        //[Category("Critical")]
        public void a_Check_Transaction_Time_In_Transactions()
        {
            SetUp();
            _orderVerifications.CheckTransactionTimeInTransactions_Back();
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
        /// Checks an amount of money in the first record in transactions (back)
        /// </summary>
        [Test]
       // [Category("Critical")]
        public void Check_Amount_In_Transaction_Back()
        {
            SetUp();
            _orderVerifications.CheckAmountInTransactions_Back(_totalPrice, 1);
        }

        /// <summary>
        /// Checks a play type of the first record in transactions (back)
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Check_PlayType_In_Transactions_Back()
        {
            SetUp();
            _orderVerifications.CheckPlayTypeInTransactions_Back("Raffle");
        }

        /// <summary>
        /// Checks a transaction type of the first record in transactions (back)
        /// </summary>
        [Test]
        public void Check_TransactionType_In_Transactions_Back()
        {
            SetUp();
            _orderVerifications.CheckTransactionTypeInTransactions_Back("Play from real money");
        }

        /// <summary>
        /// Performs once before all other tests. Buys a raffle ticket
        /// </summary>
        private void Buy_Raffle_Ticket(WayToPay merchant)
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

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);
            _totalPrice = rafflePage.TotalPrice;

            rafflePage.ClickBuyNowButton();//
            MerchantsObj merchants = new MerchantsObj(_driver);

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
