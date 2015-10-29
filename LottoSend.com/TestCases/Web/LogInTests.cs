using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.FrontEndObj.Login;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace LottoSend.com.TestCases.Web
{
    [TestFixture(typeof(ChromeDriver))]
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(InternetExplorerDriver))]
    public class LogInTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private WebUserVerifications _usersVerifications;

        /// <summary>
        /// Logs in express checkout on a game page
        /// </summary>
        [Test]
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
        [Test]
        public void Login_Express_Checkout_In_Cart()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/eurojackpot/");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);

            CartObj cart = rafflePage.ClickBuyNowButton();
            cart.ClickProceedToCheckoutButton();

            ExpressCheckoutObj checkout = new ExpressCheckoutObj(_driver);
            checkout.SignIn(_driverCover.Login, _driverCover.Password);

            _usersVerifications.CheckIfSignedIn_Web();
        }

        /// <summary>
        /// Logs in on sign_in page 
        /// </summary>
        [Test]
        public void Login_On_SignIn_Page_One()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "web_users/sign_in");
            SignInPageOneObj signInOne = new SignInPageOneObj(_driver);

            signInOne.FillInFields(_driverCover.Login, _driverCover.Password);
            TopBarObj topBar = signInOne.ClickLogInButton();

            _usersVerifications.CheckIfSignedIn_Web();
        }

        /// <summary>
        /// Logs in pup up form and checks in user logged in
        /// </summary>
        [Test]
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
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _usersVerifications = new WebUserVerifications(_driver);
        }
    }
}
