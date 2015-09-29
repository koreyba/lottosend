using LottoSend.com.BackEndObj.GroupGapePages;
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

        [Test]
        public void CreateNewGroup()
        {
            _commonActions.Authorize_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/groups/new");
            NewGroupCreationObj newGroup = new NewGroupCreationObj(_driver);
            string name = RandomGenerator.GenerateRandomString(10);
            GroupsPageObj groupsPage =  newGroup.CreateNewGroup(name);

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
        }
    }
}
