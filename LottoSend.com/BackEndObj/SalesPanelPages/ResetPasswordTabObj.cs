using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.SalesPanelPages
{
    /// <summary>
    /// PageObject of password recovery tab in the sales panel
    /// </summary>
    public class ResetPasswordTabObj : DriverCover
    {
        public ResetPasswordTabObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#change_password")]
        private IWebElement _changePasswordCheckBox;

        [FindsBy(How = How.CssSelector, Using = "#password_instructions_create_password")]
        private IWebElement _createPasswordRadioButton;

        [FindsBy(How = How.CssSelector, Using = "#Password")]
        private IWebElement _passwordInput;

        [FindsBy(How = How.CssSelector, Using = "#Confirmation")]
        private IWebElement _confirmInput;

        [FindsBy(How = How.CssSelector, Using = "#submit_password_option")]
        private IWebElement _submitButton;

        public void ChangePassword(string newPassword)
        {
            TabsObj tabs = new TabsObj(Driver);
            tabs.GoToResetPasswordTab();

            _changePasswordCheckBox.Click();
            WaitAjax();

            _createPasswordRadioButton.Click();
            WaitAjax();

            _passwordInput.SendKeys(newPassword);
            _confirmInput.SendKeys(newPassword);

            _submitButton.Click();
            WaitForPageLoading();
        }
    }
}
