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
