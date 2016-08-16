using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.CMS;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.BackOffice.CMS
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    public class TranslationsTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private TestFramework.CommonActions _commonActions;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Searches a key on the "translations" page by content and checks how many results are found (should be 1)
        /// </summary>
       // [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Search_Translation_By_Content()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/translations");

            KeysPageObj keysPage = new KeysPageObj(_driver);

            keysPage.ClickEditKey("account.balance.transaction.to");
            EditTranslationObj pageEditing = new EditTranslationObj(_driver);

            string content = RandomGenerator.GenerateRandomString(50);
            pageEditing.UpdateFirstLanguageContent(content);

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/translations");

            int numberOfResults = keysPage.SearchKey(content, SearchBy.Content);

            Assert.AreEqual(1, numberOfResults, "Sorry but the key was not found ");
        } 

        /// <summary>
        /// Searches a key on the snippets page and checks how many results are found (should be 1)
        /// </summary>
        //[Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Search_Snippet_By_Key()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/translations");

            KeysPageObj keysPage = new KeysPageObj(_driver);
            int numberOfResults = keysPage.SearchKey("account.balance.transaction.to", SearchBy.Key);

            Assert.AreEqual(1, numberOfResults);
        } 

        /// <summary>
        /// Edits a translation's content and checks if it was updated using "mass edit" function
        /// </summary>
        //[Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void MassEdit_Translation_Content()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/translations/mass_edit?locale=ro");

            MassEditPageObj massEdit = new MassEditPageObj(_driver);
            massEdit.SearchKey("account.balance.transaction.to");

            string content = RandomGenerator.GenerateRandomString(50);
            massEdit.UpdateFirstLanguageContent_EditPlus(content);
            massEdit.SearchKey("account.balance.transaction.to");

            Assert.AreEqual(content, massEdit.TextOfFirstContentInput);
        }  

        /// <summary>
        /// Edits a translation's content and checks if it was updated using "Edit +" function 
        /// </summary>
       // [Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void EditPlus_Translation_Content()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/translations");

            KeysPageObj pages = new KeysPageObj(_driver);
            pages.ClickEditPlusForKey("account.balance.transaction.to");
            EditTranslationObj snippetEditing = new EditTranslationObj(_driver);

            string content = RandomGenerator.GenerateRandomString(50);
            snippetEditing.UpdateFirstLanguageContent_EditPlus(content);

            Assert.AreEqual(content, snippetEditing.TextOfFirstContentInput);
        }

        /// <summary>
        /// Edits a translation's content and checks if it was updated
        /// </summary>
        //[Test]
        [Category("Critical")]
        [Category("Parallel")]
        public void Edit_Translation_Content()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/translations");

            KeysPageObj pages = new KeysPageObj(_driver);
            pages.ClickEditKey("account.balance.transaction.to");
            EditTranslationObj snippetEditing = new EditTranslationObj(_driver);

            string content = RandomGenerator.GenerateRandomString(50);
            snippetEditing.UpdateFirstLanguageContent(content);

            Assert.AreEqual(content, snippetEditing.TextOfFirstContentInput);
        } 

        [TearDown]
        public void CleanUp()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
                {
                    _driverCover.TakeScreenshot();
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                _sharedCode.CleanUp(ref _driver);
            }
        }

        [SetUp]
        public void SetUp()
        {
            MessageConsoleCreator message = new MessageConsoleCreator();
            message.TestWillRun();
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new TestFramework.CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
