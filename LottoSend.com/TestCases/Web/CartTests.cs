﻿using System;
using LottoSend.com.CommonActions;
using LottoSend.com.Verifications;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TestFramework;
using TestFramework.FrontEndObj.Cart;
using TestFramework.FrontEndObj.GamePages;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.Web
{
    /// <summary>
    /// Includes tests of the cart (front)
    /// </summary>
    [TestFixture(typeof(ChromeDriver))]
    //[TestFixture(typeof(FirefoxDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    public class CartTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private TestFramework.CommonActions _commonActions;
        private CartVerifications _cartVerifications;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;
        private CartActions _cartActions;

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
                _commonActions.Log_In_Front_PageOne(_driverCover.Login, _driverCover.Password);
            }

            _commonActions.AddRegularTicketToCart_Front("en/play/el-gordo-de-la-primitiva/");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts");

            CartObj cart = new CartSiteObj(_driver);
            cart.ChangeNumberOfDraws(1, 10);

            cart.EditTicket("El Gordo");

            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            Assert.AreEqual(10, regularGame.NumberOfDraws, "Sorry but number of draw on the play page is not as it was selected in the cart. ");

            if (toSignIn)
            {
                _cartActions.DeleteAllTicketFromCart_Front();
            }
        }

        /// <summary>
        /// Adds a group ticket to the cart and changes number of draws to play. Then edits the ticket and checks if correct number of draws to play is selected.
        /// </summary>
        /// <param name="toSignIn"></param>
        [TestCase(true)]
        [TestCase(false)]
        public void Change_Number_Of_Draws_In_The_Cart_For_Group_Ticket(bool toSignIn)
        {
            if (toSignIn)
            {
                _commonActions.Log_In_Front_PageOne(_driverCover.Login, _driverCover.Password);
            }

            _commonActions.AddGroupTicketToCart_Front("en/play/superenalotto/");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts");

            CartObj cart = new CartSiteObj(_driver);
            cart.ChangeNumberOfDraws(1, 5);

            cart.EditTicket("superenalotto");

            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);

            Assert.AreEqual(5, groupGame.NumberOfDraws, "Sorry but number of draw on the play page is not as it was selected in the cart. ");

            if (toSignIn)
            {
                _cartActions.DeleteAllTicketFromCart_Front();
            }
        }

        /// <summary>
        /// Checks that items in the cart is not removed when a guest signs up on sign up 1 page
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Tickets_Are_Left_In_Cart_After_Sign_Up_PageOne()
        {
             /*
             * Being a guest add tickets to the cart
             * Sing up
             * Check if tickets in the cart are left
             */
            _commonActions.AddRegularTicketToCart_Front("en/play/powerball/");
            _commonActions.AddGroupTicketToCart_Front("en/play/eurojackpot/");
            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            _commonActions.Sign_Up_Front_PageOne();
            _cartVerifications.CheckNumberOfTicketsInCart_Front(3);
        }
        
        /// <summary>
        /// Checks that items in the cart is not removed when a guest logs in on login 1 page
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Tickets_Are_Left_In_Cart_After_Log_In_PageOne()
        {
            /*
             * Being a guest add tickets to the cart
             * Sing in
             * Check if tickets in the cart are left
             */

            _commonActions.AddRegularTicketToCart_Front("en/play/powerball/");
            _commonActions.AddGroupTicketToCart_Front("en/play/eurojackpot/");
            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            _commonActions.Log_In_Front_PageOne(_driverCover.LoginThree, _driverCover.Password);
            _cartVerifications.CheckNumberOfTicketsInCart_Front(3);
            _cartActions.DeleteAllTicketFromCart_Front();
        }

        /// <summary>
        /// Checks that items in the cart is not removed when a guest logs in on login 2 page
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Tickets_Are_Left_In_Cart_After_Log_In_PageTwo()
        {
            /*
             * Being a guest add tickets to the cart
             * Sing in
             * Check if tickets in the cart are left
             */

            _commonActions.AddRegularTicketToCart_Front("en/play/powerball/");
            _commonActions.AddGroupTicketToCart_Front("en/play/eurojackpot/");
            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            _commonActions.Log_In_Front_PageTwo(_driverCover.LoginThree, _driverCover.Password);
            _cartVerifications.CheckNumberOfTicketsInCart_Front(3);
            _cartActions.DeleteAllTicketFromCart_Front();
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
                _commonActions.Log_In_Front_PageOne(_driverCover.LoginThree, _driverCover.Password);
            }
            
            _commonActions.AddGroupTicketToCart_Front("en/play/powerball/");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts");
            CartObj cart = new CartSiteObj(_driver);
            cart.EditTicket("Powerball");

            GroupGamePageObj groupPage = new GroupGamePageObj(_driver);

            //add 3 shares to the second ticket
            groupPage.AddShares(2);
            groupPage.ClickAddToCartButton();

            _cartVerifications.CheckNumberOfTicketsInCart_Front(3);

            _cartActions.DeleteAllTicketFromCart_Front();
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
                _commonActions.Log_In_Front_PageOne(_driverCover.LoginThree, _driverCover.Password);
            }

            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/test/");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts");

            CartObj cart = new CartSiteObj(_driver);
            cart.EditTicket("Cart Raffle");

            RafflesPageObj raffle = new RafflesPageObj(_driver);

            //add 3 shares to the second ticket
            raffle.AddShares(3, 1);
            raffle.ClickBuyNowButton();

            _cartVerifications.CheckNumberOfTicketsInCart_Front(4);

            _cartActions.DeleteAllTicketFromCart_Front();
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
                _commonActions.Log_In_Front_PageOne(_driverCover.LoginThree, _driverCover.Password);
            }

            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            //Remove tickets
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts/");
            CartObj cart = new CartSiteObj(_driver);
            cart.DeleteTicket("Cart Raffle");
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
                _commonActions.Log_In_Front_PageOne(_driverCover.LoginThree, _driverCover.Password);
            }

            //Add two tickets from different lotteries
            _commonActions.AddGroupTicketToCart_Front("en/play/euromillions/");
            _commonActions.AddGroupTicketToCart_Front("en/play/powerball/");

            _cartVerifications.CheckNumberOfTicketsInCart_Front(2);

            //Remove tickets
            CartObj cart = new CartSiteObj(_driver);
            cart.DeleteTicket("EuroMillions");
            cart.DeleteTicket("Powerball");

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
                _commonActions.Log_In_Front_PageOne(_driverCover.LoginThree, _driverCover.Password);
            }

            _commonActions.AddRegularTicketToCart_Front("en/play/eurojackpot/");

            _cartVerifications.CheckNumberOfTicketsInCart_Front(1);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts/");
            CartObj cart = new CartSiteObj(_driver);
            cart.DeleteTicket("EuroJackpot");
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
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                //Removes all tickets from the cart to make sure all other tests will work well
                try
                {
                    _cartActions.DeleteAllTicketFromCart_Front();
                }
                catch (Exception)
                {
                    _sharedCode.CleanUp(ref _driver);
                }

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
            _commonActions = new TestFramework.CommonActions(_driver);
            _cartVerifications = new CartVerifications(_driver);
            _sharedCode = new TestsSharedCode(_driver);
            _cartActions = new CartActions(_driver);
        }
    }
}
