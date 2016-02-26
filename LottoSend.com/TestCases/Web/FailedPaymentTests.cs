using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
        private bool _setUpFailed = false;

        /// <summary>
        /// Fails payment and check if URL displayes the word "failure"
        /// </summary>
        /// <param name="merchant"></param>
        [TestCase("ekonto")]
        [Category("Critical")]
        public void Fail_Online_Pament_Check_URL(string merchant)
        {
            //TODO
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);

            rafflePage.ClickBuyNowButton();

            _driverCover.Driver.FindElement(By.CssSelector("input[id$='merchant_23'] + img.merchant")).Click();
            OnlineMerchantsObj online = new OnlineMerchantsObj(_driver);
            online.FailPayment();
            StringAssert.Contains("failure", _driverCover.Driver.Url);
        }

        /// <summary>
        /// Checks if a raffle ticket stays in the cart after payment was failed
        /// </summary>
        [Test]
        [Category("Critical")]
        public void If_Raffle_Ticket_Stays_In_Cart_When_Payment_Was_Failed()
        {
            // Log in     
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);

            rafflePage.ClickBuyNowButton();//
            MerchantsObj merchants = new MerchantsObj(_driver);

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
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
            {
                _driverCover.TakeScreenshot();
            }
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
