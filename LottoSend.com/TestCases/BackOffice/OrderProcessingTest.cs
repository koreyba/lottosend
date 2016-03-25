using System;
using LottoSend.com.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace LottoSend.com.TestCases.BackOffice
{
  //  [TestFixture(typeof(ChromeDriver), WayToPay.Offline)]
   // //[TestFixture(typeof(ChromeDriver), WayToPay.Neteller)]
    public class OrderProcessingTest <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private WayToPay _merchant;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;

        public OrderProcessingTest(WayToPay merchant)
        {
            _merchant = merchant;
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
