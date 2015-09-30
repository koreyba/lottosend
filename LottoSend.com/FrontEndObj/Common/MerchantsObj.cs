using System;
using System.Threading;
using LottoSend.com.FrontEndObj.GamePages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj.Common
{
    /// <summary>
    /// Contains all merchants (ways to pay) on front-end
    /// </summary>
    public class MerchantsObj : DriverCover
    {
        public MerchantsObj(IWebDriver driver) : base(driver)
        {
            //TODO make validation

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "input[id$='merchant_27'] + img.merchant")]
        private IWebElement _neteller;

        [FindsBy(How = How.CssSelector, Using = "input[id$='merchant_22'] + img.merchant")]
        private IWebElement _eKontoePlatby;

        [FindsBy(How = How.CssSelector, Using = "input#credit_card + img.merchant")]
        private IWebElement _offline;

        [FindsBy(How = How.CssSelector, Using = "input[id$='merchant_18'] + img.merchant")]
        private IWebElement _trustPay;

        /// <summary>
        /// Pays with trustpay (can success or fail the payment)
        /// </summary>
        /// <param name="isSuccessful"></param>
        public void PayWithTrustPay(bool isSuccessful)
        {
            //TODO
            Thread.Sleep(TimeSpan.FromSeconds(1));
            WaitForElement(_trustPay, 15);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _trustPay.Click();
            WaitForPageLoading();
            bool w = WaitjQuery();

            Driver.FindElement(By.CssSelector("#ctl00_cphContent_pStep4Header > div > div > div.jquery-selectbox-moreButton")).Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IWebElement countrySelect = Driver.FindElement(By.CssSelector("span.jquery-selectbox-item.value-SK.item-12"));
            countrySelect.Click();
           // ChooseElementInSelect("SK", countrySelect, SelectBy.Value);
            Thread.Sleep(TimeSpan.FromSeconds(2));

            IWebElement trustPayAuthorization = Driver.FindElement(By.CssSelector("div.step2Banks > a > img[id^='ct']"));
            trustPayAuthorization.Click();
            Thread.Sleep(TimeSpan.FromSeconds(2));

            IWebElement payNow = Driver.FindElement(By.CssSelector("#ctl00_cphContent_iPayOnline"));
            payNow.Click();
           // bool v = WaitjQuery();
            WaitForPageLoading();

            if (isSuccessful)
            {
                IWebElement ok = Driver.FindElement(By.CssSelector("#btnOK"));
                ok.Click();
            }
            else
            {
                IWebElement fail = Driver.FindElement(By.CssSelector("#btnFAIL"));
                fail.Click();
            }

            bool a = WaitjQuery();
            WaitForPageLoading();

            IWebElement returnToMerchant = Driver.FindElement(By.CssSelector("span[id$='ContinueMiddle']"));
            returnToMerchant.Click();

            WaitForPageLoading();

        }

        /// <summary>
        /// Provides payment using Neteller
        /// </summary>
        public void PayWithNeteller()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            WaitForElement(_neteller, 15);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _neteller.Click();
            WaitForPageLoading();
            bool w = WaitjQuery();
            
            //TODO: change to something
            Thread.Sleep(TimeSpan.FromSeconds(3));
            bool a = WaitAjax();

            IWebElement login = WaitForElement(By.CssSelector("#login_identity"), 10);
            login.SendKeys("451823760529");

            //TODO: change to something
            Thread.Sleep(TimeSpan.FromSeconds(1));
            IWebElement password = WaitForElement(By.CssSelector("#password"), 10);
            password.SendKeys("NTt3st1!");

            //TODO: change to something
            Thread.Sleep(TimeSpan.FromSeconds(1));
            IWebElement continueBtn = WaitForElement(By.CssSelector("#checkout_continue_btn"), 10);
            continueBtn.Click();
            WaitForPageLoading();

            IWebElement completeOrder = Driver.FindElement(By.CssSelector("#checkout_button"));
            completeOrder.Click();
            WaitForPageLoading();

            IWebElement returnToLottosend = Driver.FindElement(By.CssSelector("div.panel-footer > a.button"));
            returnToLottosend.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// Provides payment with offline charge
        /// </summary>
        public void PayWithOfflineCharge()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            WaitForElement(_offline, 10);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _offline.Click();
            WaitjQuery();

            _fillInFields();
        }

        /// <summary>
        /// Fills in all necesarry fields 
        /// </summary>
        private void _fillInFields()
        {
            OfflineChargeFormObj form = new OfflineChargeFormObj(Driver);
            form.FilInForm();
            form.ClickNextButton();
        }

        public void ClickOnEKontoEPlatby()
        {
            //TODO
            
            _eKontoePlatby.Click();
           
        }

        
    }
}
