﻿using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace LottoSend.com.TestCases.Web
{
    /// <summary>
    /// Includes tests of the cart (front)
    /// </summary>
    [TestFixture(typeof(ChromeDriver))]
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(InternetExplorerDriver))]
    public class CartTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private CartVerifications _cartVerifications;

        /// <summary>
        /// Adds and edit a group ticket adding more shares and checking if they were added
        /// </summary>
        [Test]
        public void Edit_Group_Ticket_And_Add_More()
        {
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
            _commonActions.AddGroupTicketToCart("play/powerball/");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts");
            CartObj cart = new CartObj(_driver);
            cart.EditTicket_Web("Powerball");

            GroupGamePageObj groupPage = new GroupGamePageObj(_driver);

            //add 3 shares to the second ticket
            groupPage.AddShares(2);
            groupPage.ClickAddToCartButton();

            _cartVerifications.CheckNumberOfTicketsInCart(3);

            _commonActions.DeleteAllTicketFromCart();
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
            cart.EditTicket_Web("Cart Raffle");

            RafflesPageObj raffle = new RafflesPageObj(_driver);

            //add 3 shares to the second ticket
            raffle.AddShares(3, 2);
            raffle.ClickBuyNowButton();

            _cartVerifications.CheckNumberOfTicketsInCart(4);

            _commonActions.DeleteAllTicketFromCart();
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
            cart.DeleteTicket_Web("Cart Raffle");

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
            _commonActions.AddGroupTicketToCart("play/euromillions/");
            _commonActions.AddGroupTicketToCart("play/powerball/");

            _cartVerifications.CheckNumberOfTicketsInCart(2);

            //Remove tickets
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket_Web("EuroMillions");
            cart.DeleteTicket_Web("Powerball");

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

            _commonActions.AddRegularTicketToCart("play/eurojackpot/");

            _cartVerifications.CheckNumberOfTicketsInCart(1);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts/");
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket_Web("EuroJackpot");

            _cartVerifications.CheckIfTicketIsNotInCart("EuroJackpot");
            _cartVerifications.CheckNumberOfTicketsInCart(0);
        }


        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
            {
                //Removes all tickets from the cart to make sure all other tests will work well
                _commonActions.DeleteAllTicketFromCart();
            }
            _driver.Dispose();
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
