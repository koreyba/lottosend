using System;
using System.Threading;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.FrontEndObj.Common;
using TestFramework.FrontEndObj.GamePages;
using TestFramework.FrontEndObj.Login;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.Mobile
{
    [TestFixture("Apple iPhone 4")]
    [Parallelizable(ParallelScope.Fixtures)]
    //[TestFixture("Apple iPhone 6")]
    //[TestFixture("Apple iPhone 5")]
    //[TestFixture("Samsung Galaxy S4")]
    //[TestFixture("Samsung Galaxy Note II")]
    public class LogInTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private WebUserVerifications _usersVerifications;
        private string _device;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;

        public LogInTests(string device)
        {
            _device = device;
        }

        /// <summary>
        /// Logs in express checkout on a game page
        /// </summary>
      //  [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Login_Express_Checkout_Game_Page()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/eurojackpot/");

            //Pay for tickets
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            //Go to single tab
            regularGame.ClickStandartGameButton();
            regularGame.ClickBuyTicketsButton();

            ExpressCheckoutObj checkout = new ExpressCheckoutObj(_driver);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            checkout.SignIn(_driverCover.Login, _driverCover.Password);

            _usersVerifications.CheckIfSignedIn_Mobile();
        }

        /// <summary>
        /// Logs in express checkout in the cart
        /// </summary>
       // [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Login_Express_Checkout_In_Cart()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/" + "");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);

            //CartObj cart = rafflePage.ClickBuyNowButton();
            //cart.ClickProceedToCheckoutButton();

            ExpressCheckoutObj checkout = new ExpressCheckoutObj(_driver);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            checkout.SignIn(_driverCover.Login, _driverCover.Password);

            _usersVerifications.CheckIfSignedIn_Mobile();
        }

        /// <summary>
        /// Logs in on sign_in page 
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Login_On_SignIn_Page_One()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "web-users/sign-in");
            SignInPageOneObj signInOne = new SignInPageOneObj(_driver);

            signInOne.FillInFields(_driverCover.Login, _driverCover.Password);
            TopBarObj topBar = signInOne.ClickLogInButton();

            _usersVerifications.CheckIfSignedIn_Mobile();
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
            _driver = new ChromeDriver(CreateOptions(_device));
            _driverCover = new DriverCover(_driver);
            _usersVerifications = new WebUserVerifications(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
