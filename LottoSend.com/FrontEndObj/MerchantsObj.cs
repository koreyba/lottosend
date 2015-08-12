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
    public class MerchantsObj : DriverCover
    {
        public MerchantsObj(IWebDriver driver) : base(driver)
        {
            //TODO make validation

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "input#payment_merchant_22 + img.merchant")]
        private IWebElement _eKontoePlatby;

        [FindsBy(How = How.CssSelector, Using = "input#credit_card + img")]
        private IWebElement _offline;

        /// <summary>
        /// Provide payment with offline charge
        /// </summary>
        public void PayWithOfflineCharge()
        {
            waitForElement(_offline, 10);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _offline.Click();
            WaitjQuery();

            _fillInFields();
        }

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
