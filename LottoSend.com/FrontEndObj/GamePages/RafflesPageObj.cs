using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;

namespace LottoSend.com.FrontEndObj.GamePages
{
    /// <summary>
    /// Page Object of Front-Raffles page
    /// </summary>
    public class RafflesPageObj : DriverCover
    {
        public RafflesPageObj(IWebDriver driver) : base(driver)
        {
            if(!Driver.Url.Contains("raffles"))
            {
                throw new Exception("Sorry, it must be not \"Raffles\" page. Please check it ");
            }
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "table.table.resume > tbody > tr.total > td.text-right")]
        private IWebElement _totalPrice;

        /// <summary>
        /// Gets total price of added raffle tickets
        /// </summary>
        public double TotalPrice
        {
            get { return Convert.ToDouble(_totalPrice.Text.ParceDouble()); }
        }

        [FindsBy(How = How.CssSelector, Using = "input#raffle_submit")]
        private IWebElement _buyNowButton;

        /// <summary>
        /// Clicks on "Buy Now" button
        /// </summary>
        public CartObj ClickBuyNowButton()
        {
            _buyNowButton.Click();
            WaitjQuery();
            WaitForPageLoading();

            return new CartObj(Driver);
        }
    }
}
