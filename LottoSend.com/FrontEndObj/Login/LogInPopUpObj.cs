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

        [FindsBy(How = How.CssSelector, Using = "div.control > input.btn.btn-lg.btn-success.btn-xl.btn-block")]
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
