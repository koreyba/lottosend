﻿using System;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.WebUsersPages;
using TestFramework.FrontEndObj.Common;
using TestFramework.FrontEndObj.GamePages;
using TestFramework.FrontEndObj.Login;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.Web
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    public class LogInTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private WebUserVerifications _usersVerifications;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;
        private TestFramework.CommonActions _commonActions;

        [TestCase("selenium3@grr.la")]
        public void Login_With_Token(string email)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/web_users");
            WebUsersPageObj webusers = new WebUsersPageObj(_driver);
            string token = webusers.GetUserToken(email);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "?auth_token=" + token);

            _usersVerifications.CheckIfSignedIn_Web();
        }

        /// <summary>
        /// Logs in express checkout on a game page
        /// </summary>
      //  [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Login_Express_Checkout_Game_Page()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/eurojackpot/");

            //Pay for tickets
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            //Go to single tab
            regularGame.ClickStandartGameButton();
            regularGame.ClickBuyTicketsButton();

            ExpressCheckoutObj checkout = new ExpressCheckoutObj(_driver);
            checkout.SignIn(_driverCover.Login, _driverCover.Password);

            _usersVerifications.CheckIfSignedIn_Web();
        }

        /// <summary>
        /// Logs in express checkout in the cart
        /// </summary>
       // [Test]
        public void Login_Express_Checkout_In_Cart()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/eurojackpot/");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);

            //CartObj cart = rafflePage.ClickBuyNowButton();
            //cart.ClickProceedToCheckoutButton();

            ExpressCheckoutObj checkout = new ExpressCheckoutObj(_driver);
            checkout.SignIn(_driverCover.Login, _driverCover.Password);

            _usersVerifications.CheckIfSignedIn_Web();
        }

        /// <summary>
        /// Logs in on sign_in page 
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Login_On_SignIn_Page_One()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "web-users/sign-in");
            SignInPageOneObj signInOne = new SignInPageOneObj(_driver);

            signInOne.FillInFields(_driverCover.Login, _driverCover.Password);
            TopBarObj topBar = signInOne.ClickLogInButton();

            _usersVerifications.CheckIfSignedIn_Web();
        }

        /// <summary>
        /// Logs in pup up form and checks in user logged in
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Login_In_PopUp_Form()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "lotteries/results/");
            TopBarObj topBar = new TopBarObj(_driver);
            LogInPopUpObj loginPopUp = topBar.ClickLogInButton();

            loginPopUp.FillInFields(_driverCover.Login, _driverCover.Password);
            topBar = loginPopUp.ClickLogInButton();

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
            _usersVerifications = new WebUserVerifications(_driver);
            _sharedCode = new TestsSharedCode(_driver);
            _commonActions = new TestFramework.CommonActions(_driver);
        }
    }
}
