using LottoSend.com.BackEndObj.CMS;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice.CMS
{
    [TestFixture(typeof(ChromeDriver))]
    public class PagesTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;

        /// <summary>
        /// Creates a page key in CMS and removes it (also use search in this test)
        /// </summary>
        [Test]
        public void Remove_Page_Key()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "cms/sites/1/pages");

            PagesObj pages = new PagesObj(_driver);
            NewPageObj newPage = pages.ClickNewButton();
           
            string key = newPage.CreateKey();

            pages.RemoveKey(key);
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
