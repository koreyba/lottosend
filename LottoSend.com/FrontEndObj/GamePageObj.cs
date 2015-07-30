using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoSend.com.FrontEndObj
{
    public class GamePageObj : DriverCover
    {
        public GamePageObj(IWebDriver driver) : base(driver)
        {
            //TODO make validation

            PageFactory.InitElements(Driver, this);
        }

        public MerchantsObj ClickBuyTicketsButton()
        {
            _detectBuyTicketsButton().Click();
            WaitAjax();

            return new MerchantsObj(Driver);
        }

        /// <summary>
        /// Returns displayed button "Buy Tickets" or null if the button was not found
        /// </summary>
        /// <returns>Button/null</returns>
        private IWebElement _detectBuyTicketsButton()
        {
            return _visibleElementFromList("button.btn.btn-success.btn-lg.btn-xl.btn-block.play-now");
        }

        /// <summary>
        /// Clicks on "Add to Cart and Play More Games"
        /// </summary>
        public void ClickAddToCartButton()
        {
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
            return _visibleElementFromList("#add-to-cart");
        }


        /// <summary>
        /// Returns the first visible element from list of elements that are found by the same cssSelector.
        /// </summary>
        /// <param name="cssSelector">CssSelector of list of elements</param>
        /// <returns>Element/null</returns>
        private IWebElement _visibleElementFromList(string cssSelector)
        {
            IList<IWebElement> buttons = Driver.FindElements(By.CssSelector(cssSelector));
            foreach (IWebElement button in buttons)
            {
                if (button.Displayed)
                    return button;
            }

            return null;
        }
    }
}
