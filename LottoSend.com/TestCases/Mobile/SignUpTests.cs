using System;
using System.Collections.Generic;
using System.Threading;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Mobile
{
    [TestFixture("Apple iPhone 4")]
    [Parallelizable(ParallelScope.Fixtures)]
    //[TestFixture("Apple iPhone 6")]
    //[TestFixture("Apple iPhone 5")]
    //[TestFixture("Samsung Galaxy S4")]
    //[TestFixture("Samsung Galaxy Note II")]
    public class SignUpTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private string _email;
        private WebUserVerifications _usersVerifications;
        private string _device;

        public SignUpTests(string device)
        {
            _device = device;
        }

        /// <summary>
        /// Signs up in express checkout in the cart
        /// </summary>
        [Test]
        [Category("Critical")]
        public void SignUp_Express_Checkout_In_Cart()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);

            CartObj cart = rafflePage.ClickBuyNowButton();
            cart.ClickProceedToCheckoutButton();

            ExpressCheckoutObj checkout = new ExpressCheckoutObj(_driver);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            checkout.SignUp_Mobile();

            _usersVerifications.CheckIfSignedIn_Mobile();
        }

        /// <summary>
        /// Signs up in express checkout on a game page
        /// </summary>
        [Test]
        [Category("Critical")]
        public void SignUp_Express_Checkout_Game_Page()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/eurojackpot/");

            //Pay for tickets
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            //Go to single tab
            regularGame.ClickStandartGameButton();
            regularGame.ClickBuyTicketsButton();

            ExpressCheckoutObj checkout = new ExpressCheckoutObj(_driver);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            checkout.SignUp_Mobile();

            _usersVerifications.CheckIfSignedIn_Mobile();
        }

        /// <summary>
        /// Checks if user exists in the backoffice - web_users page
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Check_Back_Office_User()
        {
            _email = _commonActions.Sign_Up_Mobile();
            _usersVerifications.CheckIfSignedIn_Mobile();
            _usersVerifications.CheckBackOfficeUser(_email);
        }

        /// <summary>
        /// Creates and returns ChromeOptions for a mobile device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public ChromeOptions CreateOptions(string device)
        {
            var mobileEmulation = new Dictionary<string, string>
            {
                {"deviceName", device}
            };

            ChromeOptions options = new ChromeOptions();
            options.AddAdditionalCapability("mobileEmulation", mobileEmulation);
            return options;
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver(CreateOptions(_device));
            _driverCover = new DriverCover(_driver);
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _usersVerifications = new WebUserVerifications(_driver);
        }
    }
}
