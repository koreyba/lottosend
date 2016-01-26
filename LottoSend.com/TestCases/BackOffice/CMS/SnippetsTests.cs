using LottoSend.com.BackEndObj.CMS;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice.CMS
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SnippetsTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;

        /// <summary>
        /// Searches a key on the "snippets" page by content and checks how many results are found (should be 1)
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Search_Snippet_By_Content()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/snippets");

            KeysPageObj keysPage = new KeysPageObj(_driver);

            keysPage.ClickEditKey("content-test-key");
            EditSnippetObj pageEditing = new EditSnippetObj(_driver);

            string content = RandomGenerator.GenerateRandomString(50);
            pageEditing.UpdateFirstLanguageContent(content);

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/snippets");

            int numberOfResults = keysPage.SearchKey(content, SearchBy.Content);

            Assert.AreEqual(1, numberOfResults, "Sorry but the key was not found ");
        } 

        /// <summary>
        /// Searches a key on the snippets page and checks how many results are found (should be 1)
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Search_Snippet_By_Key()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/snippets");

            KeysPageObj keysPage = new KeysPageObj(_driver);
            int numberOfResults = keysPage.SearchKey("content-test-key", SearchBy.Key);

            Assert.AreEqual(1, numberOfResults, "Sorry but the key was not found ");
        }  

        /// <summary>
        /// Edits a snippet's content and checks if it was updated using "mass edit" function
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void MassEdit_Snippet_Content()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/snippets/mass_edit?locale=nl");

            MassEditPageObj massEdit = new MassEditPageObj(_driver);
            massEdit.SearchKey("content-test-key");

            string content = RandomGenerator.GenerateRandomString(50);
            massEdit.UpdateFirstLanguageContent_EditPlus(content);
            massEdit.SearchKey("content-test-key");

            Assert.AreEqual(content, massEdit.TextOfFirstContentInput);
        }  

        /// <summary>
        /// Edits a snippet's content and checks if it was updated using "Edit +" function 
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void EditPlus_Snippet_Content()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/snippets");

            KeysPageObj pages = new KeysPageObj(_driver);
            pages.ClickEditPlusForKey("content-test-key");
            EditSnippetObj snippetEditing = new EditSnippetObj(_driver);

            string content = RandomGenerator.GenerateRandomString(50);
            snippetEditing.UpdateFirstLanguageContent_EditPlus(content);

            Assert.AreEqual(content, snippetEditing.TextOfFirstContentInput);
        }

        /// <summary>
        /// Edits a snippet's content and checks if it was updated
        /// </summary>
        [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Edit_Snippet_Content()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/snippets");

            KeysPageObj pages = new KeysPageObj(_driver);
            pages.ClickEditKey("content-test-key");
            EditSnippetObj snippetEditing = new EditSnippetObj(_driver);

            string content = RandomGenerator.GenerateRandomString(50);
            snippetEditing.UpdateFirstLanguageContent(content);

            Assert.AreEqual(content, snippetEditing.TextOfFirstContentInput);
        }

        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                _driverCover.TakeScreenshot();
            }
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
