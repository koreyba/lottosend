using System.Runtime.InteropServices;
using System.Text;
using LottoSend.com.BackEndObj.GroupGapePages;
using LottoSend.com.BackEndObj.SalesPanelPages;
using LottoSend.com.TestCases;
using NUnit.Framework;
using OpenQA.Selenium;

namespace LottoSend.com.Verifications
{
    class BackOfficeVerifications
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        public StringBuilder Errors;
        private CommonActions _commonActions;

        public BackOfficeVerifications(IWebDriver newDriver)
        {
            _driver = newDriver;
            _driverCover = new DriverCover(_driver);
            Errors = new StringBuilder();
            _commonActions = new CommonActions(_driver);
        }

        public void IfUserSignedInSalesPanel()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            SalesPanelRegisterObj register = new SalesPanelRegisterObj(_driver);
            bool signedIn = register.IsSignedIn();

            Assert.IsTrue(signedIn, "Sorry but no user is signed in the sales panel. Please check it ");
        }

        /// <summary>
        /// Navigates to groups page (in back office) and check is a ticket exists
        /// </summary>
        /// <param name="group"></param>
        /// <param name="lottery"></param>
        /// <param name="numbers"></param>
        public void IsTicketExists(string group, string lottery, string numbers)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/groups");
            GroupsPageObj groupsPage = new GroupsPageObj(_driver);
            bool exists =  groupsPage.IsTicketExists(group, lottery, numbers);

            Assert.IsTrue(exists, "Sorry but the ticket doesn't exist, check it on page: " + _driverCover.Driver.Url + " ");
        }
    }
}
