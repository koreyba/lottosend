using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.SalesPanelPages
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


        /// <summary>
        /// Navigates to the lottery (also works for raffles)
        /// </summary>
        /// <param name="lotteryName"></param>
        public void GoToLotteryPage(string lotteryName)
        {
            _menu.FindElement(By.LinkText(lotteryName)).Click();
            WaitForPageLoading();
            WaitjQuery();
        }
    }
}
