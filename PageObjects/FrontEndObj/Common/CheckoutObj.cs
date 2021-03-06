﻿using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.Common
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

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = ".btn.btn-info.show-coupon")]
        private IWebElement _doYouHaveCouponButton;

        [FindsBy(How = How.CssSelector, Using = "input.btn-info")]
        private IWebElement _applyButton;

        [FindsBy(How = How.CssSelector, Using = "#reduction_code")]
        private IWebElement _codeInput;

        [FindsBy(How = How.CssSelector, Using = "table.table.order > tbody > tr.coupon > td.text-success.text-center")]
        private IWebElement _discountCoupon;

        [FindsBy(How = How.CssSelector, Using = "table.table.order > tbody > tr.bulk-buy> td.text-success.text-center")]
        private IWebElement _discountBulk;

        [FindsBy(How = How.CssSelector, Using = "table.table.order > tbody tr.blue > td.text-center > strong")]
        private IWebElement _totalPrice;

        [FindsBy(How = How.CssSelector, Using = "div.control > .btn.btn-lg.btn-success.btn-xl.btn-block")]
        private IWebElement _completeYourOrder;

        [FindsBy(How = How.CssSelector, Using = "table.table.order > tbody > tr:nth-child(1) > td.text-center > strong")]
        private IWebElement _subTotalPrice;

        [FindsBy(How = How.CssSelector, Using = ".btn.btn-info.pull-right")]
        private IWebElement _removeCoupon; 
        /// <summary>
        /// Gets sub-total price in the checkout
        /// </summary>
        public double SubTotalPrice
        {
            get { return StringExtention.ParseDouble(_subTotalPrice.Text); }
        }

        /// <summary>
        /// Gets total price at checkout
        /// </summary>
        public double TotalPrice
        {
            get { return StringExtention.ParseDouble(_totalPrice.Text); }
        }

        /// <summary>
        /// Returns current multi-draw discount
        /// </summary>
        public double DiscountMultiDraw
        {
            get { return StringExtention.ParseDouble(_discountBulk.Text); }
        }

        /// <summary>
        /// Returns current discount from coupon
        /// </summary>
        public double DiscountCoupon
        {
            get { return StringExtention.ParseDouble(_discountCoupon.Text); }
        }

        /// <summary>
        /// Removes applied coupon
        /// </summary>
        public void RemoveCoupon()
        {
            _removeCoupon.Click();
            WaitAjax();
        }

        /// <summary>
        /// Clicks on "Completer Your Order" button in order to pay with internal balance
        /// </summary>
        public void ClickCompleteYourOrderButton()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            _completeYourOrder.Click();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Applies a coupon
        /// </summary>
        /// <param name="code"></param>
        public void ApplyCoupon(string code)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            _doYouHaveCouponButton.Click();
            WaitjQuery();
            _codeInput.SendKeys(code);
            _applyButton.Click();
            WaitjQuery();
        }
    }
}
