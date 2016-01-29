using System;
using System.Collections.Generic;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace LottoSend.com.TestCases.Mobile
{
    [TestFixture("Apple iPhone 4")]
    [Parallelizable(ParallelScope.Fixtures)]
    class CouponTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private string _device;
        private TestsSharedCode _sharedCode;
        private bool _setUpFailed = false;

        public CouponTests(string device)
        {
            _device = device;
        }

        /// <summary>
        /// Pays for tickets using coupon with 100% discount (Completes order)
        /// </summary>
        /// <param name="code"></param>
        [TestCase("ForFree100")]
        [Category("Critical")]
        [Category("Parallel")]
        public void Pay_With_Free_Coupon(string code)
        {
            SetUp(CreateOptions(_device));

            // sign up    
            _commonActions.Sign_Up_Mobile();
            _commonActions.AddGroupTicketToCart_Front("en/play/euro-miliony-slovakia/");
            _commonActions.AddRegularTicketToCart_Front("en/play/megamillions/");
            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts");
            CheckoutObj checkout = _commonActions.ApplyCouponInCart_Web(code);
            checkout.ClickCompleteYourOrderButton();

            Assert.IsTrue(_driverCover.Driver.Url.Contains("payments/success"));
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
            SetUp(CreateOptions(_device));
            // sign up    
            _commonActions.Sign_Up_Mobile();
            _commonActions.AddGroupTicketToCart_Front("en/play/euro-miliony-slovakia/");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts/");

            CartObj cart = new CartObj(_driver);
            double totalPrice = cart.TotalPrice_Mobile;

            CheckoutObj checkout = _commonActions.ApplyCouponInCart_Web(code);
            checkout.RemoveCoupon();

            Assert.AreEqual(totalPrice, checkout.TotalPrice, "Sorry but coupon is probably not removed ");
        }

        /// <summary>
        /// Applies a coupon and checks if total price is correct (calculates using coupon discount and multi-draw discount)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="discount"></param>
        [TestCase("Denis666", 50)]
        [Category("Critical")]
        [Category("Parallel")]
        public void Check_Discount_Checkout(string code, double discount)
        {
            SetUp(CreateOptions(_device));
            // sign up    
            _commonActions.Sign_Up_Mobile();
            _commonActions.AddGroupTicketToCart_Front("en/play/euro-miliony-slovakia/");
            _commonActions.AddRegularTicketToCart_Front("en/play/el-gordo-de-la-primitiva/");
            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts");
            CartObj cart = new CartObj(_driver);  
            cart.ClickProceedToCheckoutButton();
            CheckoutObj checkout = new CheckoutObj(_driver);
            checkout.ApplyCoupon(code);
            double subTotalPrice = checkout.SubTotalPrice;
            double price = checkout.TotalPrice;
            double disc = checkout.DiscountMultiDraw;

            Assert.AreEqual(Math.Round(subTotalPrice - disc - (subTotalPrice - disc) / 100 * discount, 2), Math.Round(price, 2));
        }

        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
            {
                _driverCover.TakeScreenshot();
            }
            _sharedCode.CleanUp(ref _driver);
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

        public void SetUp(ChromeOptions option)
        {
            _driver = new ChromeDriver(option);
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }

        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
