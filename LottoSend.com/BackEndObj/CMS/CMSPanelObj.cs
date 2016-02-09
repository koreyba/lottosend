using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.CMS
{
    /// <summary>
    /// Page object of Log Out button in CMS... for CMS manager
    /// </summary>
    public class CMSPanelObj : DriverCover
    {
        public CMSPanelObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("cms"))
            {
                throw new Exception("Sorry it must be not a CMS page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.LinkText, Using = "Logout")]
        private IWebElement _logOutButton;

        public void Logout()
        {
            _logOutButton.Click();
            WaitForPageLoading();
        }
    }
}
