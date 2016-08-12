using System;
using NewCombinedPageConfigTests.CommonActions;
using NewCombinedPageConfigTests.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.FrontEndObj.Cart;
using TestFramework.Helpers;

namespace NewCombinedPageConfigTests.TestCases.Web
{
    /// <summary>
    /// Includes tests of the cart (front)
    /// </summary>
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
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
                _commonActions.Log_In_Front_PageOne(_driverCover.LoginFour, _driverCover.Password);
            }

            _commonActions.AddRegularTicketToCart_Front("en/play/eurojackpot/");

            _cartVerifications.CheckNumberOfTicketsInCombinedCart_Front(1);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/payments/new/");
            CartObj cart = new CartCombinedWebObj(_driver);
            cart.DeleteTicket("EuroJackpot");
            _cartVerifications.CheckNumberOfTicketsInCombinedCart_Front(0);
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
