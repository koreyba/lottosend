using System;
using LottoSend.com.BackEndObj.SalesPanelPages;
using LottoSend.com.Helpers;
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

        [TestCase(70677)]
        public void Sign_In_Sales_Panel(int ID)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            RegisterObj register = new RegisterObj(_driver);
            register.SignIn(_driverCover.LoginSix);
            _backOfficeVerifications.IfUserSignedInSalesPanel();
            register.SignOut();

            register.SignIn(ID);
            _backOfficeVerifications.IfUserSignedInSalesPanel();
        }

        [TearDown]
        public void CleanUp()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
                {
                    _driverCover.TakeScreenshot();
                    //Removes all tickets from the cart to make sure all other tests will work well
                    _commonActions.DeleteAllTicketFromCart_Front();
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                MessageConsoleCreator message = new MessageConsoleCreator();
                message.DriverDisposed();
                _driver.Dispose();
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
            _backOfficeVerifications = new BackOfficeVerifications(_driver);
        }
    }
}
