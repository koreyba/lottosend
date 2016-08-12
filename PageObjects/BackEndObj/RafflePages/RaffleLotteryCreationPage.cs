using System;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj.RafflePages
{
    /// <summary>
    /// PageObject of new raffle creation page admin/raffles/new
    /// </summary>
    public class RaffleLotteryCreationPage : DriverCover
    {
        public RaffleLotteryCreationPage(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("admin/raffles/new"))
            {
                throw new Exception("Sorry it must be not raffle creation page. Curren URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#raffle_name")]
        private IWebElement _name;

        [FindsBy(How = How.CssSelector, Using = "#raffle_currency")]
        private IWebElement _currency;

        [FindsBy(How = How.CssSelector, Using = "#raffle_jackpot")]
        private IWebElement _jackpot;

        [FindsBy(How = How.CssSelector, Using = "#raffle_draw_date")]
        private IWebElement _drawDate;

        [FindsBy(How = How.CssSelector, Using = "#raffle_close_time")]
        private IWebElement _closeTime;

        [FindsBy(How = How.CssSelector, Using = "#raffle_logo_input")]
        private IWebElement _logo;

        [FindsBy(How = How.CssSelector, Using = "#raffle_share_price")]
        private IWebElement _sharePrice;

        [FindsBy(How = How.CssSelector, Using = "#raffle_share_for_discount")]
        private IWebElement _shareForDiscount;

        [FindsBy(How = How.CssSelector, Using = "#raffle_discount")]
        private IWebElement _discount;

        [FindsBy(How = How.CssSelector, Using = "#raffle_submit_action > input")]
        private IWebElement _createRaffleButton;

        /// <summary>
        /// Creates a new raffle lottery
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public RaffleDashboardPageObj CreateNewRaffleLottery(string name)
        {
            _name.SendKeys(name);
            ChooseElementInSelect("EUR", _currency, SelectBy.Value);
            _jackpot.SendKeys("10000000");
            var imgPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Assets\scannedimage2.jpg");
            //_logo.SendKeys(imgPath);
            _sharePrice.SendKeys("31");
            _shareForDiscount.SendKeys("3");

            _discount.SendKeys("10");
            _drawDate.SendKeys("11");
            _drawDate.SendKeys("12");
            _drawDate.SendKeys(Keys.Tab);
            _drawDate.SendKeys("2018");

            _closeTime.SendKeys("10");
            _closeTime.SendKeys("12");
            _closeTime.SendKeys(Keys.Tab);
            _closeTime.SendKeys("2018");

            _createRaffleButton.Click();
            

            return new RaffleDashboardPageObj(Driver);
        }
    }
}
