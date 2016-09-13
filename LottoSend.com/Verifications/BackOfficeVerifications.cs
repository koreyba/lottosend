using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using TestFramework;
using TestFramework.BackEndObj;
using TestFramework.BackEndObj.GroupGapePages;
using TestFramework.BackEndObj.SalesPanelPages;

namespace LottoSend.com.Verifications
{
    class BackOfficeVerifications
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        public StringBuilder Errors;
        private TestFramework.CommonActions _commonActions;

        public BackOfficeVerifications(IWebDriver newDriver)
        {
            _driver = newDriver;
            _driverCover = new DriverCover(_driver);
            Errors = new StringBuilder();
            _commonActions = new TestFramework.CommonActions(_driver);
        }

        /// <summary>
        /// On backoffice/bulk-buys/not completed not deleted page checks all the records is the draw date is not in past
        /// </summary>
        public void IfPastDrawDateIsPresent()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/bulk_buys?scope=not_completed_not_deleted");
            BulkBuysPageObj bulkBuys = new BulkBuysPageObj(_driver);

            StringBuilder buildReport = new StringBuilder();

            for (int j = 1; j <= bulkBuys.NumberOfPages; ++j)
            {
                bulkBuys.NavigateToPage(j);
                for (int i = 1; i < bulkBuys.NumberOfRecordsOnPage; ++i)
                {
                    DateTime date = bulkBuys.DrawDate(i);
                    if (date.Date < DateTime.Today)
                    {
                        buildReport.Append("\n Record №" + i + " on page " + j + " has a past draw date. So the bet was not pushed to the next draw. \n");
                    }
                }
            }

            

            if (buildReport.Length > 0)
            {
                throw new Exception(buildReport.ToString());
            }
        }

        public void IfUserSignedInSalesPanel()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            RegisterObj register = new RegisterObj(_driver);
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
