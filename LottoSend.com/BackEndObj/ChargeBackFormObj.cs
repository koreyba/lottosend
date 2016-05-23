using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj
{
    /// <summary>
    /// Page object of ChargeBack form on the client order processing page
    /// </summary>
    public class ChargeBackFormObj : DriverCover
    {
        public ChargeBackFormObj(IWebDriver driver) : base(driver)
        {
            if (
                !Driver.FindElement(
                    By.XPath(
                        "//div[@class='ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix ui-draggable-handle']"))
                    .Displayed)
            {
                throw new Exception("Sorry but the ChargeBack form must have been not open ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//*[@id='transaction_comments_attributes_0_content']")]
        private IWebElement _commentField;

        [FindsBy(How = How.XPath, Using = "//*[@class='edit_transaction']/input[@type='submit']")]
        private IWebElement _submitButton;

        [FindsBy(How = How.XPath, Using = "//*[@class='edit_transaction']/input[@value='CHB']")]
        private IWebElement _chargeBackOption;

        [FindsBy(How = How.XPath, Using = "//*[@class='edit_transaction']/input[@value='CHBR']")]
        private IWebElement _chargeBackRSOption;

        [FindsBy(How = How.XPath, Using = "//*[@class='edit_transaction']/input[@value='RR']")]
        private IWebElement _rROption;

        /// <summary>
        /// Makes a chargeback with a selected status
        /// </summary>
        /// <param name="status"></param>
        public void ChargeBack(ChargeBackStatus status)
        {
            if (status == ChargeBackStatus.CHB)
            {
                _chargeBackOption.Click();
            }

            if (status == ChargeBackStatus.CHBR)
            {
                _chargeBackRSOption.Click();
            }

            if (status == ChargeBackStatus.RR)
            {
                _rROption.Click();
            }

            _commentField.SendKeys("It's a test comment");
            _submitButton.Click();
            WaitAjax();
        }
    }
}
