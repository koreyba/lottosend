using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.GamePages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases
{
    [TestFixture]
    public class CartTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;

        /// <summary>
        /// Adds and edit a group ticket adding more shares and checking if they were added
        /// </summary>
        [Test]
        public void Edit_Group_Ticket_And_Add_More()
        {
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
            Add_group_ticket_to_cart("en/plays/powerball/");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts");
            CartObj cart = new CartObj(_driver);
            cart.EditTicket("Powerball");

            GroupGamePageObj groupPage = new GroupGamePageObj(_driver);

            //add 3 shares to the second ticket
            groupPage.AddShares(2);
            groupPage.ClickAddToCartButton();

            Check_number_of_tickets_in_cart(3);
        }

        /// <summary>
        /// Adds and edit raffle ticket adding more shares and checking if they were added
        /// </summary>
        [Test]
        public void Edit_Raffle_Ticket_And_Add_More()
        {
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
            Add_Raffle_Ticket_to_cart();

            CartObj cart = new CartObj(_driver);
            cart.EditTicket("Cart Raffle");

            RafflesPageObj raffle = new RafflesPageObj(_driver);

            //add 3 shares to the second ticket
            raffle.AddShares(3, 2);
            raffle.ClickBuyNowButton();

            Check_number_of_tickets_in_cart(4);
        }

        /// <summary>
        /// Adds a raffle ticket to the cart and deletes it. Cheks if a ticket was added and removed
        /// </summary>
        [Test]
        public void Delete_Raffle_ticket_from_cart()
        {
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            Add_Raffle_Ticket_to_cart();
            Check_number_of_tickets_in_cart(1);

            //Remove tickets
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket("Cart Raffle");

            Check_if_ticket_is_not_in_cart("Cart Raffle");
            Check_number_of_tickets_in_cart(0);
        }

        /// <summary>
        /// Adds two different lottery group ticket to the cart and deletes them. Checks if were added and deleted
        /// </summary>
        [Test]
        public void Delete_two_group_ticket_from_cart()
        {
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            //Add two tickets from different lotteries
            Add_group_ticket_to_cart("en/plays/euromillions/");
            Add_group_ticket_to_cart("en/plays/powerball/");

            Check_number_of_tickets_in_cart(2);

            //Remove tickets
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket("EuroMillions");
            cart.DeleteTicket("Powerball");

            //Check if tickets are still present
            Check_if_ticket_is_not_in_cart("EuroMillions");
            Check_if_ticket_is_not_in_cart("Powerball");

            Check_number_of_tickets_in_cart(0);
        }

        private void Add_Raffle_Ticket_to_cart()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/");
            RafflesPageObj raffle = new RafflesPageObj(_driver);

            raffle.ClickBuyNowButton();
        }

        private void Check_number_of_tickets_in_cart(int expected)
        {
            //Go to the cart 
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts/");

            //Check number of elemetns in the cart
            CartObj cart = new CartObj(_driver);
            Assert.AreEqual(expected, cart.NumberOfTickets, "There are " + cart.NumberOfTickets + " group tickets in the cart, but " + expected + " were expected ");
        }

        private void Add_group_ticket_to_cart(string adress)
        {
            //Add ticket to the cart
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + adress);
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);
            groupGame.ClickAddToCartButton();
        }

        /// <summary>
        /// Adds single ticket to cart and removes it. Checks if there is no tickets of a specific lottery game
        /// </summary>
        [Test]
        public void Delete_Single_Ticket_From_Cart()
        {
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            Add_Regular_Ticket_To_Cart("en/plays/eurojackpot/");

            Check_number_of_tickets_in_cart(1);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts/");
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket("EuroJackpot");

            Check_if_ticket_is_not_in_cart("EuroJackpot");
            Check_number_of_tickets_in_cart(0);

        }

        private void Add_Regular_Ticket_To_Cart(string address)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + address);
            RegularGamePageObj game = new RegularGamePageObj(_driver);
            game.ClickStandartGameButton();

            game.ClickAddToCartButton();
        }

        private void Check_if_ticket_is_not_in_cart(string lotteryName)
        {
            CartObj cart = new CartObj(_driver);
            Assert.IsTrue(!cart.IsTicketInCart(lotteryName), "The ticket " + lotteryName + " was not removed from the cart ");
        }

        [TearDown]
        public void CleanUp()
        {
            //Removes all tickets from the cart to make sure all other tests will work well
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts/");
            CartObj cart = new CartObj(_driver);
            cart.DeleteAllTickets();

            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
        }
    }
}
