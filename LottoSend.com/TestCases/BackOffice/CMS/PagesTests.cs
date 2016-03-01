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
    public class PagesTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private bool _setUpFailed = false;

        /// <summary>
        /// Searches a key on the "pages" page by content and checks how many results are found (should be 1)
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Search_Page_By_Content()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/pages");

            KeysPageObj keysPage = new KeysPageObj(_driver);

            keysPage.ClickEditKey("content-test-key");
            EditPageObj pageEditing = new EditPageObj(_driver);

            string content = RandomGenerator.GenerateRandomString(50);
            pageEditing.UpdateFirstLanguageContent(content);

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/pages");

            int numberOfResults = keysPage.SearchKey(content, SearchBy.Content);

            Assert.AreEqual(1, numberOfResults, "Sorry but the key was not found ");
        } 

        /// <summary>
        /// Searches a key on the "pages" page and checks how many results are found (should be 1)
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Search_Page_By_Key()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/pages");

            KeysPageObj keysPage = new KeysPageObj(_driver);
            int numberOfResults = keysPage.SearchKey("content-test-key", SearchBy.Key);

            Assert.AreEqual(1, numberOfResults, "Sorry but the key was not found ");
        }  

        /// <summary>
        /// Edits a page's content and checks if it was updated using "mass edit" function
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void MassEdit_Page_Content()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/pages/mass_edit?locale=de");

            MassEditPageObj massEdit = new MassEditPageObj(_driver);
            massEdit.SearchKey("content-test-key");

            string content = RandomGenerator.GenerateRandomString(50);
            massEdit.UpdateFirstLanguageContent_EditPlus(content);
            massEdit.SearchKey("content-test-key");

            Assert.AreEqual(content, massEdit.TextOfFirstContentInput);
        }   

        /// <summary>
        /// Edits a page's content and checks if it was updated using "Edit +" function 
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void EditPlus_Page_Content()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/pages");

            KeysPageObj pages = new KeysPageObj(_driver);
            pages.ClickEditPlusForKey("content-test-key");
            EditPageObj snippetEditing = new EditPageObj(_driver);

            string content = RandomGenerator.GenerateRandomString(50);
            snippetEditing.UpdateFirstLanguageContent_EditPlus(content);

            Assert.AreEqual(content, snippetEditing.TextOfFirstContentInput);
        }   

        /// <summary>
        /// Edits a page's content and checks if it was updated
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        [Category("CMS")]
        public void Edit_Page_Content()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/pages");

            KeysPageObj pages = new KeysPageObj(_driver);
            pages.ClickEditKey("content-test-key");
            EditPageObj pageEditing = new EditPageObj(_driver);

            string content = RandomGenerator.GenerateRandomString(50);
            pageEditing.UpdateFirstLanguageContent(content);

            Assert.AreEqual(content, pageEditing.TextOfFirstContentInput);
        }

        /// <summary>
        /// Creates a page key in CMS and removes it (also use search in this test)
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Remove_Page_Key()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/pages");

            KeysPageObj pages = new KeysPageObj(_driver);
            NewPageObj newPage = pages.ClickNewButton();
           
            string key = newPage.CreateKey();

            pages.RemoveKey(key);
        }

        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
            {
                _driverCover.TakeScreenshot();
            }
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
