using System;
using System.Globalization;
using System.Net.Mime;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.WebUsersPages;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.BackOffice
{

    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    public class BackOfficeTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private TestFramework.CommonActions _commonActions;
        private TestsSharedCode _sharedCode;

        [Test]
        public void If_Bulk_Buys_Are_Not_Stuck()
        { 
            BackOfficeVerifications back = new BackOfficeVerifications(_driver);
            back.IfPastDrawDateIsPresent();
        }

        [Test]
        public void Create_Web_User_BackOffice()
        {
            _commonActions.SignIn_in_admin_panel();

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/web_users/new");

            NewWebUserPageObj newUser = new NewWebUserPageObj(_driver);
            string email = newUser.CreateWebUser();

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/web_users");

            WebUsersPageObj users = new WebUsersPageObj(_driver);

            Assert.IsTrue(users.FindUserOnCurrentPage(email), "Sorry but a user with email: " + email + " must have not been created. Check it ");
        }

        [TearDown]
        public void CleanUp()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
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
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
