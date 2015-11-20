using LottoSend.com.TestCases;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.Helpers
{
    /// <summary>
    /// Includes code shared between different classes of tests cases
    /// </summary>
    public class TestsSharedCode
    {
        private IWebDriver _driver;
        public TestsSharedCode(IWebDriver driver)
        {
            _driver = driver;
            OrderVerifications = new OrderVerifications(_driver);
            CommonActions = new CommonActions(_driver);
        }

        private DriverCover _driverCover;

        public CommonActions CommonActions;
        public OrderVerifications OrderVerifications;
        

        /// <summary>
        /// If a test was failed or inconclusive then the user's cart will be cleaned up
        /// </summary>
        public void CleanCartIfTestWasFailed()
        {
            if (TestContext.CurrentContext.Result.Status == TestStatus.Failed || TestContext.CurrentContext.Result.State == TestState.Inconclusive)
            {
                SetUp();
                CommonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
                //Removes all tickets from the cart to make sure all other tests will work well
                CommonActions.DeleteAllTicketFromCart_Front();
                CleanUp(ref _driver);
            }
        }

        /// <summary>
        /// Dispose WebDriver and pushes erros to console from OrderVerifications class
        /// </summary>
        public void CleanUp(ref IWebDriver driver)
        {
            driver.Dispose();
            if (OrderVerifications.Errors.Length > 0)
            {
                Assert.Fail(OrderVerifications.Errors.ToString());
            }
        }

        private void SetUp()
        {
            _driver = new ChromeDriver();
            _driverCover = new DriverCover(_driver);
        }
    }
}
