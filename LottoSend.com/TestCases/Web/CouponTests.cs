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

        [TestCase("Denis666", 50)]
        [Category("Critical")]
        public void Check_Discount_Checkout(string code, double discount)
        {
            // sign up    
            _commonActions.Sign_Up_Front();
            _commonActions.AddGroupTicketToCart_Front("en/play/euro-miliony-slovakia/");
            _commonActions.AddRegularTicketToCart_Front("en/play/el-gordo-de-la-primitiva/");
            _commonActions.AddRaffleTicketToCart_Front("en/raffles/loteria-de-navidad/");

            CartObj cart = new CartObj(_driver);  
            cart.ClickProceedToCheckoutButton();
            CheckoutObj checkout = new CheckoutObj(_driver);
            checkout.ApplyCoupon(code);
            double subTotalPrice = checkout.SubTotalPrice;
            double price = checkout.TotalPrice;
            double disc = checkout.DiscountMultiDraw;

            Assert.AreEqual(subTotalPrice - disc - (subTotalPrice - disc) / 100 * discount, price);
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
