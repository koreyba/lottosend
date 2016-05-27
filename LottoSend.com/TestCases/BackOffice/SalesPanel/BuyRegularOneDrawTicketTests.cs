using System;
using LottoSend.com.BackEndObj.SalesPanelPages;
using LottoSend.com.Helpers;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver), WayToPay.Offline, "El Gordo")]
    [TestFixture(typeof(ChromeDriver), WayToPay.InternalBalance, "EuroMillions")]
    class BuyRegularOneDrawTicketTests<TWebDriver> where TWebDriver : IWebDriver, new() 
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private OrderVerifications _orderVerifications;
        private WayToPay _merchant;
        private double _totalPrice;
        private TestsSharedCode _sharedCode;
        private string _lotteryName;
        private CartVerifications _cartVerifications;
        private bool _setUpFailed = false;

        public BuyRegularOneDrawTicketTests(WayToPay merchant, string lottery)
        {
            _merchant = merchant;
            _lotteryName = lottery;

            SetUp();
            _sharedCode = new TestsSharedCode(_driver);

            try
            {
                Buy_Regular_One_Draw_Ticket(merchant, _lotteryName);
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
                _orderVerifications.CheckTypeOfBetInBets_BackOffice(_driverCover.Login, "Single");
            }
            else
            {
                _orderVerifications.CheckTypeOfBetInBets_BackOffice(_driverCover.LoginTwo, "Single");
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
                _orderVerifications.CheckLotteryInBets_BackOffice(_driverCover.Login, _lotteryName);
            }
            else
            {
                _orderVerifications.CheckLotteryInBets_BackOffice(_driverCover.LoginTwo, _lotteryName);
            }
        }

        /// <summary>
        /// Checks a play type of the first record in transactions (back)
        /// </summary>
        //[TestCase(1)]
        //[TestCase(2)]
        public void Check_PlayType_In_Transactions_Back(int numberOfRecordToCheck)
        {
            if (numberOfRecordToCheck == 1)
            {
                _orderVerifications.CheckPlayTypeInTransactions_Back("Single");
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
        [Category("Critical")]
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
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Category("Critical")]
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
        [Category("Critical")]
        public void a_Check_Transaction_Time_In_Transactions()
        {
            _orderVerifications.CheckTransactionTimeInTransactions_Back();
        }

        /// <summary>
        /// Checks an amount of money in the first record in transactions (back)
        /// </summary>
        //[Test]
        [Category("Critical")]
        public void Check_Amount_In_Transaction_Back()
        {
            _orderVerifications.CheckAmountInTransactions_Back(_totalPrice, 1);
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_Email_In_Transactions()
        {
            if (_merchant == WayToPay.InternalBalance)
            {
                _orderVerifications.CheckTransactionsEmailInTransactions_Back(_driverCover.LoginTwo);
            }

            if (_merchant == WayToPay.Offline)
            {
                _orderVerifications.CheckTransactionsEmailInTransactions_Back(_driverCover.Login);
            }
        }

        /// <summary>
        /// Checks a time when the last bet (in the first record) was made
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Check_Record_Time_In_Draw()
        {
            _orderVerifications.CheckRecordTimeInDraw(_lotteryName);
        }

        /// <summary>
        /// Checks an email in the first record (the last bet) 
        /// </summary>
        [Test]
        public void Check_Record_Email_In_Draw()
        {
            if (_merchant != WayToPay.InternalBalance)
            {
                _orderVerifications.CheckRecordEmailInDraw(_lotteryName, _driverCover.Login);
            }
            else
            {
                _orderVerifications.CheckRecordEmailInDraw(_lotteryName, _driverCover.LoginTwo);
            }
        }

        /// <summary>
        /// Checks type of the ticket in the first record (must be Single)
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Check_Record_Type_In_Draw()
        {
            _orderVerifications.CheckRecordBetTypeInDraw("Single", _lotteryName);
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Check_Record_Price_In_Draw()
        {
            _orderVerifications.CheckRecordPriceInDraw(_totalPrice, _lotteryName);
        }

        /// <summary>
        /// Performs once before all other tests. Buys a regular single ticket 
        /// </summary>
        public void Buy_Regular_One_Draw_Ticket(WayToPay merchant, string lottery)
        {
            _commonActions.SignIn_in_admin_panel();

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            Add_Ticket_To_Cart(lottery);

            //Pays for tickets in the cart (offline or internal balance).
            _totalPrice = _commonActions.PayForTicketsInCart_SalesPanel(_merchant);
        }

        /// <summary>
        /// Checks if after buying a ticket (after payment) there are no items in the cart
        /// </summary>
        [Test]
        public void Check_If_There_Is_No_Ticket_In_Cart()
        {
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
        [TestCase(1)]
        [TestCase(2)]
        [Category("Critical")]
        public void Check_Type_Of_Transaction_Front(int numberOfRecordToCheck)
        {
            if (numberOfRecordToCheck == 1)
            {
                if (_merchant != WayToPay.InternalBalance)
                {
                    _orderVerifications.CheckTypeOfTransaction_Front("Play - Single", _driverCover.Login,
                        _driverCover.Password);
                }
                else
                {
                    _orderVerifications.CheckTypeOfTransaction_Front("Play - Single", _driverCover.LoginTwo,
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
        /// Checks date of the first and seconds records in users account - transactions (front-end)
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [Category("Critical")]
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
                _orderVerifications.CheckTransactionLotteryName_Front(_lotteryName, _driverCover.Login,
                    _driverCover.Password, 1);
            }
            else
            {
                _orderVerifications.CheckTransactionLotteryName_Front(_lotteryName, _driverCover.LoginTwo,
                    _driverCover.Password, 1);
            }
        }

        /// <summary>
        /// Add a ticket to the cart
        /// </summary>
        /// <param name="lotteryName"></param>
        private void Add_Ticket_To_Cart(string lotteryName)
        {
            RegisterObj regForm = new RegisterObj(_driver);

            if (_merchant == WayToPay.Offline)
            {
                regForm.SignIn(_driverCover.Login);
            }

            if (_merchant == WayToPay.InternalBalance)
            {
                 regForm.SignIn(_driverCover.LoginTwo);
            }

            MenuObj menu = new MenuObj(_driver);
            menu.GoToLotteryPage(lotteryName);
            GroupGameObj group = new GroupGameObj(_driver);
            group.SwitchToSingleTab();

            RegularGameObj game = new RegularGameObj(_driver);
            game.AddToCart();
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
            _commonActions = new CommonActions(_driver);
            _orderVerifications = new OrderVerifications(_driver);
            _cartVerifications = new CartVerifications(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
