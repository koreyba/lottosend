using System;
using LottoSend.com.BackEndObj.GroupGapePages;
using LottoSend.com.Helpers;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    public class GroupGameTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BackOfficeVerifications _backOfficeVerifications;
        private bool _setUpFailed = false;

        /// <summary>
        /// Creates a new group ticket in a new group. Then removes the group with the ticket
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void CreateNewGroupTicket()
        {
            _commonActions.SignIn_in_admin_panel();

            string groupName = RandomGenerator.GenerateRandomString(10);
            _commonActions.CreateGroup(groupName);

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/aggrupations/new");
            NewGroupTicketCreationObj newGroupTicket = new NewGroupTicketCreationObj(_driver);

            string lottery = "SuperLotto Plus";
            newGroupTicket.FillInFields(lottery, groupName, 50, true, 15, 11.3);

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

            _backOfficeVerifications.IsTicketExists(groupName, lottery, common3 + "," + special3 + "\r\n" + common2 + "," + special2 + "\r\n" + common1 + "," + special1);

            _commonActions.DeleteGroup(groupName);
        }

        /// <summary>
        /// Creates a new group in backoffice and checks if it is created (then removes it)
        /// </summary>
        [Test]
        public void CreateNewGroup()
        {
            _commonActions.SignIn_in_admin_panel();

            string name = RandomGenerator.GenerateRandomString(10);

            _commonActions.CreateGroup(name);
           
            GroupsPageObj groupsPage = new GroupsPageObj(_driver);

            Assert.IsTrue(groupsPage.IsGroupExists(name), "Sorry but the group you searched for doesn't exist! Please check it on the page: " + _driverCover.Driver.Url + " ");

            groupsPage.DeleteGroup(name);
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
