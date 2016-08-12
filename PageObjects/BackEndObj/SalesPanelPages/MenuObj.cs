using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj.SalesPanelPages
{
    /// <summary>
    /// Page Object for menu in the sales panel (lotteries and raffles)
    /// </summary>
    public class MenuObj : DriverCover
    {
        public MenuObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        //[FindsBy(How = How.CssSelector, Using = "#\\35")]
        //private IWebElement _megaMillions;

        //[FindsBy(How = How.CssSelector, Using = "#ui-id-26 > a")]
        //private IWebElement _testRaffle3;

        [FindsBy(How = How.CssSelector, Using = "div.left")]
        private IWebElement _menu;

        [FindsBy(How = How.CssSelector, Using = "#orders_deposit")]
        private IWebElement _deposit;

        /// <summary>
        /// Clicks "Deposit" button in menu
        /// </summary>
        public void GoToDeposit()
        {
            _deposit.Click();
            WaitForPageLoading();
            WaitjQuery();
        }

        /// <summary>
        /// Navigates to the lottery (also works for raffles)
        /// </summary>
        /// <param name="lotteryName"></param>
        public void GoToLotteryPage(string lotteryName)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _menu.FindElement(By.LinkText(lotteryName)).Click();
            WaitForPageLoading();
            WaitjQuery();
        }
    }
}
