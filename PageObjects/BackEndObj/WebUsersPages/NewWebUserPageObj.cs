using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj.WebUsersPages
{
    /// <summary>
    /// Page Object of a creating a new web user page in back office admin/web_users/new
    /// </summary>
    public class NewWebUserPageObj : DriverCover
    {
        public NewWebUserPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("web_users/new"))
            {
                throw new Exception("Sorry but it must be not back/new_web_user page. Current URL is:" + Driver.Url +
                                    " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#web_user_email")]
        private IWebElement _email;

        [FindsBy(How = How.CssSelector, Using = "#web_user_first_name")]
        private IWebElement _firstName;

        [FindsBy(How = How.CssSelector, Using = "#web_user_last_name")]
        private IWebElement _lastName;

        [FindsBy(How = How.CssSelector, Using = "#web_user_reset_password_2")]
        private IWebElement _createPasswordRadio;

        [FindsBy(How = How.CssSelector, Using = "#web_user_password")]
        private IWebElement _password;

        [FindsBy(How = How.CssSelector, Using = "#web_user_password_confirmation")]
        private IWebElement _passwordConfirm;

        [FindsBy(How = How.CssSelector, Using = "#web_user_site_id")]
        private IWebElement _site;

        [FindsBy(How = How.CssSelector, Using = "#web_user_phone")]
        private IWebElement _phone;

        [FindsBy(How = How.CssSelector, Using = "#web_user_address")]
        private IWebElement _address;

        [FindsBy(How = How.CssSelector, Using = "#web_user_submit_action > input")]
        private IWebElement _createWebUserButton;

        /// <summary>
        /// Creates a new web user with default password and returns its email
        /// </summary>
        /// <returns></returns>
        public string CreateWebUser()
        {
            string email = RandomGenerator.GenerateRandomString(10) + "@grr.la";

            _email.SendKeys(email);
            _firstName.SendKeys(RandomGenerator.GenerateRandomString(4));
            _lastName.SendKeys(RandomGenerator.GenerateRandomString(4));
            _createPasswordRadio.Click();
            _password.SendKeys("11111111");
            _passwordConfirm.SendKeys("11111111");
            ChooseElementInSelect("1", _site, SelectBy.Value);
            _phone.SendKeys("380996665544");
            _address.SendKeys("Kyiv");
            _createWebUserButton.Click();

            WaitForPageLoading();

            return email;
        }

    }
}
