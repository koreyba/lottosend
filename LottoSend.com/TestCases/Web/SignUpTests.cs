using System;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.Helpers;

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
        private TestFramework.CommonActions _commonActions;
        private string _email;
        private WebUserVerifications _usersVerifications;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Signs up a new user on sign_up2 page and checks if the user is logged in
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Sign_Up_Page_Two()
        {
            _email = _commonActions.Sign_Up_Front_PageTwo();
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
            _commonActions = new TestFramework.CommonActions(_driver);
            _usersVerifications = new WebUserVerifications(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
