﻿using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.RegularTicketsPages;
using TestFramework.FrontEndObj.GamePages;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.BackOffice.Packages
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DeactivtePackage <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private TestFramework.CommonActions _commonActions;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Deactivates a package of regular game and checks if it's visible on the site.
        /// </summary>
        [Test]
        public void Deactivate_Site_For_Package_And_Check_Visibility()
        {
            _commonActions.SignIn_in_admin_panel();

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/packages");

            PackagesPageObj packagesPage= new PackagesPageObj(_driver);
            packagesPage.EditPackage("Oz Lotto (1-45)", 7);

            NewPackagePageObj packageEdit = new NewPackagePageObj(_driver);
            packageEdit.DeactivateSite("stg.lottobaba.com");

            _commonActions.ClearCache("stg.lottobaba.com");
            
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/oz-lotto?tab=single");
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            foreach (var line in regularGame.Lines)
            {
                if (line.Equals("7"))
                {
                    Assert.Fail("The package was not deactivated, it's still visible on the play page. Current URL is: " + _driver.Url + " ");
                }
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
                    packagesPage.EditPackage("Oz Lotto (1-45)", 7);

                    NewPackagePageObj packageEdit = new NewPackagePageObj(_driver);
                    packageEdit.ActivateSite("stg.lottobaba");

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
            _commonActions = new TestFramework.CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
