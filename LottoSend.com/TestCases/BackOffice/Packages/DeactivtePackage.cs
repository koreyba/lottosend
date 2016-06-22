﻿using System;
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
    public class DeactivtePackage <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private TestsSharedCode _sharedCode;

        [Test]
        public void Deactivate_Site_For_Package_And_Check_Visibility()
        {
            _commonActions.SignIn_in_admin_panel();

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/packages");

            PackagesPageObj packagesPage= new PackagesPageObj(_driver);
            packagesPage.EditPackage("Oz Lotto (1-45)", 7);

            NewPackagePageObj packageEdit = new NewPackagePageObj(_driver);
            packageEdit.DeactivateSite("stg.lottobaba");

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/sites/1/edit");
            SiteEditingPageObj siteEdit = new SiteEditingPageObj(_driver);
            siteEdit.ClearCache();
            
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

                    _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/sites/1/edit");
                    SiteEditingPageObj siteEdit = new SiteEditingPageObj(_driver);
                    siteEdit.ClearCache();
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
