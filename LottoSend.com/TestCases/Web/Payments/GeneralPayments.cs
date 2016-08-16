using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.FrontEndObj.Common;
using TestFramework.FrontEndObj.GamePages;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.Web.Payments
{
    /// <summary>
    /// Tests for successful and pending payments
    /// </summary>
    [TestFixture(typeof(ChromeDriver))]
    public class GeneralPayments <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private TestFramework.CommonActions _commonActions;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Makes a successful payment payment (buys a ticket) and checks URL
        /// </summary>
        /// <param name="merchant"></param>
        [TestCase(WayToPay.Offline)]
        [TestCase(WayToPay.Neteller)]
        [TestCase(WayToPay.TrustPay)]
        public void Check_URL_Of_Successful_Payment(WayToPay merchant)
        {
            _commonActions.Log_In_Front_PageOne(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/oz-lotto?tab=single");

            RegularGamePageObj regulaerTicket = new RegularGamePageObj(_driver);
            regulaerTicket.ClickBuyTicketsButton();

            MerchantsObj merchants = new MerchantsObj(_driver);

            switch (merchant)
            {
                case WayToPay.Offline:
                    {
                        merchants.PayWithOfflineCharge();
                        _driverCover.OpenNewTab();
                        _commonActions.SignIn_in_admin_panel();
                        _commonActions.Authorize_the_first_payment();
                        _commonActions.Approve_offline_payment();
                        _driverCover.SwitchToTab(1);
                        _driverCover.RefreshPage();
                    }
                    break;

                case WayToPay.Neteller:
                    {
                        merchants.Pay(WayToPay.Neteller);
                    }
                    break;

                case WayToPay.TrustPay:
                    {
                        merchants.Pay(WayToPay.TrustPay);
                    }
                    break;
            }

            StringAssert.Contains("success", _driverCover.Driver.Url);
        }

        /// <summary>
        /// Makes a payment payment (buys a ticket) and checks URL
        /// </summary>
        /// <param name="merchant"></param>
        [TestCase(WayToPay.Offline)]
        public void Check_URL_Of_Pending_Payment(WayToPay merchant)
        {
            _commonActions.Log_In_Front_PageOne(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/oz-lotto?tab=single");

            RegularGamePageObj regulaerTicket = new RegularGamePageObj(_driver);
            regulaerTicket.ClickBuyTicketsButton();

            MerchantsObj merchants = new MerchantsObj(_driver);

            switch (merchant)
            {
                case WayToPay.Offline:
                    {
                        merchants.PayWithOfflineCharge();
                    }
                    break;
            }

            StringAssert.Contains("pending", _driverCover.Driver.Url);
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
            _commonActions = new TestFramework.CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
