using System;
using LottoSend.com.BackEndObj.SalesPanelPages;
using LottoSend.com.Verifications;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    class CartTests<TWebDriver> where TWebDriver : IWebDriver, new() 
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BackOfficeVerifications _backOfficeVerifications;
        private CartVerifications _cartVerifications;

        /// <summary>
        /// Adds two different lottery group ticket to the cart and delets them. Checks if were added and deleted
        /// </summary>
        /// <param name="lotteryOne"></param>
        /// <param name="lotteryTwo"></param>
        [TestCase("El Gordo", "SuperLotto Plus")]
        [Category("Critical")]
        [Category("Parallel")]
        public void Remove_Two_Group_Tickets_From_Cart(string lotteryOne, string lotteryTwo)
        {
            _commonActions.SignIn_in_admin_panel();
            _commonActions.AddGroupTicketToCart_SalesPanel(lotteryOne);
            _commonActions.AddGroupTicketToCart_SalesPanel(lotteryTwo);

            _cartVerifications.CheckNumberOfTicketsInCart_SalesPanel(2);

            CartObj cart = new CartObj(_driver);
            Assert.AreEqual(2, cart.NumberOfTickets, "Number of tickets in the cart is wrong");

            cart.DeleteTicket(lotteryOne);
            cart.DeleteTicket(lotteryTwo);

            _cartVerifications.CheckNumberOfTicketsInCart_SalesPanel(0);
        }

        /// <summary>
        /// Adds and removes a regular ticket from the cart
        /// </summary>
        [TestCase("Loteria de Navidad")]
        [Category("Critical")]
        [Category("Parallel")]
        public void Remove_Raffle_Ticket_From_Cart(string raffleName)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");
            MenuObj menu = new MenuObj(_driver);
            menu.GoToLotteryPage(raffleName);

            RafflePageObj raffle = new RafflePageObj(_driver);
            raffle.AddShareToTicket(1, 2);
            raffle.ClickAddToCartButton();

            CartObj cart = new CartObj(_driver);
            Assert.AreEqual(2, cart.NumberOfTickets, "Number of tickets in the cart is wrong");

            cart.DeleteTicket(raffleName);
            cart.DeleteTicket(raffleName);
            Assert.AreEqual(0, cart.NumberOfTickets, "Number of tickets in the cart is wrong");
        }

        /// <summary>
        /// Adds and removes a regular ticket from the cart
        /// </summary>
        [TestCase("Mega Millions")]
        [Category("Critical")]
        [Category("Parallel")]
        public void Remove_Regular_Ticket_From_Cart(string lottery)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");
            MenuObj menu = new MenuObj(_driver);
            menu.GoToLotteryPage(lottery);
            GroupGameObj group = new GroupGameObj(_driver);
            group.SwitchToSingleTab();

            SingleGameObj game = new SingleGameObj(_driver);
            game.AddToCart();

            CartObj cart = new CartObj(_driver);
            Assert.AreEqual(1, cart.NumberOfTickets, "Number of tickets in the cart is wrong");

            cart.DeleteTicket(lottery);
            Assert.AreEqual(0, cart.NumberOfTickets, "Number of tickets in the cart is wrong");
        }

        [TearDown]
        public void CleanUp()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    _driverCover.TakeScreenshot();
                    //Removes all tickets from the cart to make sure all other tests will work well
                    _commonActions.DeleteAllTicketFromCart_Front();
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                _driver.Dispose();
            }
        }

        [SetUp]
        public void SetUp()
        {
            
           // DesiredCapabilities capabilities = new DesiredCapabilities();

           //// 
           // if (typeof (TWebDriver).Name.Equals("ChromeDriver"))
           // {
           //      capabilities = DesiredCapabilities.Chrome();
           // }

           // if (typeof(TWebDriver).Name.Equals("FirefoxDriver"))
           // {
           //      capabilities = DesiredCapabilities.Firefox();
           // }

           // _driver = new RemoteWebDriver(
           //   new Uri("http://localhost:4444/wd/hub"), capabilities
           // );

            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _backOfficeVerifications = new BackOfficeVerifications(_driver);
            _cartVerifications = new CartVerifications(_driver); 
        }
    }
}
