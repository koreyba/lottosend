using System;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.WebUsersPages;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.Web.Payments
{
    [TestFixture(typeof(ChromeDriver))]
    class CombinedPaymentsTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BalanceVerifications _balanceVerifications;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Pays for a regular, group and raffle tickets using internal balance (both wallets) + offline payment. Checks if after this payment the user have correct balance (30$ as promotion)
        /// </summary>
        [TestCase(WayToPay.Offline)]
        [TestCase(WayToPay.Neteller)]
        [TestCase(WayToPay.TrustPay)]
        public void Combined_IB_Plus_Merchant_Pay_For_Tickets(WayToPay merchant)
        {
            string email = _commonActions.Sign_Up_Front_PageOne();
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/web_users");

            WebUsersPageObj users = new WebUsersPageObj(_driver);

            users.FilterByEmail(email);
            users.CreditToRealMoney("3");
            users.CreditToStoreCredit("5");

            _commonActions.AddRegularTicketToCart_Front("en/play/superenalotto");
            _commonActions.AddGroupTicketToCart_Front("en/play/superenalotto");

            if (merchant == WayToPay.Offline)
            {
                _commonActions.BuyRaffleTicket_Front(WayToPay.Offline);
            }

            if (merchant == WayToPay.Neteller)
            {
                _commonActions.BuyRaffleTicket_Front(WayToPay.Neteller);
            }

            if (merchant == WayToPay.TrustPay)
            {
                _commonActions.BuyRaffleTicket_Front(WayToPay.TrustPay);
            }

            _balanceVerifications.CheckBalanceOnDepositPage_Web(30);
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
                //Removes all tickets from the cart to make sure all other tests will work well
                try
                {
                    _commonActions.DeleteAllTicketFromCart_Front();
                }
                catch (Exception)
                {
                    _sharedCode.CleanUp(ref _driver);
                }

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
            _sharedCode = new TestsSharedCode(_driver);
            _commonActions = new CommonActions(_driver);
            _balanceVerifications = new BalanceVerifications(_driver);
        }
    }
}
