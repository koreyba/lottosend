using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.SalesPanelPages
{
    /// <summary>
    /// Page Object for "CC Details" tab in the sales panel
    /// </summary>
    public class CcDetails : Tabs
    {
        public CcDetails(IWebDriver driver) : base(driver)
        {
            //TODO: add validation

           PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "select#transaction_cc_type")]
        private IWebElement _type;

        [FindsBy(How = How.CssSelector, Using = "input#transaction_number")]
        private IWebElement _number;

        [FindsBy(How = How.CssSelector, Using = "select#transaction_expiration_month")]
        private IWebElement _exrirationDateMonth;

        [FindsBy(How = How.CssSelector, Using = "select#transaction_expiration_year")]
        private IWebElement _expirationDateYear;

        [FindsBy(How = How.CssSelector, Using = "input#transaction_code")]
        private IWebElement _code;

        [FindsBy(How = How.CssSelector, Using = "input#transaction_holder_name")]
        private IWebElement _holderName;

         [FindsBy(How = How.CssSelector, Using = "#new_transaction > input[type='submit']")]
        private IWebElement _okButton;

        /// <summary>
        /// Fill in CC Details form using exact type of card and the number
        /// </summary>
        /// <param name="type"></param>
        /// <param name="number"></param>
        public void InputCcDetails(string type, string number)
        {
            ChooseElementInSelect(type, _type, SelectBy.Text);
            _number.SendKeys(number);
            ChooseElementInSelect("2017", _expirationDateYear, SelectBy.Text);
            _code.SendKeys("123");
            _holderName.SendKeys("Selenium");

            _okButton.Click();
            WaitForPageLoading();
        }
    }
}
