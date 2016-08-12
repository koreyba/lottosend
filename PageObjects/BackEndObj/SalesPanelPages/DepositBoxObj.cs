using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj.SalesPanelPages
{
    /// <summary>
    /// Deposit box in the sales panel
    /// </summary>
    public class DepositBoxObj : DriverCover
    {
        public DepositBoxObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.FindElement(By.CssSelector("#new_deposit")).Enabled)
            {
                throw new Exception("Sorry but there is no deposit box on the current page. URL: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#deposit_total_other")]
        private IWebElement _otherRadio;

        [FindsBy(How = How.CssSelector, Using = "#deposit_total")]
        private IWebElement _otherInput;

        [FindsBy(How = How.CssSelector, Using = "#charge-deposit")]
        private IWebElement _chargeButton;

        /// <summary>
        /// Deposit custom amount of money (input value and clicks "charge" button)
        /// </summary>
        /// <param name="amount"></param>
        public void DepositOtherAmoun(double amount)
        {
            _otherRadio.Click();
            _otherInput.SendKeys(amount.ToString(System.Globalization.CultureInfo.InvariantCulture));

            _chargeButton.Click();
            WaitForPageLoading();
            WaitjQuery();
        }
    }
}
