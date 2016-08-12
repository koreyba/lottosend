using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace TestFramework.FrontEndObj.GamePages
{
    /// <summary>
    /// Page Object of offline charge form (front-end, game page)
    /// </summary>
    public class OfflineChargeFormObj : DriverCover
    {
        public OfflineChargeFormObj(IWebDriver driver) : base(driver)
        {
            //TODO validation

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id, 'transaction_holder_name')]")]
        private IWebElement _name;

        [FindsBy(How = How.XPath, Using = "//input[contains(@id, 'transaction_number')]")]
        private IWebElement _cardNumber;

        [FindsBy(How = How.XPath, Using = "//select[contains(@id, 'transaction_expiration_month')]")]
        private IWebElement _expirationDateM;

        [FindsBy(How = How.XPath, Using = "//select[contains(@id, 'transaction_expiration_year')]")]
        private IWebElement _expirationDateY;

        [FindsBy(How = How.XPath, Using = "//input[contains(@id, 'transaction_code')]")]
        private IWebElement _cvvCode;

        [FindsBy(How = How.CssSelector, Using = ".btn.btn-success.btn-block")]
        private IWebElement _nextButton;

        /// <summary>
        /// Enters correct data in the form
        /// </summary>
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

        /// <summary>
        /// Click on "Next" button to submit the forms
        /// </summary>
        public void ClickNextButton()
        {
            _nextButton.Click();
            WaitAjax();
            WaitForPageLoading();
        }
    }
}
