using System;
using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Web
{
    /// <summary>
    /// Tests that check front/deposit page
    /// </summary>
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DepositPageTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private TestsSharedCode _sharedCode;

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

            DepositObj depositPage = new DepositObj(_driver);
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
