using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace LottoSend.com.TestCases.Web
{
    /// <summary>
    /// Tests that are connected with failed payments
    /// </summary>
    /// <typeparam name="TWebDriver"></typeparam>
    [TestFixture(typeof(ChromeDriver))]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    class FailedPaymentTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private CartVerifications _cartVerifications;

        /// <summary>
        /// Checks if a raffle ticket stays in the cart after payment was failed
        /// </summary>
        [Test]
        public void If_Raffle_Ticket_Stays_In_Cart_When_Payment_Was_Failed()
        {
            // Log in     
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);

            CartObj cart = rafflePage.ClickBuyNowButton();
            MerchantsObj merchants = cart.ClickProceedToCheckoutButton();

            merchants.PayWithOfflineCharge();

            //Go to admin panel
            _commonActions.SignIn_in_admin_panel();

            //authorize payment in charge panel
            _commonActions.Authorize_the_first_payment();

            //approve payment
            _commonActions.Fail_offline_payment();

            _cartVerifications.CheckIfTicketIsInCart("Cart Raffle");

            _commonActions.DeleteAllTicketFromCart_Front();
        }

        /// <summary>
        /// Checks if a regular ticket stays in the cart after payment was failed
        /// </summary>
        [Test]
        public void If_Regular_Ticket_Stays_In_Cart_When_Payment_Was_Failed()
        {
            // Log in     
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/eurojackpot/");

            //Pay for tickets
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            //Go to single tab
            regularGame.ClickStandartGameButton();

            MerchantsObj merchants = regularGame.ClickBuyTicketsButton();
            merchants.PayWithOfflineCharge();

            //Go to admin panel
            _commonActions.SignIn_in_admin_panel();

            //authorize payment in charge panel
            _commonActions.Authorize_the_first_payment();

            //approve payment
            _commonActions.Fail_offline_payment();

            _cartVerifications.CheckIfTicketIsInCart("EuroJackpot");

            _commonActions.DeleteAllTicketFromCart_Front();
        }

        /// <summary>
        /// Checks if a grup ticket stays in the cart after payment was failed
        /// </summary>
        [Test]
        public void If_Group_Ticket_Stays_In_Cart_When_Payment_Was_Failed()
        {
            // Log in     
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/powerball/");

            //Pay for tickets
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);

            MerchantsObj merchants = groupGame.ClickBuyTicketsButton();
            merchants.PayWithOfflineCharge();

            //Go to admin panel
            _commonActions.SignIn_in_admin_panel();

            //authorize payment in charge panel
            _commonActions.Authorize_the_first_payment();

            //approve payment
            _commonActions.Fail_offline_payment();

            _cartVerifications.CheckIfTicketIsInCart("Powerball");

            _commonActions.DeleteAllTicketFromCart_Front();
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _cartVerifications = new CartVerifications(_driver);
        }
    }
}
