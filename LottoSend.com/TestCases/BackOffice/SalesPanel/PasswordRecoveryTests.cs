﻿using System;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.SalesPanelPages;
using TestFramework.Helpers;


namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    public class PasswordRecoveryTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private TestFramework.CommonActions _commonActions;
        private WebUserVerifications _usersVerifications;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Signs up a new user via the sales panel and changes its password. Then logs in on the front with this password
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Recover_Password()
        {
            _commonActions.SignIn_in_admin_panel();

            string email = "User_" + RandomGenerator.GenerateRandomString(6) + "@grr.la";
            string password = "11111111";

            _commonActions.Sign_Up_SalesPanel(email);

            TabsObj tabs = new TabsObj(_driver);
            tabs.GoToResetPasswordTab();

            ResetPasswordTabObj recoveryPage = new ResetPasswordTabObj(_driver);
            recoveryPage.ChangePassword(password);

            _commonActions.Log_In_Front_PageOne(email, password);
            _usersVerifications.CheckIfSignedIn_Web();
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
            _usersVerifications = new WebUserVerifications(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
