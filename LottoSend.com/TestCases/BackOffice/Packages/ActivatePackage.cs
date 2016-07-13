using System;
using LottoSend.com.BackEndObj;
using LottoSend.com.BackEndObj.RegularTicketsPages;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice.Packages
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ActivatePackage <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Activates a package of regular game and checks if it's visible on the site.
        /// </summary>
        [Test]
        public void Activate_Site_For_Package_And_Check_Visibility()
        {
            _commonActions.SignIn_in_admin_panel();

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/packages");

            PackagesPageObj packagesPage= new PackagesPageObj(_driver);
            packagesPage.EditPackage("EuroJackpot (1-50)", 6);

            NewPackagePageObj packageEdit = new NewPackagePageObj(_driver);
            packageEdit.DeactivateSite("stg.lottobaba");

            _commonActions.ClearCache("stg.lottobaba");

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/eurojackpot?tab=single");
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            bool found = false;

            foreach (var line in regularGame.Lines)
            {
                if (line.Equals("6"))
                {
                    found = true;
                }
            }

            if (!found)
            {
                Assert.Fail("The package was not activated, it's still invisible on the play page. Current URL is: " + _driver.Url + " ");
            }
        }

        [TearDown]
        public void CleanUp()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    _driverCover.TakeScreenshot();
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                try
                {
                    _commonActions.SignIn_in_admin_panel();
                    _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/packages");

                    PackagesPageObj packagesPage = new PackagesPageObj(_driver);
                    packagesPage.EditPackage("EuroJackpot (1-50)", 3);

                    NewPackagePageObj packageEdit = new NewPackagePageObj(_driver);
                    packageEdit.DeactivateSite("stg.lottobaba");

                    _commonActions.ClearCache("stg.lottobaba");
                }
                catch (Exception)
                {

                }
                finally
                {
                    _sharedCode.CleanUp(ref _driver);
                }

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
            _commonActions = new CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
