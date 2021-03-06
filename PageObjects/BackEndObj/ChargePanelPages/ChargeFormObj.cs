﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace TestFramework.BackEndObj.ChargePanelPages
{
    /// <summary>
    /// Form that opens when "Charge" buttons is clicked in charge panel 
    /// </summary>
    public class ChargeFormObj : DriverCover
    {
        public ChargeFormObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "select#transaction_state")]
        private IWebElement _transactionStatus;

        /// <summary>
        /// Select "Fail" in payment status
        /// </summary>
        public void MakeTransactionFailed()
        {
            SelectElement select = new SelectElement(_transactionStatus);
            select.SelectByValue("failed");
        }

        /// <summary>
        /// Select "Succeed" in payment status
        /// </summary>
        public void MakeTransactionSucceed()
        {
            SelectElement select = new SelectElement(_transactionStatus);
            select.SelectByValue("succeed");
        }

        /// <summary>
        /// Click Update Transaction button
        /// </summary>
        public void UpdateTransaction()
        {
            Driver.FindElement(By.CssSelector("fieldset.inputs")).Submit();
            WaitAjax();
            WaitjQuery();
        }
    }
}
