using LottoSend.com.BackEndObj;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
