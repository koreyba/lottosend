using System;
using System.Threading;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Helpers;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Web
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    public class SignUpTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private string _email;
        private WebUserVerifications _usersVerifications;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Signs up in express checkout in the cart
        /// </summary>
        //[Test]
        public void SignUp_Express_Checkout_In_Cart()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);

            //CartObj cart = rafflePage.ClickBuyNowButton();
            //cart.ClickProceedToCheckoutButton();

            ExpressCheckoutObj checkout = new ExpressCheckoutObj(_driver);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            checkout.SignUp_Web();

            _usersVerifications.CheckIfSignedIn_Web();
        }

        /// <summary>
        /// Signs up in express checkout on a game page
        /// </summary>
       // [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void SignUp_Express_Checkout_Game_Page()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/eurojackpot/");

            //Pay for tickets
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            //Go to single tab
            regularGame.ClickStandartGameButton();
            regularGame.ClickBuyTicketsButton();

            ExpressCheckoutObj checkout = new ExpressCheckoutObj(_driver);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            checkout.SignUp_Web();

            _usersVerifications.CheckIfSignedIn_Web();
        }

        /// <summary>
        /// Checks if user exists in the backoffice - web_users page
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Check_Back_Office_User()
        {
            _email = _commonActions.Sign_Up_Front_PageOne();
            _usersVerifications.CheckBackOfficeUser(_email);
        }

         /// <summary>
        /// Sign up in pop up form
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Sign_Up_In_Pop_Up()
         {
             _commonActions.Sign_Up_In_Pop_Up_Front();
            _usersVerifications.CheckIfSignedIn_Web();
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
            _usersVerifications = new WebUserVerifications(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
