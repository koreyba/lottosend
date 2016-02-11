using LottoSend.com.BackEndObj.SalesPanelPages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    public class SignInUpTests<TWebDriver> where TWebDriver : IWebDriver, new() 
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BackOfficeVerifications _backOfficeVerifications;
        private bool _setUpFailed = false;

        [Test]
        [Category("Critical")]
        [Category("Test")]
        [Category("Parallel")]
        public void Sign_Up_In_Sales_Panel()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");
            RegisterObj register = new RegisterObj(_driver);
            register.SignUp(RandomGenerator.GenerateRandomString(7) + "@grr.la");

            _backOfficeVerifications.IfUserSignedInSalesPanel();
        }
        
        [Test]
        public void Sign_In_Sales_Panel()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            RegisterObj register = new RegisterObj(_driver);
            register.SignIn(_driverCover.Login);
            _backOfficeVerifications.IfUserSignedInSalesPanel();
            register.SignOut();

            register.SignIn(40540);
            _backOfficeVerifications.IfUserSignedInSalesPanel();
        }

        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
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
            _backOfficeVerifications = new BackOfficeVerifications(_driver);
        }
    }
}
