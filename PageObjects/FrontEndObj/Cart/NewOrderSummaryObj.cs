using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.Cart
{
    /// <summary>
    /// Page object for a new "Order Summary" section on a new play page (site)
    /// </summary>
    public class NewOrderSummaryObj : DriverCover
    {
        public NewOrderSummaryObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.FindElement(By.CssSelector(".always-display > td")).Displayed)
            {
                throw new Exception("Sorry but the Order Summary block is not displayed. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "tr.bet")]
        private IList<IWebElement> _tickets;

        [FindsBy(How = How.CssSelector, Using = ".summary_button")]
        private IWebElement _proceedButton;

        [FindsBy(How = How.CssSelector, Using = ".js--type-money")]
        private IWebElement _totalPrice;

        [FindsBy(How = How.XPath, Using = "..//span[@class='plus']")]
        private IWebElement _addCouponButton;

        [FindsBy(How = How.CssSelector, Using = "#reduction_code")]
        private IWebElement _couponInputField;

        [FindsBy(How = How.CssSelector, Using = ".btn.btn-info")]
        private IWebElement _applyButton;

        [FindsBy(How = How.XPath, Using = "//*[@id='coupon-container']//button[@class='close']")]
        private IWebElement _closeButton;

        [FindsBy(How = How.XPath, Using = "//*[@class='coupon']/td[2]")]
        private IWebElement _couponDiscount;

        [FindsBy(How = How.CssSelector, Using = ".modal-footer > a.btn")]
        private IWebElement _yesImSureButton;

        /// <summary>
        /// Returns total price 
        /// </summary>
        public double TotalPrice
        {
            get { return StringExtention.ParseDouble(_totalPrice.Text); }
        }

        /// <summary>
        /// Returns the discount given an by applied coupon
        /// </summary>
        public double CouponDiscount
        {
            get { return StringExtention.ParseDouble(_couponInputField.Text); }
        }

        /// <summary>
        /// Total number of ticket is the "Order summary" block
        /// </summary>
        public int NumberOfTickets
        {
            get { return _tickets.Count; }
        }

        /// <summary>
        /// Removes a lottery ticket that includes sent to the method name
        /// </summary>
        /// <param name="lotteryName"></param>
        public void DeleteTicket(string lotteryName)
        {
            IWebElement ticket = _findTrOfTicket(lotteryName);
            ticket.FindElement(By.XPath("./td[2]/div/a[@class='destroy']")).Click();
            WaitForElement(_yesImSureButton, 10).Click();
            WaitAjax();
        }

        /// <summary>
        /// Searches for <tr> tag that includes lottery that contains specific name
        /// </summary>
        /// <param name="lottery"></param>
        /// <returns></returns>
        private IWebElement _findTrOfTicket(string lottery)
        {
            foreach (var element in _tickets)
            {
                if (element.FindElement(By.XPath("./td[1]/div[@class='name']")).Text.Contains(lottery))
                {
                    return element;
                }
            }

            return null;
        }

        /// <summary>
        /// Applies a coupon
        /// </summary>
        /// <param name="coupon"></param>
        public void ApplyCoupon(string coupon)
        {
            _addCouponButton.Click();
            WaitAjax();
            _couponInputField.SendKeys(coupon);
            _applyButton.Click();
            WaitAjax();
        }
    }
}
