using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;


namespace LottoSend.com.BackEndObj
{
    /// <summary>
    /// Log in admin page
    /// </summary>
    public class LoginObj : DriverCover 
    {
        public LoginObj(IWebDriver driver) : base(driver)
        {
            if(!Driver.Url.Contains("admin/login"))
            {
                throw new Exception("Sorry, it must be not admin/login page "); 
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "input#user_email")]
        private IWebElement _loginField;

        [FindsBy(How = How.CssSelector, Using = "input#user_password")]
        private IWebElement _password;

        [FindsBy(How = How.CssSelector, Using = "#user_submit_action > input")]
        private IWebElement _loginButton;

        /// <summary>
        /// Log in using login and password
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns>true if loged in successfuly</returns>
        public bool LogIn(string login, string password)
        {
            _loginField.SendKeys(login);
            _password.SendKeys(password);
            _loginButton.Click();

            WaitForPageLoading();
            WaitjQuery();

            Driver.FindElement(By.CssSelector("#page_title"));

            return true;
        }
    }
}
