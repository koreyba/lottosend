using LottoSend.com.BackEndObj.CMS;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice.CMS
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    public class TranslationsTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;

        /// <summary>
        /// Edits a translation's content and checks if it was updated using "mass edit" function
        /// </summary>
        [Test]
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
        [Test]
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
        [Test]
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
