using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LottoSend.com.FrontEndObj.SignUp;

namespace LottoSend.com.FrontEndObj
{
    public class TopBarObj : DriverCover
    {
        public TopBarObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        #region locators 

        [FindsBy(How = How.Id, Using="signup")]
        private IWebElement _signUp;

        [FindsBy(How = How.Id, Using = "signin")]
        private IWebElement _logIn;

        [FindsBy(How = How.CssSelector, Using = "div.navbar-right.navbar-form.text-right > span.text-primary")]
        private IWebElement _myAccount;

        #endregion 

        #region elements

        /// <summary>
        /// My Account button (only when logged in)
        /// </summary>
        public IWebElement MyAccount
        {
            get { return _myAccount; }
        }

        /// <summary>
        /// Sign Up Button in the top bar
        /// </summary>
        public IWebElement SignUpButton
        {
            get { return _signUp; }
        }

        /// <summary>
        /// Log In button in the top bar
        /// </summary>
        public IWebElement LogInButton
        {
            get { return _logIn; }
        }

        #endregion

        #region methods

        public SignUpPopUpObj ClickSignUpButton()
        {
            SignUpButton.Click();
            WaitAjax();

            return new SignUpPopUpObj(Driver);
        }

        public LogInPopUpObj ClickLogInButton()
        {
            LogInButton.Click();
            WaitjQuery();
            WaitAjax();

            return new LogInPopUpObj(Driver);
        }

        #endregion 
    }
}
