using System;
using LottoSend.com.FrontEndObj;
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
    /// Includes tests of the cart (front-mobile)
    /// </summary>
    [TestFixture("Apple iPhone 4")]
    [Parallelizable(ParallelScope.Fixtures)]
    //[TestFixture("Apple iPhone 6")]
    //[TestFixture("Apple iPhone 5")]
    //[TestFixture("Samsung Galaxy S4")]
    //[TestFixture("Samsung Galaxy Note II")]
    public class CartTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private string _device;
        private CartVerifications _cartVerifications;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;

        public CartTests(string device)
        {
            _device = device;
        }

        /// <summary>
        /// Adds a regular ticket to the cart and changes number of draws to play. Then edits the ticket and checks if correct number of draws to play is selected.
        /// </summary>
        /// <param name="toSignIn"></param>
        [TestCase(true)]
        [TestCase(false)]
        public void Change_Number_Of_Draws_In_The_Cart_For_Regular_Ticket(bool toSignIn)
        {
            if (toSignIn)
            {
                _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
            }

            _commonActions.AddRegularTicketToCart_Front("en/play/el-gordo-de-la-primitiva/");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts");

            CartObj cart = new CartObj(_driver);
            cart.ChangeNumberOfDraws(1, 10);

            cart.EditTicket_Mobile("El Gordo");

            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);

            Assert.AreEqual(10, groupGame.NumberOfDraws, "Sorry but number of draw on the play page is not as it was selected in the cart. ");

            if (toSignIn)
            {
                _commonActions.DeleteAllTicketFromCart_Front();
            }
        }

        /// <summary>
        /// Adds a group ticket to the cart and changes number of draws to play. Then edits the ticket and checks if correct number of draws to play is selected.
        /// </summary>
        /// <param name="toSignIn"></param>
        [TestCase(true, 5)]
        [TestCase(false, 5)]
        public void Change_Number_Of_Draws_In_The_Cart_For_Group_Ticket(bool toSignIn, int drawsAmount)
        {
            if (toSignIn)
            {
                _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
            }

            _commonActions.AddGroupTicketToCart_Front("en/play/superenalotto/");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts");

            CartObj cart = new CartObj(_driver);
            cart.ChangeNumberOfDraws(1, drawsAmount);

            cart.EditTicket_Mobile("superenalotto");

            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);

            Assert.AreEqual(drawsAmount, groupGame.NumberOfDraws, "Sorry but number of draw on the play page is not as it was selected in the cart. ");

            if (toSignIn)
            {
                _commonActions.DeleteAllTicketFromCart_Front();
            }
        }

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
                _commonActions.Log_In_Front(_driverCover.LoginFour, _driverCover.Password);
            }
        
            _commonActions.AddGroupTicketToCart_Front("en/play/powerball/");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts");
            CartObj cart = new CartObj(_driver);
            cart.EditTicket_Mobile("Powerball");

            GroupGamePageObj groupPage = new GroupGamePageObj(_driver);

            //add 3 shares to the second ticket
            groupPage.AddShares(2);
            groupPage.ClickBuyTicketsButton();

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
                _commonActions.Log_In_Front(_driverCover.LoginFour, _driverCover.Password);
            }

            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts");

            CartObj cart = new CartObj(_driver);
            cart.EditRaffleTicketMobile("Cart Raffle");

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
        public void Delete_Raffle_ticket_from_cart(bool toLogIn)
        {
            if (toLogIn)
            {
                _commonActions.Log_In_Front(_driverCover.LoginFour, _driverCover.Password);
            }

            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");
            _cartVerifications.CheckNumberOfTicketsInCart_Front(1);

            //Remove tickets
            CartObj cart = new CartObj(_driver);
            cart.DeleteRaffleTicket_Mobile("Cart Raffle");

            _cartVerifications.CheckIfTicketIsNotInCart("Cart Raffle");
            _cartVerifications.CheckNumberOfTicketsInCart_Front(0);
        }

        /// <summary>
        /// Adds two different lottery group ticket to the cart and deletes them. Checks if were added and deleted
        /// </summary>
        [TestCase(false)]
        [TestCase(true)]
        [Category("Critical")]
        [Category("Parallel")]
        public void Delete_two_group_ticket_from_cart(bool toLogIn)
        {
            if (toLogIn)
            {
                _commonActions.Log_In_Front(_driverCover.LoginFour, _driverCover.Password);
            }

            //Add two tickets from different lotteries
            _commonActions.AddGroupTicketToCart_Front("en/play/euromillions/");
            _commonActions.AddGroupTicketToCart_Front("en/play/powerball/");

            _cartVerifications.CheckNumberOfTicketsInCart_Front(2);

            //Remove tickets
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket_Mobile("EuroMillions");
            cart.DeleteTicket_Mobile("Powerball");

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
                _commonActions.Log_In_Front(_driverCover.LoginFour, _driverCover.Password);
            }

            _commonActions.AddRegularTicketToCart_Front("en/play/eurojackpot/");

            _cartVerifications.CheckNumberOfTicketsInCart_Front(1);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts/");
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket_Mobile("EuroJackpot");

            _cartVerifications.CheckIfTicketIsNotInCart("EuroJackpot");
            _cartVerifications.CheckNumberOfTicketsInCart_Front(0);
        }

        /// <summary>
        /// Creates and returns ChromeOptions for a mobile device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public ChromeOptions CreateOptions(string device)
        {
            ChromeOptions options = new ChromeOptions();
            options.EnableMobileEmulation(device);
            return options;
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
                //Removes all tickets from the cart to make sure all other tests will work well
                _commonActions.DeleteAllTicketFromCart_Front();

                _sharedCode.CleanUp(ref _driver);
            }
        }

        [SetUp]
        public void SetUp()
        {
            MessageConsoleCreator message = new MessageConsoleCreator();
            message.TestWillRun();
            _driver = new ChromeDriver(CreateOptions(_device));
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _cartVerifications = new CartVerifications(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }

    }
}
