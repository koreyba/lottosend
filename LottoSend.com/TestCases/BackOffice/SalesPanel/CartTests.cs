using LottoSend.com.BackEndObj.SalesPanelPages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver))]
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(InternetExplorerDriver))]
    class CartTests<TWebDriver> where TWebDriver : IWebDriver, new() 
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BackOfficeVerifications _backOfficeVerifications;

        /// <summary>
        /// Adds and removes a regular ticket from the cart
        /// </summary>
        [Test]
        public void Remove_Raffle_Ticket_From_Cart()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");
            MenuObj menu = new MenuObj(_driver);
            menu.GoToLotteryPage("Loteria de Navidad");

            RafflePageObj raffle = new RafflePageObj(_driver);
            raffle.AddShareToTicket(5, 2);
            raffle.ClickAddToCartButton();

            CartObj cart = new CartObj(_driver);
            Assert.AreEqual(2, cart.NumberOfTickets, "Number of tickets in the cart is wrong");

            cart.DeleteTicket("raffle");
            Assert.AreEqual(0, cart.NumberOfTickets, "Number of tickets in the cart is wrong");
        }

        /// <summary>
        /// Adds and removes a regular ticket from the cart
        /// </summary>
        [Test]
        public void Remove_Regular_Ticket_From_Cart()
        {
            //TODO: it is hardcoded for a specific lottery

            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");
            MenuObj menu = new MenuObj(_driver);
            menu.GoToLotteryPage("Mega Millions");
            GroupGameObj group = new GroupGameObj(_driver);
            group.SwitchToSingleTab();

            SingleGameObj game = new SingleGameObj(_driver);
            game.AddToCart();

            CartObj cart = new CartObj(_driver);
            Assert.AreEqual(1, cart.NumberOfTickets, "Number of tickets in the cart is wrong");

            cart.DeleteTicket("Mega Millions");
            Assert.AreEqual(0, cart.NumberOfTickets, "Number of tickets in the cart is wrong");
        }

        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
            {
                //Removes all tickets from the cart to make sure all other tests will work well
                _commonActions.DeleteAllTicketFromCart_SalesPanel();
            }
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _backOfficeVerifications = new BackOfficeVerifications(_driver);
        }
    }
}
