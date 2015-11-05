using LottoSend.com.BackEndObj.ChargePanelPages;
using LottoSend.com.BackEndObj.SalesPanelPages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver), WayToPay.Offline, "El Gordo")]
    [TestFixture(typeof(ChromeDriver), WayToPay.InternalBalance, "EuroMillions")]
    class BuyRegularOneDrawTicketTest<TWebDriver> where TWebDriver : IWebDriver, new() 
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private OrderVerifications _orderVerifications;
        private WayToPay _merchant;
        private double _totalPrice;

        public BuyRegularOneDrawTicketTest(WayToPay merchant, string lottery)
        {
            _merchant = merchant;

            SetUp();
            Buy_Regular_One_Draw_Ticket(merchant, lottery);
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
            _orderVerifications.CheckAmountInTransactions_Back(_totalPrice, _driverCover.Login, _driverCover.Password, 1);
        }


        /// <summary>
        /// Performs once before all other tests. Buys a regular single ticket 
        /// </summary>
        public void Buy_Regular_One_Draw_Ticket(WayToPay merchant, string lottery)
        {
            _commonActions.SignIn_in_admin_panel();

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            Pay_For_Tickets_In_Cart(lottery); //if merchant is internal balance then ticket will be bought

            if (_merchant == WayToPay.Offline) //if merchant is offline it needs approvement
            {
                ChargePanelObj chargePanel = new ChargePanelObj(_driver);
                chargePanel.ChargeTheLastPayment();
            }
        }

        /// <summary>
        /// Pays for tickets in the cart (offline or internal balance)
        /// </summary>
        /// <param name="lotteryName"></param>
        private void Pay_For_Tickets_In_Cart(string lotteryName)
        {
            Add_Ticket_To_Cart(lotteryName);

            CartObj cart = new CartObj(_driver);
            _totalPrice = cart.TotalBalance;

            if (_merchant == WayToPay.Offline)
            {
                TabsObj tabs = new TabsObj(_driver);
                tabs.GoToCcDetailsTab();
                CcDetailsObj form = new CcDetailsObj(_driver);
                form.InputCcDetails("VISA", "4580458045804580");

                cart.Charge();
            }

            if (_merchant == WayToPay.InternalBalance)
            {
                cart.PayWithInternalBalance();
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
                regForm.SignIn("selenium@gmail.com");
            }

            if (_merchant == WayToPay.InternalBalance)
            {
                 regForm.SignIn("selenium2@gmail.com");
            }

            MenuObj menu = new MenuObj(_driver);
            menu.GoToLotteryPage(lotteryName);
            GroupGameObj group = new GroupGameObj(_driver);
            @group.SwitchToSingleTab();

            SingleGameObj game = new SingleGameObj(_driver);
            game.AddToCart();
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
