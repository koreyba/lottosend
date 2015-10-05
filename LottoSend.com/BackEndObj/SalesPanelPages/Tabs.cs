using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.SalesPanelPages
{
    /// <summary>
    /// Tabs in the sales panel
    /// </summary>
    public class Tabs : DriverCover
    {
        public Tabs(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("admin/orders"))
            {
                throw new Exception("Sorry it must be not the sales panel page. Please check it. Current page is: " + Driver.Url + " ");
            }
            
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#ui-id-1")]
        private IWebElement _register;

        [FindsBy(How = How.CssSelector, Using = "#ui-id-2")]
        private IWebElement _ccDetails;

        [FindsBy(How = How.CssSelector, Using = "#ui-id-3")]
        private IWebElement _viewBets;

        [FindsBy(How = How.CssSelector, Using = "#ui-id-5")]
        private IWebElement _clientLogs;

        [FindsBy(How = How.CssSelector, Using = "#ui-id-6")]
        private IWebElement _clientTransactions;

        [FindsBy(How = How.CssSelector, Using = "#ui-id-7")]
        private IWebElement _resetPassword;

        public void GoToRegisterTab()
        {
            _register.Click();
            WaitForPageLoading();
        }

        public void GoToCcDetailsTab()
        {
            _ccDetails.Click();
            WaitForPageLoading();
        }

        public void GoToViewBetsTab()
        {
            _viewBets.Click();
            WaitForPageLoading();
        }

        public void GoToClientLogsTab()
        {
            _clientLogs.Click();
            WaitForPageLoading();
        }

        public void GoToClientTransactionsTab()
        {
            _clientTransactions.Click();
            WaitForPageLoading();
        }

        public void GoToResetPasswordTab()
        {
            _resetPassword.Click();
            WaitForPageLoading();
        }
    }
}
