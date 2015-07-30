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

        public void PayWithOfflineCharge()
        {
            WaitAjax();
            _offline.Click();
            WaitAjax();

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
