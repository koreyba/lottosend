using LottoSend.com.BackEndObj.CMS;
using NUnit.Framework;
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
