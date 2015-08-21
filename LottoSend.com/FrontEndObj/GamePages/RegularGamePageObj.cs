using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LottoSend.com.FrontEndObj
{
    public class RegularGamePageObj : DriverCover
    {
        public RegularGamePageObj(IWebDriver driver) : base(driver)
        {
            //TODO make validation

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "ul.nav.nav-tabs > li:nth-child(2) > a")]
        private IWebElement _singleGame;

        /// <summary>
        /// Click on "Standart game" button
        /// </summary>
        public void ClickStandartGameButton()
        {
            _singleGame.Click();

            WaitAjax();
            WaitjQuery();
        }

        /// <summary>
        /// Click "Buy Tickets" button
        /// </summary>
        /// <returns></returns>
        public MerchantsObj ClickBuyTicketsButton()
        {
            Thread.Sleep(100);
            _detectBuyTicketsButton().Click();
            bool w = WaitjQuery();

            return new MerchantsObj(Driver);
        }

        /// <summary>
        /// Returns displayed button "Buy Tickets" or null if the button was not found
        /// </summary>
        /// <returns>Button/null</returns>
        private IWebElement _detectBuyTicketsButton()
        {
            return GetFirstVisibleElementFromList(By.CssSelector("button.btn.btn-warning.play-now"));
        }

        /// <summary>
        /// Clicks on "Add to Cart and Play More Games"
        /// </summary>
        public void ClickAddToCartButton()
        {
            Thread.Sleep(100);
            _detectAddToCartButton().Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Returns displayed button "Add To Cart" or null if the button was not found
        /// </summary>
        /// <returns>Button/null</returns>
        private IWebElement _detectAddToCartButton()
        {
            return GetFirstVisibleElementFromList(By.CssSelector("#add-to-cart"));
        }        
    }
}
