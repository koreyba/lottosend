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
    public class SignUpPopUpObj : DriverCover 
    {
        public SignUpPopUpObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.FindElement(By.CssSelector("div.modal-dialog.modal-sm > h4.modal-title.text-center")).Displayed)
            {
                throw new Exception("No pop up is displayed");
            }
            PageFactory.InitElements(Driver, this);
        }

        #region locators

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

        [FindsBy(How = How.Id, Using = "web_user_has_accepted_tos")]
        private IWebElement _checkBoxAcceptance;

        [FindsBy(How = How.CssSelector, Using = ".btn.btn-lg.btn-success.btn-xl.btn-block")]
        private IWebElement _signUpButton;

        #endregion

        #region elements

        public IWebElement FirstName
        {
            get { return _firstName; }
        }

        public IWebElement LastName
        {
            get { return _lastName; }
        }

        public IWebElement Phone
        {
            get { return _phone; }
        }

        public IWebElement Email
        {
            get { return _email; }
        }

        public IWebElement Adress
        {
            get { return _adress; }
        }

        /// <summary>
        /// Returns list of countries 
        /// </summary>
        public SelectElement Country
        {
            get
            {
                return new SelectElement(_country);
            }
        }

        public IWebElement Password
        {
            get { return _password; }
        }

        public IWebElement PasswordConfirmation
        {
            get { return _passwordConfirm; }
        }

        public IWebElement AccetpCheckBox
        {
            get{ return _checkBoxAcceptance;}
        }

        public IWebElement SignUpButton
        {
            get { return _signUpButton; }
        }

        #endregion 

        #region methods

        /// <summary>
        /// Fills in all fields and accepts privacy policy
        /// </summary>
        public void FillInFields()
        {
            FirstName.SendKeys("my first name");
            LastName.SendKeys("my last name");
            Phone.SendKeys("12345678");
            Email.SendKeys(RandomGenerator.GenerateRandomString(12) + "@ukr.net");
            Adress.SendKeys("new orlean))");
            Country.SelectByValue("UA");
            Password.SendKeys("11111111");
            PasswordConfirmation.SendKeys("11111111");
            AccetpCheckBox.Click();
        }

        /// <summary>
        /// Click on sign up button
        /// </summary>
        public SignUpSuccessObj ClickSignUp()
        {
            SignUpButton.Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();

            return new SignUpSuccessObj(Driver);
        }

        #endregion
    }
}
