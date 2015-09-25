using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace LottoSend.com.FrontEndObj.SignUp
{
    public class SignUpPageOneObj : DriverCover
    {
        public SignUpPageOneObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("sign_up"))
            {
                throw new Exception("It's not sign up page. Please check ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.Id, Using = "web_user_first_name")]
        private IWebElement _firstName;

        [FindsBy(How = How.Id, Using = "web_user_last_name")]
        private IWebElement _lastName;

        [FindsBy(How = How.Id, Using = "web_user_phone")]
        private IWebElement _phone;

        [FindsBy(How = How.CssSelector, Using = "div.control>input#web_user_email")]
        private IWebElement _email;

        [FindsBy(How = How.Id, Using = "web_user_address")]
        private IWebElement _adress;

        [FindsBy(How = How.Id, Using = "web_user_country")]
        private IWebElement _country;

        [FindsBy(How = How.CssSelector, Using = "div.control>input#web_user_password")]
        private IWebElement _password;

        [FindsBy(How = How.Id, Using = "web_user_password_confirmation")]
        private IWebElement _passwordConfirm;

        [FindsBy(How = How.CssSelector, Using = "#web_user_has_accepted_tos + label > small")] 
        private IWebElement _checkBoxAcceptanceMobile;

        [FindsBy(How = How.CssSelector, Using = "#web_user_has_accepted_tos")]
        private IWebElement _checkBoxAcceptance;

        [FindsBy(How = How.CssSelector, Using = ".btn.btn-lg.btn-success.btn-xl.btn-block")]
        private IWebElement _signUpButton;

        /// <summary>
        /// Fills in all fields and accepts privacy policy
        /// </summary>
        public string FillInFields()
        {
            var email = SendKeys();
            _checkBoxAcceptance.Click();

            return email;
        }

        /// <summary>
        /// Fills in all fields and accepts privacy policy
        /// </summary>
        public string FillInFieldsMobile()
        {
            var email = SendKeys();
            _checkBoxAcceptanceMobile.Click();

            return email;
        }

        private string SendKeys()
        {
            _firstName.SendKeys("my first name");
            _lastName.SendKeys("my last name");
            _phone.SendKeys("12345678");
            string email = RandomGenerator.GenerateRandomString(12) + "@ukr.net";
            _email.SendKeys(email);
            _adress.SendKeys("new orlean))");
            SelectElement select = new SelectElement(_country);
            @select.SelectByValue("UA");
            _password.SendKeys("11111111");
            _passwordConfirm.SendKeys("11111111");
            return email;
        }

        /// <summary>
        /// Click on sign up button
        /// </summary>
        public SignUpSuccessObj ClickSignUp()
        {
            _signUpButton.Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();

            return new SignUpSuccessObj(Driver);
        }

    }
}
