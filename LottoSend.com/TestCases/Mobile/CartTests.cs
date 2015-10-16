using System.Collections.Generic;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace LottoSend.com.TestCases.Mobile
{
    /// <summary>
    /// Includes tests of the cart (front-mobile)
    /// </summary>
    [TestFixture("Apple iPhone 4")]
    [TestFixture("Apple iPhone 6")]
    [TestFixture("Apple iPhone 5")]
    [TestFixture("Samsung Galaxy S4")]
    [TestFixture("Samsung Galaxy Note II")]
    public class CartTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private string _device;
        private CartVerifications _cartVerifications;

        public CartTests(string device)
        {
            _device = device;
        }

        /// <summary>
        /// Adds and edit a group ticket adding more shares and checking if they were added
        /// </summary>
        [Test]
        public void Edit_Group_Ticket_And_Add_More()
        {
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
            _commonActions.AddGroupTicketToCart("en/play/powerball/");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts");
            CartObj cart = new CartObj(_driver);
            cart.EditTicket_Mobile("Powerball");

            GroupGamePageObj groupPage = new GroupGamePageObj(_driver);

            //add 3 shares to the second ticket
            groupPage.AddShares(2);
            groupPage.ClickBuyTicketsButton();

            _cartVerifications.CheckNumberOfTicketsInCart(3);

            _commonActions.DeleteAllTicketFromCart_Front();
        }

        /// <summary>
        /// Adds and edit raffle ticket adding more shares and checking if they were added
        /// </summary>
        [Test]
        public void Edit_Raffle_Ticket_And_Add_More()
        {
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
            _commonActions.AddRaffleTicketToCart();

            CartObj cart = new CartObj(_driver);
            cart.EditRaffleTicketMobile("Cart Raffle");

            RafflesPageObj raffle = new RafflesPageObj(_driver);

            //add 3 shares to the second ticket
            raffle.AddShares(3, 2);
            raffle.ClickBuyNowButton();

            _cartVerifications.CheckNumberOfTicketsInCart(4);

            _commonActions.DeleteAllTicketFromCart_Front();
        }

        /// <summary>
        /// Adds a raffle ticket to the cart and deletes it. Cheks if a ticket was added and removed
        /// </summary>
        [Test]
        public void Delete_Raffle_ticket_from_cart()
        {
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _commonActions.AddRaffleTicketToCart();
            _cartVerifications.CheckNumberOfTicketsInCart(1);

            //Remove tickets
            CartObj cart = new CartObj(_driver);
            cart.DeleteRaffleTicket_Mobile("Cart Raffle");

            _cartVerifications.CheckIfTicketIsNotInCart("Cart Raffle");
            _cartVerifications.CheckNumberOfTicketsInCart(0);
        }

        /// <summary>
        /// Adds two different lottery group ticket to the cart and deletes them. Checks if were added and deleted
        /// </summary>
        [Test]
        public void Delete_two_group_ticket_from_cart()
        {
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            //Add two tickets from different lotteries
            _commonActions.AddGroupTicketToCart("en/play/euromillions/");
            _commonActions.AddGroupTicketToCart("en/play/powerball/");

            _cartVerifications.CheckNumberOfTicketsInCart(2);

            //Remove tickets
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket_Mobile("EuroMillions");
            cart.DeleteTicket_Mobile("Powerball");

            //Check if tickets are still present
            _cartVerifications.CheckIfTicketIsNotInCart("EuroMillions");
            _cartVerifications.CheckIfTicketIsNotInCart("Powerball");

            _cartVerifications.CheckNumberOfTicketsInCart(0);
        }

        /// <summary>
        /// Adds single ticket to cart and removes it. Checks if there is no tickets of a specific lottery game
        /// </summary>
        [Test]
        public void Delete_Single_Ticket_From_Cart()
        {
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _commonActions.AddRegularTicketToCart("en/play/eurojackpot/");

            _cartVerifications.CheckNumberOfTicketsInCart(1);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts/");
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket_Mobile("EuroJackpot");

            _cartVerifications.CheckIfTicketIsNotInCart("EuroJackpot");
            _cartVerifications.CheckNumberOfTicketsInCart(0);
        }

        private ChromeOptions CreateOptions(string device)
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
            if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
            {
                //Removes all tickets from the cart to make sure all other tests will work well
                _commonActions.DeleteAllTicketFromCart_Front();
            }
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver(CreateOptions(_device));
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _cartVerifications = new CartVerifications(_driver);
        }

    }
}
