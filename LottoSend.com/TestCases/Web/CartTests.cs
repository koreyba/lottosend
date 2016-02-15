using System;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Verifications;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace LottoSend.com.TestCases.Web
{
    /// <summary>
    /// Includes tests of the cart (front)
    /// </summary>
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    public class CartTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private CartVerifications _cartVerifications;
        private bool _setUpFailed = false;

        /// <summary>
        /// Adds and edit a group ticket adding more shares and checking if they were added
        /// </summary>
        [TestCase(false)]
        [TestCase(true)]
        [Category("Critical")]
        [Category("Parallel")]
        public void Edit_Group_Ticket_And_Add_More(bool toLogIn)
        {
            if (toLogIn)
            {
                _commonActions.Log_In_Front(_driverCover.LoginThree, _driverCover.Password);
            }
            
            _commonActions.AddGroupTicketToCart_Front("en/play/powerball/");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts");
            CartObj cart = new CartObj(_driver);
            cart.EditTicket_Web("Powerball");

            GroupGamePageObj groupPage = new GroupGamePageObj(_driver);

            //add 3 shares to the second ticket
            groupPage.AddShares(2);
            groupPage.ClickAddToCartButton();

            _cartVerifications.CheckNumberOfTicketsInCart_Front(3);

            _commonActions.DeleteAllTicketFromCart_Front();
        }

        /// <summary>
        /// Adds and edit raffle ticket adding more shares and checking if they were added
        /// </summary>
        [TestCase(false)]
        [TestCase(true)]
        [Category("Critical")]
        [Category("Parallel")]
        public void Edit_Raffle_Ticket_And_Add_More(bool toLogIn)
        {
            if (toLogIn)
            {
                _commonActions.Log_In_Front(_driverCover.LoginThree, _driverCover.Password);
            }

            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts");

            CartObj cart = new CartObj(_driver);
            cart.EditTicket_Web("Cart Raffle");

            RafflesPageObj raffle = new RafflesPageObj(_driver);

            //add 3 shares to the second ticket
            raffle.AddShares(3, 2);
            raffle.ClickBuyNowButton();

            _cartVerifications.CheckNumberOfTicketsInCart_Front(4);

            _commonActions.DeleteAllTicketFromCart_Front();
        }

        /// <summary>
        /// Adds a raffle ticket to the cart and deletes it. Cheks if a ticket was added and removed
        /// </summary>
        [TestCase(false)]
        [TestCase(true)]
        [Category("Critical")]
        [Category("Parallel")]
        public void Delete_Raffle_Ticket_From_Cart(bool toLogIn)
        {
            if (toLogIn)
            {
                _commonActions.Log_In_Front(_driverCover.LoginThree, _driverCover.Password);
            }

            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");
            _cartVerifications.CheckNumberOfTicketsInCart_Front(1);

            //Remove tickets
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket_Web("Cart Raffle");

            _cartVerifications.CheckIfTicketIsNotInCart("Cart Raffle");
            _cartVerifications.CheckNumberOfTicketsInCart_Front(0);
        }

        /// <summary>
        /// Adds two different lottery group ticket to the cart and delets them. Checks if were added and deleted
        /// </summary>
        [TestCase(false)]
        [TestCase(true)]
        [Category("Critical")]
        [Category("Parallel")]
        public void Delete_Two_Group_Ticket_From_Cart(bool toLogIn)
        {
            if (toLogIn)
            {
                _commonActions.Log_In_Front(_driverCover.LoginThree, _driverCover.Password);
            }

            //Add two tickets from different lotteries
            _commonActions.AddGroupTicketToCart_Front("en/play/euromillions/");
            _commonActions.AddGroupTicketToCart_Front("en/play/powerball/");

            _cartVerifications.CheckNumberOfTicketsInCart_Front(2);

            //Remove tickets
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket_Web("EuroMillions");
            cart.DeleteTicket_Web("Powerball");

            //Check if tickets are still present
            _cartVerifications.CheckIfTicketIsNotInCart("EuroMillions");
            _cartVerifications.CheckIfTicketIsNotInCart("Powerball");

            _cartVerifications.CheckNumberOfTicketsInCart_Front(0);
        }

        /// <summary>
        /// Adds single ticket to cart and removes it. Checks if there is no tickets of a specific lottery game
        /// </summary>
        [TestCase(false)]
        [TestCase(true)]
        [Category("Critical")]
        [Category("Parallel")]
        public void Delete_Single_Ticket_From_Cart(bool toLogIn)
        {
            if (toLogIn)
            {
                _commonActions.Log_In_Front(_driverCover.LoginThree, _driverCover.Password);
            }

            _commonActions.AddRegularTicketToCart_Front("en/play/eurojackpot/");

            _cartVerifications.CheckNumberOfTicketsInCart_Front(1);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts/");
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket_Web("EuroJackpot");

            _cartVerifications.CheckIfTicketIsNotInCart("EuroJackpot");
            _cartVerifications.CheckNumberOfTicketsInCart_Front(0);
        }


        [TearDown]
        public void CleanUp()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
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
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _cartVerifications = new CartVerifications(_driver);
        }
    }
}
