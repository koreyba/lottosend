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
    public class OfflineChargeFormObj : DriverCover
    {
        public OfflineChargeFormObj(IWebDriver driver) : base(driver)
        {
            //TODO validation

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "input#transaction_holder_name")]
        private IWebElement _name;

        [FindsBy(How = How.CssSelector, Using = "input#transaction_number")]
        private IWebElement _cardNumber;

        [FindsBy(How = How.Id, Using = "transaction_expiration_month")]
        private IWebElement _expirationDateM;

        [FindsBy(How = How.Id, Using = "transaction_expiration_year")]
        private IWebElement _expirationDateY;

        [FindsBy(How = How.CssSelector, Using = "input#transaction_code")]
        private IWebElement _cvvCode;

        [FindsBy(How = How.CssSelector, Using = "a.btn.btn-success.btn-xl.btn-block")]
        private IWebElement _nextButton;

        public void FilInForm()
        {
            _name.SendKeys("Selenium");
            _cardNumber.SendKeys("4580458045804580");
            SelectElement select = new SelectElement(_expirationDateM);
            select.SelectByValue("11");
            select = new SelectElement(_expirationDateY);
            select.SelectByValue("2019");
            _cvvCode.SendKeys("860");
        }

        public void ClickNextButton()
        {
            _nextButton.Click();
            WaitAjax();
            WaitForPageLoading();
        }
    }
}
