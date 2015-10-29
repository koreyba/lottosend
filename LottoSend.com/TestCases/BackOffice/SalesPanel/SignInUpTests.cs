﻿using LottoSend.com.BackEndObj.SalesPanelPages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver))]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    public class SignInUpTests<TWebDriver> where TWebDriver : IWebDriver, new() 
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BackOfficeVerifications _backOfficeVerifications;

        [Test]
        public void Sign_Up_In_Sales_Panel()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");
            RegisterObj register = new RegisterObj(_driver);
            register.SignUp(RandomGenerator.GenerateRandomString(7) + "@ukr.net");

            _backOfficeVerifications.IfUserSignedInSalesPanel();
        }

        [TestCase("selenium@gmail.com", 40540)]
        public void Sign_In_Sales_Panel(string email, int id)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            RegisterObj register = new RegisterObj(_driver);
            register.SignIn(email);
            _backOfficeVerifications.IfUserSignedInSalesPanel();
            register.SignOut();

            register.SignIn(id);
            _backOfficeVerifications.IfUserSignedInSalesPanel();
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
            _backOfficeVerifications = new BackOfficeVerifications(_driver);
        }
    }
}