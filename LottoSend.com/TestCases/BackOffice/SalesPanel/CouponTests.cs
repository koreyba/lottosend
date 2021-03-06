﻿using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.SalesPanelPages;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    class CouponTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private TestFramework.CommonActions _commonActions;
        private TestsSharedCode _sharedCode;
        private bool _setUpFailed = false;

        /// <summary>
        /// Pays for tickets using coupon with 100% discount (Completes order)
        /// </summary>
        /// <param name="code"></param>
        [TestCase("ForFree100")]
        [Category("Critical")]
        [Category("Parallel")]
        public void Pay_With_Free_Coupon(string code)
        {
            _commonActions.SignIn_in_admin_panel();
            _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            _commonActions.AddGroupBulkBuyTicketToCart_SalesPanel("Mega Millions");
            _commonActions.AddRegularOneDrawTicketToCart_SalesPanel("SuperLotto Plus");
            _commonActions.AddRaffleTicketToCart_SalesPanel("Loteria de Navidad");

            CartObj cart = _commonActions.ApplyCouponInCart_SalesPanel(code);
            cart.PayWithInternalBalance();
        }

        /// <summary>
        /// Applies a coupon and removes it. Checks if total price is the same as before applying 
        /// </summary>
        /// <param name="code"></param>
        [TestCase("Denis666")]
        [Category("Critical")]
        [Category("Parallel")]
        public void Cancel_Coupon(string code)
        {
            _commonActions.SignIn_in_admin_panel();
            _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            _commonActions.AddRaffleTicketToCart_SalesPanel("Loteria de Navidad");

            CartObj cart = new CartObj(_driver);
            double totalPrice = cart.TotalPrice; //take price before applying a coupon

            cart = _commonActions.ApplyCouponInCart_SalesPanel(code); //apply a coupon

            cart.CancelCoupon(); //reomove the coupon

            Assert.AreEqual(totalPrice, cart.TotalPrice, "Sorry but coupon is probably not removed ");
        }

        /// <summary>
        /// Applies a coupon and checks if total price is correct (calculates using coupon discount and multi-draw discount)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="discount"></param>
        [TestCase("Denis666", 50)]
        [Category("Critical")]
        [Category("Parallel")]
        [Category("Test")]
        public void Check_Discount_Checkout(string code, double discount)
        {
            _commonActions.SignIn_in_admin_panel();
            _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            _commonActions.AddGroupBulkBuyTicketToCart_SalesPanel("Powerball");
            _commonActions.AddRegularOneDrawTicketToCart_SalesPanel("SuperLotto Plus");
            _commonActions.AddRaffleTicketToCart_SalesPanel("Loteria de Navidad");

            CartObj cart = new CartObj(_driver);
            double subTotalPrice = cart.TotalPrice;

            cart = _commonActions.ApplyCouponInCart_SalesPanel(code);
            double price = cart.TotalPrice;
            double discountPrice = Math.Round(subTotalPrice - subTotalPrice/100*discount, 2);
            double expectedPrice = Math.Round(subTotalPrice - discountPrice, 2);
            
            Assert.AreEqual(expectedPrice, price, "Sory, but coupon is probably applied wrong ");
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
                    _commonActions.DeleteAllTicketFromCart_SalesPanel();
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
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
