using System;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Helpers;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Web.Regular_tickets
{
    /// <summary>
    /// Buys a regular multi-draw ticket and performs all needed assertations 
    /// </summary>
    //[TestFixture(typeof(ChromeDriver), WayToPay.Neteller)]
    ////[TestFixture(typeof(FirefoxDriver), WayToPay.Neteller)]
    //[TestFixture(typeof(InternetExplorerDriver), (WayToPay.Neteller))]
    [TestFixture(typeof(ChromeDriver), WayToPay.Offline)]
    ////[TestFixture(typeof(FirefoxDriver), WayToPay.Offline)]
    //[TestFixture(typeof(InternetExplorerDriver), WayToPay.Offline)]
    //[TestFixture(typeof(ChromeDriver), WayToPay.TrustPay)]
    [TestFixture(typeof(ChromeDriver), WayToPay.InternalBalance)]
    ////[TestFixture(typeof(FirefoxDriver), WayToPay.TrustPay)]
    //[TestFixture(typeof(InternetExplorerDriver), WayToPay.TrustPay)]
    //[TestFixture(typeof(ChromeDriver), WayToPay.Skrill)]
    ////[TestFixture(typeof(FirefoxDriver), WayToPay.Skrill)]
    //[TestFixture(typeof(InternetExplorerDriver), WayToPay.Skrill)]
    public class BuyRegularMultiDrawTicketTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private double _totalPrice;
        private OrderVerifications _orderVerifications;
        private CommonActions _commonActions;
        private WayToPay _merchant;
        private CartVerifications _cartVerifications;
        private TestsSharedCode _sharedCode;
        private bool _setUpFailed = false;
        private int _drawsToPlay;

        public BuyRegularMultiDrawTicketTests(WayToPay merchant)
        {
            _merchant = merchant;

            SetUp();
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
        /// Checks web user's email of the last not completed bulk-buy in back/bulk-buys 
        /// </summary>
        [Test]
        public void Check_WebUser_In_BulkBuys_BackOffice()
        {     
            if (_merchant == WayToPay.InternalBalance)
            {
                _orderVerifications.CheckWebUserInBulkBuys_BackOffice(_driverCover.LoginTwo);
            }
            else
            {
                _orderVerifications.CheckWebUserInBulkBuys_BackOffice(_driverCover.Login);
            }
        }

        /// <summary>
        /// Checks lottery name of the last not completed bulk-buy in back/bulk-buys 
        /// </summary>
        [Test]
        public void Check_Lottery_In_BulkBuys_BackOffice()
        {
            _orderVerifications.CheckLotteryInBulkBuys_BackOffice("EuroJackpot");
        } 

        /// <summary>
        /// Checks amount of played draws of the last not completed bulk-buy in back/bulk-buys 
        /// </summary>
        [Test]
        public void Check_Draws_Played_In_BulkBuys_BackOffice()
        {
            _orderVerifications.CheckDrawsPlayedInBulkBuys_BackOffice(1);
        }

        /// <summary>
        /// Checks amount of draws to play of the last not completed bulk-buy in back/bulk-buys 
        /// </summary>
        [Test]
        public void Check_Draw_To_Play_In_BulkBuys_BackOffice()
        {
            _orderVerifications.CheckDrawsToPlayInBulkBuys_BackOffice(_drawsToPlay);
        }

        /// <summary>
        /// Checks back/web_users/bets page and compare type of bet with expected one
        /// </summary>
        [Test]
        public void Check_Type_Of_Bet_In_WebUser_Bets_BackOffice()
        {
            if (_merchant != WayToPay.InternalBalance)
            {
                _orderVerifications.CheckTypeOfBetInBets_BackOffice(_driverCover.Login, "Bulk buy");
            }
            else
            {
                _orderVerifications.CheckTypeOfBetInBets_BackOffice(_driverCover.LoginTwo, "Bulk buy");
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
                _orderVerifications.CheckLotteryInBets_BackOffice(_driverCover.Login, "EuroJackpot");
            }
            else
            {
                _orderVerifications.CheckLotteryInBets_BackOffice(_driverCover.LoginTwo, "EuroJackpot");
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
        /// Checks an amount in the first record in transactions (front)
        /// </summary>
        [Test]
       // [Category("Critical")]
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
        [TestCase(1)]
        [TestCase(2)]
       // [Category("Critical")]
        public void Check_Type_Of_Transaction_Front(int numberOfRecordToCheck)
        {
            if (numberOfRecordToCheck == 1)
            {
                if (_merchant != WayToPay.InternalBalance)
                {
                    _orderVerifications.CheckTypeOfTransaction_Front("Play - Bulk buy", _driverCover.Login,
                        _driverCover.Password);
                }
                else
                {
                    _orderVerifications.CheckTypeOfTransaction_Front("Play - Bulk buy", _driverCover.LoginTwo,
                        _driverCover.Password);
                }
            }

            if (_merchant != WayToPay.InternalBalance)
            {
                if (numberOfRecordToCheck == 2)
                {
                    _orderVerifications.CheckTypeOfTransaction_Front("Deposit and play", _driverCover.Login,
                        _driverCover.Password, 2);
                }
            }
        }

        /// <summary>
        /// Checks a play type of the first record in transactions (back)
        /// </summary>
       // [TestCase(1)]
       // [TestCase(2)]
        public void Check_PlayType_In_Transactions_Back(int numberOfRecordToCheck)
        {
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
       // [TestCase(1)]
       // [TestCase(2)]
        public void Check_TransactionType_In_Transactions_Back(int numberOfRecordToCheck)
        {
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
        /// Checks date of the first and seconds records in users account - transactions (front-end)
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
       // [Category("Critical")]
        public void Check_Transaction_Date_Front(int numberOfRecordToCheck)
        {
            if (_merchant != WayToPay.InternalBalance)
            {
                _orderVerifications.CheckTransactionDate_Front(_driverCover.Login, _driverCover.Password,
                    numberOfRecordToCheck);
            }
            else
            {
                _orderVerifications.CheckTransactionDate_Front(_driverCover.LoginTwo, _driverCover.Password,
                    numberOfRecordToCheck);
            }
        }

        /// <summary>
        /// Checks lottery name of the first and the second records in user's account - transactions in the front-end
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Check_Transaction_Lottery_Name_Front()
        {
            if (_merchant != WayToPay.InternalBalance)
            {
                _orderVerifications.CheckTransactionLotteryName_Front("EuroJackpot", _driverCover.Login,
                    _driverCover.Password, 1);
            }
            else
            {
                _orderVerifications.CheckTransactionLotteryName_Front("EuroJackpot", _driverCover.LoginTwo,
                    _driverCover.Password, 1);
            }
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
       // [Category("Critical")]
        public void Check_Transaction_Merchant_In_Transactions()
        {
            _orderVerifications.CheckTransactionMerchantInTransactions_Back(_merchant);
        }

        /// <summary>
        /// Checks if the transaction has correct status on "Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_State_In_Transactions()
        {
            _orderVerifications.CheckTransactionsStateInTransactions_Back("succeed");
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void a_Check_Transaction_Time_In_Transactions()
        {
            _orderVerifications.CheckTransactionTimeInTransactions_Back();
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
        /// Checks a time when the last bet (in the first record) was made
        /// </summary>
        [Test]
        //[Category("Critical")]
        public void Check_Record_Time_In_Draw()
        {
            _orderVerifications.CheckRecordTimeInDraw("Eurojackpot");
        }

        /// <summary>
        /// Checks an email in the first record (the last bet) 
        /// </summary>
        [Test]
        public void Check_Record_Email_In_Draw()
        {
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
       // [Category("Critical")]
        public void Check_Record_Type_In_Draw()
        {
            _orderVerifications.CheckRecordBetTypeInDraw("Bulk buy", "Eurojackpot");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end
        /// </summary>
        [Test]
       // [Category("Critical")]
        public void Check_Record_Price_In_Draw()
        {
            _orderVerifications.CheckRecordPriceInDraw(_totalPrice, "Eurojackpot");
        }

        /// <summary>
        /// Performs once before all other tests. Buys a regular single ticket 
        /// </summary>
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
            _drawsToPlay = regularGame.NumberOfDraws;


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
            _commonActions = new CommonActions(_driver);
            _cartVerifications = new CartVerifications(_driver);
        }
    }
}
