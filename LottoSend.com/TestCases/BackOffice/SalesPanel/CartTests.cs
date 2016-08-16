﻿using System;
using LottoSend.com.Verifications;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TestFramework;
using TestFramework.BackEndObj.SalesPanelPages;
using TestFramework.Helpers;

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
        private TestFramework.CommonActions _commonActions;
        private CartVerifications _cartVerifications;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Adds two different lottery group ticket to the cart and delets them. Checks if were added and deleted
        /// </summary>
        /// <param name="lotteryOne"></param>
        /// <param name="lotteryTwo"></param>
        [TestCase("El Gordo", "SuperLotto Plus", true)]
        [TestCase("El Gordo", "SuperLotto Plus", false)]
        [Category("Critical")]
        [Category("Parallel")]
        public void Remove_Two_Group_Tickets_From_Cart(string lotteryOne, string lotteryTwo, bool toLogIn)
        {
            _commonActions.SignIn_in_admin_panel();

            if (toLogIn)
            {
                _commonActions.Sign_In_SalesPanel(_driverCover.LoginSix);
            }
            
            _commonActions.AddGroupBulkBuyTicketToCart_SalesPanel(lotteryOne);
            _commonActions.AddGroupBulkBuyTicketToCart_SalesPanel(lotteryTwo);

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
        [TestCase("test", true)]
        [TestCase("test", false)]
        [Category("Critical")]
        [Category("Parallel")]
        public void Remove_Raffle_Ticket_From_Cart(string raffleName, bool toLogIn)
        {
            _commonActions.SignIn_in_admin_panel();

            if (toLogIn)
            {
                _commonActions.Sign_In_SalesPanel(_driverCover.LoginSix);
            }

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
        [TestCase("Mega Millions", true)]
        [TestCase("Mega Millions", false)]
        [Category("Critical")]
        [Category("Parallel")]
        public void Remove_Regular_Ticket_From_Cart(string lottery, bool toLogIn)
        {
            _commonActions.SignIn_in_admin_panel();

            if (toLogIn)
            {
                _commonActions.Sign_In_SalesPanel(_driverCover.LoginSix);
            }

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");
            MenuObj menu = new MenuObj(_driver);
            menu.GoToLotteryPage(lottery);
            GroupGameObj group = new GroupGameObj(_driver);
            group.SwitchToSingleTab();

            RegularGameObj game = new RegularGameObj(_driver);
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
                _commonActions.DeleteAllTicketFromCart_SalesPanel();
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
        }
    }
}
