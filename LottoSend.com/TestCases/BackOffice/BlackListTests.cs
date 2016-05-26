using System;
using LottoSend.com.BackEndObj;
using LottoSend.com.BackEndObj.ClientOrderPricessing;
using LottoSend.com.BackEndObj.WebUsersPages;
using LottoSend.com.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice
{
    [TestFixture(typeof(ChromeDriver))]
    public class BlackListTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private TestsSharedCode _sharedCode;

        [Test]
        public void Check_If_BL_Image_Is_Displayed_In_OrderProcessing()
        {
            /* Create a user
             * Make a transaction
             * Add the user to BL
             * Check if BL image appeared for this transaction in order processing
             */
            string email = _commonActions.Sign_Up_Front();
            _commonActions.BuyRegularOneDrawTicket_Front(WayToPay.Offline, false);
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/web_users");

            WebUsersPageObj users = new WebUsersPageObj(_driver);
            users.FilterByEmail(email);
            ViewWebUserPageObj viewWebUser = users.ClickViewButton();
            viewWebUser.AddToBlackList("user added to blacklist");

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders_processed");

            OrderProcessingObj orderProcessing = new OrderProcessingObj(_driver);
            orderProcessing.FilterByEmail(email);
            bool image = orderProcessing.isBLImageExist();

            Assert.IsTrue(image, "Sorry but BL image is not found in client order processing for your user. ");
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
