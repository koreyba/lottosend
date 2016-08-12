using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.ClientOrderPricessing;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.BackOffice
{
    [TestFixture(typeof(ChromeDriver))]
    public class OrderProcessingTest <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;

        [TestCase(ChargeBackStatus.CHB)]
        [TestCase(ChargeBackStatus.CHBR)]
        [TestCase(ChargeBackStatus.RR)]
        public void Make_ChargeBack(ChargeBackStatus status)
        {
            _commonActions.Log_In_Front_PageOne(_driverCover.Login, _driverCover.Password);
            _commonActions.BuyRegularOneDrawTicket_Front(WayToPay.Offline);
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders_processed");
            OrderProcessingObj orderProcessing = new OrderProcessingObj(_driver);
            orderProcessing.ClickChargeBackButton();
            ChargeBackFormObj chargeBackForm = new ChargeBackFormObj(_driver);
            chargeBackForm.ChargeBack(status);

            StringAssert.Contains(status.ToString().ToLower(), orderProcessing.ChargeBackImageText.ToLower());
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
            _commonActions = new CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
