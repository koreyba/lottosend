using System;
using LottoSend.com.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    class CouponTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private TestsSharedCode _sharedCode;

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
            _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@urk.net");

            _commonActions.AddGroupTicketToCart_SalesPanel("Powerball");
            _commonActions.AddRegularOneDrawTicketToCart_SalesPanel("SuperLotto Plus");
            _commonActions.AddRaffleTicketToCart_SalesPanel("Loteria de Navidad");

            BackEndObj.SalesPanelPages.CartObj cart = _commonActions.ApplyCouponInCart_SalesPanel(code);
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
            _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@urk.net");

            _commonActions.AddRaffleTicketToCart_SalesPanel("Loteria de Navidad");

            BackEndObj.SalesPanelPages.CartObj cart = new BackEndObj.SalesPanelPages.CartObj(_driver);
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
        public void Check_Discount_Checkout(string code, double discount)
        {
            _commonActions.SignIn_in_admin_panel();
            _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@urk.net");

            _commonActions.AddGroupTicketToCart_SalesPanel("Powerball");
            _commonActions.AddRegularOneDrawTicketToCart_SalesPanel("SuperLotto Plus");
            _commonActions.AddRaffleTicketToCart_SalesPanel("Loteria de Navidad");

            BackEndObj.SalesPanelPages.CartObj cart = new BackEndObj.SalesPanelPages.CartObj(_driver);
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
