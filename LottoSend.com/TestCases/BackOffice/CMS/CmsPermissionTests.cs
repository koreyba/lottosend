using System.Configuration;
using LottoSend.com.BackEndObj.CMS;
using LottoSend.com.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice.CMS
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    public class CmsPermissionTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;

        /// <summary>
        /// Checks if a use that has crosslabel permission can't change content of other sites not assigned to him
        /// </summary>
        [Test]
        [Category("Critical")]
        public void CMS_Crosslabel_Permission_Not_Assigned_Sites()
        {
            /* 1. Create user with crosslabel permission and assign 2 sites for him
             * 2. Go to CMS
             * 3. Change a key on one site
             * 4. Log in with different user that have different assigned sites
             * 5. Check if content of a different site was not changed in the same key
             */
            _commonActions.SignIn_in_admin_panel(ConfigurationManager.AppSettings["CMSManager"], "11111111");
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/snippets/CMS_Crosslabel_Test_Key/edit");
            EditSnippetObj snippetEditing = new EditSnippetObj(_driver);

            string newContent = RandomGenerator.GenerateRandomString(10);

            snippetEditing.UpdateFirstLanguageContent(newContent);

            CMSPanelObj menu = new CMSPanelObj(_driver);
            menu.Logout();

            _commonActions.SignIn_in_admin_panel(ConfigurationManager.AppSettings["CMSManagerTwo"], "11111111");

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/38/snippets/CMS_Crosslabel_Test_Key/edit");

            Assert.AreNotEqual(newContent, snippetEditing.TextOfFirstContentInput, "Sorry but conten of the key was changed and it was not supposed to. Maybe CMS_Crosslabel permission works wrong. Current URL is: " + _driver.Url + " "); 
        }

        /// <summary>
        /// Checks if a use that has crosslabel permission can change conten of assigned to him sites changing only 1 key
        /// </summary>
        [Test]
        [Category("Critical")]
        public void CMS_Crosslabel_Permission_Assigned_Sites()
        {
            /* 1. Create user with crosslabel permission and assign 2 sites for him
             * 2. Go to CMS
             * 3. Change a key on one site
             * 4. Check if content of a different assigned to this user site was changed in the same key
             */

             _commonActions.SignIn_in_admin_panel(ConfigurationManager.AppSettings["CMSManager"], "11111111");
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/snippets/CMS_Crosslabel_Test_Key/edit");
            EditSnippetObj snippetEditing = new EditSnippetObj(_driver);

            string newContent = RandomGenerator.GenerateRandomString(10);

            snippetEditing.UpdateFirstLanguageContent(newContent);

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/37/snippets/CMS_Crosslabel_Test_Key/edit");
           
            Assert.AreEqual(newContent, snippetEditing.TextOfFirstContentInput, "Sorry but conten of the key was not changed. Maybe CMS_Crosslabel permission doesn't work. Current URL is: " + _driver.Url + " "); 
        }

        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                _driverCover.TakeScreenshot();
            }
            MessageConsoleCreator message = new MessageConsoleCreator();
            message.DriverDisposed();
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            MessageConsoleCreator message = new MessageConsoleCreator();
            message.TestWillRun();
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
        }
    }
}
