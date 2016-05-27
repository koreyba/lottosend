using System;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Web
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    class CouponTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private WayToPay _merchant;
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
            // sign up    
            _commonActions.Sign_Up_Front_PageOne();
            _commonActions.AddGroupTicketToCart_Front("en/play/euromiliony/");
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
            // sign up    
            _commonActions.Sign_Up_Front_PageOne();
            _commonActions.AddGroupTicketToCart_Front("en/play/euromiliony/");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts/");

            CartObj cart = new CartObj(_driver);
            double totalPrice = cart.TotalPrice_Front;

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
            // sign up    
            _commonActions.Sign_Up_Front_PageOne();
            _commonActions.AddGroupTicketToCart_Front("en/play/euromiliony/");
            _commonActions.AddRegularTicketToCart_Front("en/play/el-gordo-de-la-primitiva/");
            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts");

            CheckoutObj checkout = _commonActions.ApplyCouponInCart_Web(code);
            double subTotalPrice = checkout.SubTotalPrice;
            double price = checkout.TotalPrice;
            double disc = checkout.DiscountMultiDraw;

            Assert.AreEqual(Math.Round(subTotalPrice - disc - (subTotalPrice - disc) / 100 * discount, 2), Math.Round(price, 2), "Sory, but coupon is probably applied wrong ");
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
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
