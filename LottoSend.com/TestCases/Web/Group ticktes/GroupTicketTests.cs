using System;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Web.Group_ticktes
{
    /// <summary>
    /// Includes group tests on the web that is not connected with buying tickets (other tests)
    /// </summary>
    /// <typeparam name="TWebDriver"></typeparam>
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    class GroupTicketTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private TestsSharedCode _sharedCode;
        private bool _setUpFailed = false;

        /// <summary>
        /// Buys a group ticket and removes it from its draw. Checks if the share was returned to the ticket
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void If_Share_Removed_From_Draw_Returns_To_Ticket()
        {
            _commonActions.Log_In_Front_PageOne(_driverCover.LoginFive, _driverCover.Password);
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/powerball/");
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);
            groupGame.SelectOneTimeEntryGame();
            
            int sharesBefore = groupGame.GetNumberOfLeftShares(1);
            MerchantsObj merchants = groupGame.ClickBuyTicketsButton();
            merchants.Pay(WayToPay.Offline);

            _commonActions.RemoveBetFromDraw_BackOffice("Powerball", 1, false);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/powerball/");

            Assert.AreEqual(sharesBefore, groupGame.GetNumberOfLeftShares(1), "Sorry but the share was not returned back to the ticket. Current URL: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Adds a group ticket to the cart and removes it. Checks if the share was returned to the ticket
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void If_Share_Removed_From_Cart_Returns_To_Ticket()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/el-gordo-de-la-primitiva/");
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);
            int sharesBefore = groupGame.GetNumberOfLeftShares(1);
            groupGame.ClickAddToCartButton();
            _driverCover.RefreshPage();

            Assert.AreEqual(sharesBefore - 1, groupGame.GetNumberOfLeftShares(1), "Sorry but a share was not deducted from the ticket. Maybe it was not added to the cart. Current URL: " + _driverCover.Driver.Url + " ");

            _commonActions.DeleteAllTicketFromCart_Front();

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/el-gordo-de-la-primitiva/");

            Assert.AreEqual(sharesBefore, groupGame.GetNumberOfLeftShares(1), "Sorry but the share was not returned back to the ticket. Current URL: " + _driverCover.Driver.Url + " ");
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
            _sharedCode = new TestsSharedCode(_driver);
            _commonActions = new CommonActions(_driver);
        }
    }
}
