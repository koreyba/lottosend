using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj;

namespace LottoSend.com.TestCases
{
    /// <summary>
    /// Runs once before all the test in order to config the environment 
    /// </summary>
   // [SetUpFixture]
    public class ConfigTests
    {
       // [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            IWebDriver driver = new ChromeDriver();
            try
            {
                TestFramework.CommonActions actions = new TestFramework.CommonActions(driver);
                actions.SignIn_in_admin_panel();
                DriverCover driverCover = new DriverCover(driver);
                driverCover.NavigateToUrl(driverCover.BaseAdminUrl + "admin/sites/1/edit");

                SiteEditingPageObj siteEditing = new SiteEditingPageObj(driver);
                siteEditing.SwitchCombinedPageOff();
                siteEditing.SwitchOneTimeEntryOn();
                siteEditing.SwitchGroupGameOn();
                siteEditing.SwitchSingleGameOn();
                siteEditing.SwitchAddressOn();
                siteEditing.SwitchNewSignUpOff();
                siteEditing.SwitchAddToCartOn(true);

                driver.Dispose();
            }
            catch (Exception)
            {
               driver.Dispose();
            }
        }
    }
}
