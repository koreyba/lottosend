using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Web
{
    [TestFixture(typeof(ChromeDriver))]
    class CouponTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private WayToPay _merchant;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Applies a coupon and removes it. Checks if total price is the same as before applying 
        /// </summary>
        /// <param name="code"></param>
        [TestCase("Denis666")]
        [Category("Critical")]
        public void Cancel_Coupon(string code)
        {
            // sign up    
            _commonActions.Sign_Up_Front();
            _commonActions.AddGroupTicketToCart_Front("en/play/euro-miliony-slovakia/");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts/");

            CartObj cart = new CartObj(_driver);
            double totalPrice = cart.TotalPrice;

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
        public void Check_Discount_Checkout(string code, double discount)
        {
            // sign up    
            _commonActions.Sign_Up_Front();
            _commonActions.AddGroupTicketToCart_Front("en/play/euro-miliony-slovakia/");
            _commonActions.AddRegularTicketToCart_Front("en/play/el-gordo-de-la-primitiva/");
            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            CheckoutObj checkout = _commonActions.ApplyCouponInCart_Web(code);
            double subTotalPrice = checkout.SubTotalPrice;
            double price = checkout.TotalPrice;
            double disc = checkout.DiscountMultiDraw;

            Assert.AreEqual(subTotalPrice - disc - (subTotalPrice - disc) / 100 * discount, price, "Sory, but coupon is probably applied wrong ");
        }

        [TearDown]
        public void CleanUp()
        {
            _sharedCode.CleanUp(ref _driver);
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
