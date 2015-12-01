using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj.Common
{
    /// <summary>
    /// PageObject of checkout box with prices and discounts and coupon
    /// </summary>
    public class CheckoutObj : DriverCover
    {
        public CheckoutObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.FindElement(By.CssSelector(".page-header > h1")).Displayed)
            {
                throw new Exception("Sorry it must be not checkout. Check if checkout is displayed. Current page: " + Driver.Url + " ");
            }
        }

        [FindsBy(How = How.CssSelector, Using = ".btn.btn-info.show-coupon")]
        private IWebElement _doYouHaveCouponButton;

        [FindsBy(How = How.CssSelector, Using = ".form-group > input.btn-info")]
        private IWebElement _applyButton;

        [FindsBy(How = How.CssSelector, Using = ".form-group > #code")]
        private IWebElement _codeInput;

        [FindsBy(How = How.CssSelector, Using = "table.table.order > tbody > tr.coupon > td.text-success.text-center")]
        private IWebElement _discountCoupon;

        [FindsBy(How = How.CssSelector, Using = "table.table.order > tbody > tr.bulk-buy> td.text-success.text-center")]
        private IWebElement _discountBulk;

        [FindsBy(How = How.CssSelector, Using = "#cart-resume > div > table.table.order > tbody > tr.blue >td.text-center > strong")]
        private IWebElement _totalPrice;

        /// <summary>
        /// Gets total price at checkout
        /// </summary>
        public double TotalPrice
        {
            get { return _totalPrice.Text.ParseDouble(); }
        }

        /// <summary>
        /// Returns current multi-draw discount
        /// </summary>
        public double DiscountMultiDraw
        {
            get { return _discountBulk.Text.ParseDouble(); }
        }

        /// <summary>
        /// Returns current discount from coupon
        /// </summary>
        public double DiscountCoupon
        {
            get { return _discountCoupon.Text.ParseDouble(); }
        }

        /// <summary>
        /// Applies a coupon
        /// </summary>
        /// <param name="code"></param>
        public void ApplyCoupon(string code)
        {
            _doYouHaveCouponButton.Click();
            WaitjQuery();
            _codeInput.SendKeys(code);
            _applyButton.Click();
            WaitjQuery();
        }
    }
}
