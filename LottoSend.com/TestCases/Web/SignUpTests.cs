using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.SignUp;
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
    public class SignUpTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private string _email;
        private WebUserVerifications _usersVerifications;

        /// <summary>
        /// Checks if user exists in the backoffice - web_users page
        /// </summary>
        [Test]
        public void Check_Back_Office_User()
        {
            _usersVerifications.CheckBackOfficeUser(_email);
        }

         /// <summary>
        /// Sign up in pop up form
        /// </summary>
        [Test]
        public void Sign_Up_In_Pop_Up()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/");
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/lotteries/results/");

            TopBarObj topBar = new TopBarObj(_driver);
            SignUpPopUpObj popUp = topBar.ClickSignUpButton();
            _email = popUp.FillInFields();
            SignUpSuccessObj success = popUp.ClickSignUp();
            _usersVerifications.CheckIfSignedIn_Web();
        }

        /// <summary>
        /// Signs up
        /// </summary>
        [TestFixtureSetUp]
        public void Sign_Up()
        {
            SetUp();
            _email = _commonActions.Sign_Up();
            CleanUp();
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
            _usersVerifications = new WebUserVerifications(_driver);
        }
    }
}
