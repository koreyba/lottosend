﻿using System;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.FrontEndObj.Common;
using TestFramework.FrontEndObj.GamePages;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.Web
{
    /// <summary>
    /// Buys a raffle ticket on web
    /// </summary>
    /// <typeparam name="TWebDriver"></typeparam>
    //[TestFixture(typeof(ChromeDriver), WayToPay.Neteller)]
   // ////[TestFixture(typeof(FirefoxDriver), WayToPay.Neteller)]
    //[TestFixture(typeof(InternetExplorerDriver), (WayToPay.Neteller))]
    [TestFixture(typeof(ChromeDriver), WayToPay.Offline)]
    //////[TestFixture(typeof(FirefoxDriver), WayToPay.Offline)]
    //[TestFixture(typeof(InternetExplorerDriver), WayToPay.Offline)]
    //[TestFixture(typeof(ChromeDriver), WayToPay.TrustPay)]
    [TestFixture(typeof(ChromeDriver), WayToPay.InternalBalance)]
   // ////[TestFixture(typeof(FirefoxDriver), WayToPay.TrustPay)]
    //[TestFixture(typeof(InternetExplorerDriver), WayToPay.TrustPay)]
    //[TestFixture(typeof(ChromeDriver), WayToPay.Skrill)]
    //////[TestFixture(typeof(FirefoxDriver), WayToPay.Skrill)]
    //[TestFixture(typeof(InternetExplorerDriver), WayToPay.Skrill)]
    public class BuyRaffleTicketTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private double _totalPrice;
        private OrderVerifications _orderVerifications;
        private CartVerifications _cartVerifications;
        private TestFramework.CommonActions _commonActions;
        private WayToPay _merchant;
        private TestsSharedCode _sharedCode;
        private bool _setUpFailed = false;

        public BuyRaffleTicketTests(WayToPay merchant)
        {
            _merchant = merchant;

            SetUp();
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
                    _sharedCode.CleanCart(_driverCover.LoginTwo, _driverCover.Password);
                }
                else
                {
                    _sharedCode.CleanCart(_driverCover.Login, _driverCover.Password);
                }

                throw new Exception("Exception was thrown while executing: " + e.Message + " ");
            }
            
            CleanUp();
        }

        /// <summary>
        /// Checks back/web_users/bets page and compare type of bet with expected one
        /// </summary>
        [Test]
        public void Check_Type_Of_Bet_In_WebUser_Bets_BackOffice()
        {
            if (_merchant != WayToPay.InternalBalance)
            {
                _orderVerifications.CheckTypeOfBetInBets_BackOffice(_driverCover.Login, "Raffle");
            }
            else
            {
                _orderVerifications.CheckTypeOfBetInBets_BackOffice(_driverCover.LoginTwo, "Raffle");
            }
        }

        /// <summary>
        /// Checks back/web_users/bets page and compare price of bet (amount) with expected one
        /// </summary>
        [Test]
        public void Check_Price_In_WebUser_Bets_BackOffice()
        {
            if (_merchant != WayToPay.InternalBalance)
            {
                _orderVerifications.CheckPriceInBets_BackOffice(_driverCover.Login, _totalPrice);
            }
            else
            {
                _orderVerifications.CheckPriceInBets_BackOffice(_driverCover.LoginTwo, _totalPrice);
            }
        }

        /// <summary>
        /// Checks back/web_users/bets page and compare lottery name in the first record with expected one
        /// </summary>
        [Test]
        public void Check_Lottery_Name_In_WebUser_Bets_BackOffice()
        {
            if (_merchant != WayToPay.InternalBalance)
            {
                _orderVerifications.CheckLotteryInBets_BackOffice(_driverCover.Login, "Loteria de Navidad");
            }
            else
            {
                _orderVerifications.CheckLotteryInBets_BackOffice(_driverCover.LoginTwo, "Loteria de Navidad");
            }
        }

        /// <summary>
        /// Checks if after buying a ticket (after payment) there are no items in the cart
        /// </summary>
        [Test]
        public void Check_If_There_Is_No_Ticket_In_Cart()
        {
            // Log in     
            if (_merchant != WayToPay.InternalBalance)
            {
                _commonActions.Log_In_Front_PageOne(_driverCover.Login, _driverCover.Password);
            }
            else
            {
                //If pay with internal balance we need to log in with different user
                _commonActions.Log_In_Front_PageOne(_driverCover.LoginTwo, _driverCover.Password);
            }
            _cartVerifications.CheckNumberOfTicketsInCart_Front(0);
        }

        /// <summary>
        /// Checks an amount in the first record in transactions (front)
        /// </summary>
        //[Test]
        [Category("Critical")]
        public void Check_Amount_In_Transaction_Front()
        {
            // Log in     
            if (_merchant != WayToPay.InternalBalance)
            {
                _orderVerifications.CheckAmountInTransaction_Front(_totalPrice, _driverCover.Login, _driverCover.Password, 1);
            }
            else
            {
                //If pay with internal balance we need to log in with different user
                _orderVerifications.CheckAmountInTransaction_Front(_totalPrice, _driverCover.LoginTwo, _driverCover.Password, 1);
            }
        }

        /// <summary>
        /// Checks a type of the first record in transactions (front)
        /// </summary>
        [Test]
       // [Category("Critical")]
        public void Check_Type_Of_Transaction_Front()
        {
            if (_merchant != WayToPay.InternalBalance)
            {
                _orderVerifications.CheckTypeOfTransaction_Front("Play - Raffle", _driverCover.Login,
                    _driverCover.Password);
            }
            else
            {
                _orderVerifications.CheckTypeOfTransaction_Front("Play - Raffle", _driverCover.LoginTwo,
                    _driverCover.Password);
            }
        }

        /// <summary>
        /// Checks a play type of the first record in transactions (back)
        /// </summary>
       // [Test]
        public void Check_PlayType_In_Transactions_Back()
        {
            _orderVerifications.CheckPlayTypeInTransactions_Back("Raffle");
        }

        /// <summary>
        /// Checks a transaction type of the first record in transactions (back)
        /// </summary>
        [Test]
        public void Check_TransactionType_In_Transactions_Back()
        {
            _orderVerifications.CheckTransactionTypeInTransactions_Back("Play from real money", 2);
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_Email_In_Transactions()
        {
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
        //[Category("Critical")]
        public void Check_Transaction_Merchant_In_Transactions()
        {
            _orderVerifications.CheckTransactionMerchantInTransactions_Back(_merchant);
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        //[Category("Critical")]
        public void a_Check_Transaction_Time_In_Transactions()
        {
            _orderVerifications.CheckTransactionTimeInTransactions_Back();
        }

        /// <summary>
        /// Checks if the transaction has correct status on "Transactions" page
        /// </summary>
        [Test]
       // [Category("Critical")]
        public void Check_Transactions_State_In_Transactions()
        {
            _orderVerifications.CheckTransactionsStateInTransactions_Back("succeed");
        }

        /// <summary>
        /// Checks an amount of money in the first record in transactions (back)
        /// </summary>
        [Test]
        public void Check_Amount_In_Transaction_Back()
        {
            _orderVerifications.CheckAmountInTransactions_Back(_totalPrice, 1);
        }

        /// <summary>
        /// Performs once before all other tests. Buys a raffle ticket
        /// </summary>
        public void Buy_Raffle_Ticket(WayToPay merchant)
        {
            // Log in     
            if (merchant != WayToPay.InternalBalance)
            {
                _commonActions.Log_In_Front_PageOne(_driverCover.Login, _driverCover.Password);
            }
            else
            {
                //If pay with internal balance we need to log in with different user
                _commonActions.Log_In_Front_PageOne(_driverCover.LoginTwo, _driverCover.Password);
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

        [TearDown]
        public void CleanUp()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
                {
                    _driverCover.TakeScreenshot();
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                _sharedCode.CleanUp(ref _driver);
            }
        }

        [SetUp]
        public void SetUp()
        {
            MessageConsoleCreator message = new MessageConsoleCreator();
            message.TestWillRun();
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _orderVerifications = new OrderVerifications(_driver);
            _commonActions = new TestFramework.CommonActions(_driver);
            _cartVerifications = new CartVerifications(_driver);
        }
    }
}
