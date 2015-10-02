﻿using LottoSend.com.BackEndObj.GroupGapePages;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace LottoSend.com.TestCases.BackOffice
{
    [TestFixture(typeof(ChromeDriver))]
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(InternetExplorerDriver))]
    public class GroupGameTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BackOfficeVerifications _backOfficeVerifications;

        /// <summary>
        /// Creates a new group ticket in a new group. Then removes the group with the ticket
        /// </summary>
        [Test]
        public void CreateNewGroupTicket()
        {
            _commonActions.Authorize_in_admin_panel();

            string groupName = RandomGenerator.GenerateRandomString(10);
            _commonActions.CreateGroup(groupName);

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/aggrupations/new");
            NewGroupTicketCreationObj newGroupTicket = new NewGroupTicketCreationObj(_driver);

            newGroupTicket.FillInFields("FireFox Lottery", groupName, 50, true, 15, 11.3);

            string common1 = "1, 2, 3, 4, 5";
            string common2 = "1, 2, 3, 4, 5";
            string common3 = "1, 2, 3, 4, 5";
            string special1 = "1";
            string special2 = "2";
            string special3 = "3";

            newGroupTicket.AddNewLine(common1, special1);
            newGroupTicket.AddNewLine(common2, special2);
            newGroupTicket.AddNewLine(common3, special3);

            newGroupTicket.ClickCreateButton();

            _backOfficeVerifications.IsTicketExists("Selenium Group", "FireFox", common3 + "," + special3 + "\r\n" + common2 + "," + special2 + "\r\n" + common1 + "," + special1);

            _commonActions.DeleteGroup(groupName);
        }

        /// <summary>
        /// Creates a new group in backoffice and checks if it is created (then removes it)
        /// </summary>
        [Test]
        public void CreateNewGroup()
        {
            _commonActions.Authorize_in_admin_panel();

            string name = RandomGenerator.GenerateRandomString(10);

            _commonActions.CreateGroup(name);
           
            GroupsPageObj groupsPage = new GroupsPageObj(_driver);

            Assert.IsTrue(groupsPage.IsGroupExists(name), "Sorry but the group you searched for doesn't exist! Please check it on the page: " + _driverCover.Driver.Url + " ");

            groupsPage.DeleteGroup(name);
        }


        [TearDown]
        public void CleanUp()
        {
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
