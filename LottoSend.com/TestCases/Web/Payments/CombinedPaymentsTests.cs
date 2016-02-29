using LottoSend.com.BackEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.Helpers;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
        [Test]
        public void IB_Plus_OfflinePayment_Pay_For_Tickets()
        {
            string email = _commonActions.Sign_Up_Front();
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/web_users");

            WebUsersPageObj users = new WebUsersPageObj(_driver);

            users.FilterByEmail(email);
            users.CreditToRealMoney("3");
            users.CreditToStoreCredit("5");

            _commonActions.AddRegularTicketToCart_Front("en/play/superenalotto");
            _commonActions.AddGroupTicketToCart_Front("en/play/superenalotto");
            _commonActions.AddRaffleTicketToCart_Front(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            MerchantsObj merchants = new MerchantsObj(_driver);
            merchants.PayWithOfflineCharge();
            _commonActions.SignIn_in_admin_panel();
            _commonActions.Authorize_the_first_payment();
            _commonActions.Approve_offline_payment();

            _balanceVerifications.CheckBalanceOnDepositPage_Web(30);
        }

        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                _driverCover.TakeScreenshot();
            }
            _sharedCode.CleanUp(ref _driver);
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _sharedCode = new TestsSharedCode(_driver);
            _commonActions = new CommonActions(_driver);
            _balanceVerifications = new BalanceVerifications(_driver);
        }
    }
}
