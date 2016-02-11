using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace LottoSend.com.TestCases.BackOffice
{
  //  [TestFixture(typeof(ChromeDriver), WayToPay.Offline)]
   // [TestFixture(typeof(ChromeDriver), WayToPay.Neteller)]
    public class OrderProcessingTest <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private WayToPay _merchant;

        public OrderProcessingTest(WayToPay merchant)
        {
            _merchant = merchant;
        }


        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                _driverCover.TakeScreenshot();
            }
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
        }
    }
}
