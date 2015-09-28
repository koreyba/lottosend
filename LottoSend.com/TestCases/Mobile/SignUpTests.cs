using System.Collections.Generic;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Mobile
{
    [TestFixture("Apple iPhone 4")]
    [TestFixture("Apple iPhone 6")]
    [TestFixture("Apple iPhone 5")]
    [TestFixture("Samsung Galaxy S4")]
    [TestFixture("Samsung Galaxy Note II")]
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
        /// Checks if user exists in the backoffice - web_users page
        /// </summary>
        [Test]
        public void Check_Back_Office_User()
        {
            _usersVerifications.CheckBackOfficeUser(_email);
        }

        /// <summary>
        /// Signs up
        /// </summary>
        [TestFixtureSetUp]
        public void Sign_Up()
        {
            SetUp(CreateOptions(_device));
            _email = _commonActions.Sign_Up_Mobile();
            _usersVerifications.CheckIfSignedIn_Web();
            CleanUp();
        }

        private ChromeOptions CreateOptions(string device)
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

        public void SetUp(ChromeOptions option)
        {
            _driver = new ChromeDriver(option);
            _driverCover = new DriverCover(_driver);
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _usersVerifications = new WebUserVerifications(_driver);
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _usersVerifications = new WebUserVerifications(_driver);
        }
    }
}
