using System;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
            SetUp(CreateOptions(_device));
            // sign up    
            _commonActions.Sign_Up_Mobile();
            _commonActions.AddGroupTicketToCart_Front("en/play/euromiliony/");
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
            _commonActions.AddGroupTicketToCart_Front("en/play/euromiliony/");
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

            Assert.AreEqual(Math.Round(subTotalPrice - disc - (subTotalPrice - disc) / 100 * discount, 2), Math.Round(price, 2), "Sorry but the total price is not as expected. Maybe coupon gave a wrong discount. ");
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
                    _commonActions.DeleteAllTicketFromCart_Front();
                }
                catch (Exception)
                {
                    _sharedCode.CleanUp(ref _driver);
                }

                _sharedCode.CleanUp(ref _driver);
            }
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
            MessageConsoleCreator message = new MessageConsoleCreator();
            message.TestWillRun();
            _driver = new ChromeDriver(option);
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }

        public void SetUp()
        {
            MessageConsoleCreator message = new MessageConsoleCreator();
            message.TestWillRun();
            _driver = new ChromeDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
