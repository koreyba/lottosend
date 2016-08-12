using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.SignUp
{
    public class SignUpForm : DriverCover
    {
        public SignUpForm(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.Id, Using = "web_user_first_name")]
        protected IWebElement FirstName;

        [FindsBy(How = How.Id, Using = "web_user_last_name")]
        protected IWebElement LastName;

        [FindsBy(How = How.Id, Using = "web_user_phone")]
        protected IWebElement Phone;

        [FindsBy(How = How.CssSelector, Using = "div.control>input#web_user_email")]
        protected IWebElement Email;

        [FindsBy(How = How.Id, Using = "web_user_address")]
        protected IWebElement Adress;

        [FindsBy(How = How.Id, Using = "web_user_country")]
        protected IWebElement Country;

        [FindsBy(How = How.CssSelector, Using = "div.control>input#web_user_password")]
        protected new IWebElement Password;

        [FindsBy(How = How.Id, Using = "web_user_password_confirmation")]
        protected IWebElement PasswordConfirm;

        [FindsBy(How = How.CssSelector, Using = "#web_user_has_accepted_tos + label")] //> small")]
        protected IWebElement CheckBoxAcceptanceMobile;

        [FindsBy(How = How.CssSelector, Using = "#web_user_has_accepted_tos")]
        protected IWebElement CheckBoxAcceptance;

        [FindsBy(How = How.CssSelector, Using = ".btn.btn-lg.btn-success.btn-xl.btn-block")]
        protected IWebElement SignUpButton;

        /// <summary>
        /// Fills in all fields and accepts privacy policy
        /// </summary>
        public string FillInFieldsWeb()
        {
            var email = SendKeys();
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            CheckBoxAcceptance.Click();

            return email;
        }

        /// <summary>
        /// Fills in all fields and accepts privacy policy
        /// </summary>
        public string FillInFieldsMobile()
        {
            var email = SendKeys();
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            CheckBoxAcceptanceMobile.Click();

            return email;
        }

        private string SendKeys()
        {
            FirstName.SendKeys("my first name");
            LastName.SendKeys("my last name");
            Phone.SendKeys("12345678");
            string email = RandomGenerator.GenerateRandomString(12) + "@grr.la";
            Email.SendKeys(email);
            Adress.SendKeys("new orlean))");
           // SelectElement select = new SelectElement(Country);
           // select.SelectByValue("UA");
            Password.SendKeys("11111111");
            PasswordConfirm.SendKeys("11111111");
            return email;
        }

        /// <summary>
        /// Click on sign up button
        /// </summary>
        public SignUpSuccessObj ClickSignUp()
        {
            WaitForElement((IWebElement) SignUpButton, 2).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();

            return new SignUpSuccessObj(Driver);
        }

    }
}
