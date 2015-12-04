using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Web.Group_ticktes
{
    /// <summary>
    /// Includes group tests on the web that is not connected with buying tickets (other tests)
    /// </summary>
    /// <typeparam name="TWebDriver"></typeparam>
    [TestFixture(typeof(ChromeDriver))]
    class GroupTicketTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Adds a group ticket to the cart and removes it. Checks if the share was returned to the ticket
        /// </summary>
        [Test]
        [Category("Critical")]
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
            _sharedCode.CleanUp(ref _driver);
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _sharedCode = new TestsSharedCode(_driver);
            _commonActions = new CommonActions(_driver);
        }
    }
}
