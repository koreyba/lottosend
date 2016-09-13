using System;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.SalesPanelPages;
using TestFramework.BackEndObj.WebUsersPages;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver))]
    public class InternalBalanceTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private TestFramework.CommonActions _commonActions;
        private TestsSharedCode _sharedCode;
        private BalanceVerifications _balanceVerifications;

        /// <summary>
        /// Checks if store credit balance is reduced when you buys a ticket via the sales panel using store credit
        /// </summary>
        [Test]
        public void If_Store_Credit_Is_Reduced_After_Purchasing()
        {
            string email = "fkoreybadenis@gmail.com";
            _commonActions.SignIn_in_admin_panel();
            
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/web_users");

            WebUsersPageObj users = new WebUsersPageObj(_driver);
            users.FilterByEmail(email);
            users.CreditToStoreCredit("20");
            double balance = users.StoreCredit;

            _commonActions.Sign_In_SalesPanel(email);
            _commonActions.AddRegularOneDrawTicketToCart_SalesPanel("EuroMillions");
            CartObj cart = new CartObj(_driver);
            double price = cart.TotalPrice;

            _commonActions.PayForTicketsInCart_SalesPanel(WayToPay.InternalBalance);

            _balanceVerifications.CheckUserStoreCredit_BackOffice(email, balance - price);
        }

        /// <summary>
        /// Checks if store credit balance is reduced when you buys a ticket via the sales panel using store credit
        /// </summary>
        [Test]
        public void If_Real_Money_Are_Reduced_After_Purchasing()
        {
            string email = "fqqsctytpqwv@grr.la";
            _commonActions.SignIn_in_admin_panel();

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/web_users");

            WebUsersPageObj users = new WebUsersPageObj(_driver);
            users.FilterByEmail(email);
            users.CreditToRealMoney("20");
            double balance = users.RealMoney;

            _commonActions.Sign_In_SalesPanel(email);
            _commonActions.AddRegularOneDrawTicketToCart_SalesPanel("EuroMillions");
            CartObj cart = new CartObj(_driver);
            double price = cart.TotalPrice;

            _commonActions.PayForTicketsInCart_SalesPanel(WayToPay.InternalBalance);

            _balanceVerifications.CheckUserSRealMoney_BackOffice(email, balance - price);
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
            _balanceVerifications = new BalanceVerifications(_driver);
        }
    }
}
