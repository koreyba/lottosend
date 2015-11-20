using System;
using LottoSend.com.BackEndObj.SalesPanelPages;
using LottoSend.com.Helpers;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver), WayToPay.Offline, "SuperLotto Plus")]
    [TestFixture(typeof(ChromeDriver), WayToPay.InternalBalance, "SuperEnalotto")]
    class BuyGroupOneDrawTicketTests<TWebDriver> where TWebDriver : IWebDriver, new() 
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private OrderVerifications _orderVerifications;
        private WayToPay _merchant;
        private double _totalPrice;
        private TestsSharedCode _sharedCode;

        public BuyGroupOneDrawTicketTests(WayToPay merchant, string lottery)
        {
            _merchant = merchant;

            SetUp();
            _sharedCode = new TestsSharedCode(_driver);

            try
            {
                Buy_Group_One_Draw_Ticket(merchant, lottery);
            }
            catch (Exception e)
            {
                CleanUp();
                _sharedCode.CleanCartIfTestWasFailed();
                throw new Exception("Exception was thrown while executing: " + e.Message + " ");
            }
            CleanUp();
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test, Property("Priority", "Critical")]
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
        [Test]
        [Category("Critical")]
        public void Check_Amount_In_Transaction_Back()
        {
            _orderVerifications.CheckAmountInTransactions_Back(_totalPrice, _driverCover.Login, _driverCover.Password, 1);
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
        /// Performs once before all other tests. Buys a regular single ticket 
        /// </summary>
        public void Buy_Group_One_Draw_Ticket(WayToPay merchant, string lottery)
        {
            _commonActions.SignIn_in_admin_panel();

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            Add_Ticket_To_Cart(lottery);

            //Pays for tickets in the cart (offline or internal balance).
            _totalPrice = _commonActions.PayForTicketsInCart_SalesPanel(_merchant);
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
            group.AddShareToTicket(1, 1);
            group.ClickAddToCartButton();
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _orderVerifications = new OrderVerifications(_driver);
        }
    }
}
