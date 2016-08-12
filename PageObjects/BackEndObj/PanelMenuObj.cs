using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj
{
    /// <summary>
    /// Page Object of the top panel (menu)
    /// </summary>
    public class PanelMenuObj : DriverCover
    {
        public PanelMenuObj(IWebDriver driver) : base(driver)
        {
            if(!Driver.Url.Contains("admin"))
            {
                throw new Exception("It's not admin panel ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.LinkText, Using = "Logout")]
        private IWebElement _logOutButton;

        public void LogOut()
        {
            _logOutButton.Click();
            WaitForPageLoading();
        }
    }
}
