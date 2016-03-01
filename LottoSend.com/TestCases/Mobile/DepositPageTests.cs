using System;
using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Mobile
{
    /// <summary>
    /// Tests that check front/deposit page
    /// </summary>
    [TestFixture("Apple iPhone 4")]
    public class DepositPageTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private string _device;

        public DepositPageTests(string device)
        {
            _device = device;
        }

        /// <summary>
        /// Changes default deposit amount in the back office and checks if it was changed on the site/deposit
        /// </summary>
        [Test]
        public void Change_Default_Deposit_Amount()
        {
            _commonActions.SignIn_in_admin_panel();
            string defaultValue =_commonActions.Change_Amount_Of_Default_Deposit("1");

            _commonActions.Log_In_Front(_driverCover.LoginThree, _driverCover.Password);
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/deposits/new/");

            DepositMobileObj depositPage = new DepositMobileObj(_driver);
            string defaultValueFront = depositPage.GetSelectedAmount();

            Assert.AreEqual(defaultValue, defaultValueFront, "Sorry but default amount is not as expected. Current page is: " + _driverCover.Driver.Url + " ");
        }

        [TearDown]
        public void CleanUp()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
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
            _driver = new ChromeDriver(CreateOptions(_device));
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
        }

        /// <summary>
        /// Creates and returns ChromeOptions for a mobile device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public ChromeOptions CreateOptions(string device)
        {
            ChromeOptions options = new ChromeOptions();
            options.EnableMobileEmulation(device);
            return options;
        }
    }
}
