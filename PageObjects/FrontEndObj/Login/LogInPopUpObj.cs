﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestFramework.FrontEndObj.Common;

namespace TestFramework.FrontEndObj.Login
{
    /// <summary>
    /// Describes elements of Log In pop up form
    /// </summary>
    public class LogInPopUpObj : DriverCover
    {
        public LogInPopUpObj(IWebDriver driver) : base (driver)
        {
            ValidateObject();

            PageFactory.InitElements(Driver, this);
        }

        protected virtual void ValidateObject()
        {
            //if (Driver.FindElement(By.CssSelector("h4.modal-title.text-center")).Text.Length < 5)
            //{
            //    throw new Exception("No pop up is displayed ");
            //}
        }

        [FindsBy(How = How.CssSelector, Using = "div.control > input#web_user_email")]
        private IWebElement _email;

        [FindsBy(How = How.CssSelector, Using = "div.control > input#web_user_password")]
        private IWebElement _password;

        [FindsBy(How = How.XPath, Using = "//input[contains(@class, 'btn-xl')]")]
        private IWebElement _loginButton;

        public IWebElement Email
        {
            get { return _email; }
        }

        public IWebElement Password
        {
            get { return _password; }
        }

        public void FillInFields(string email, string password)
        {
            Email.SendKeys(email);
            Password.SendKeys(password);
        }
         
        public TopBarObj ClickLogInButton()
        {
            _loginButton.Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();

            return new TopBarObj(Driver);
        }
    }
}
