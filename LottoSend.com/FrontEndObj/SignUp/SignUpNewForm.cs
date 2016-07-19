using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj.SignUp
{
    /// <summary>
    /// Page object of the new sign up from (sign up 2)
    /// </summary>
    class SignUpNewForm : DriverCover
    {
        public SignUpNewForm(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//div[@class='control']/*[@id='web_user_email']")]
        protected IWebElement Email;

        [FindsBy(How = How.XPath, Using = "//div[@class='control']/*[@id='web_user_password']")]
        protected IWebElement Password;

        [FindsBy(How = How.XPath, Using = "//*[@id='web_user_dob_3i']")]
        protected IWebElement Day;

        [FindsBy(How = How.XPath, Using = "//*[@id='web_user_dob_2i']")]
        protected IWebElement Month;

        [FindsBy(How = How.XPath, Using = "//*[@id='web_user_dob_1i']")]
        protected IWebElement Year;

        [FindsBy(How = How.XPath, Using = "//*[@id='web_user_first_name']")]
        protected IWebElement FirstName;

        [FindsBy(How = How.XPath, Using = "//*[@id='web_user_last_name']")]
        protected IWebElement LastName;

        [FindsBy(How = How.XPath, Using = "//*[@id='web_user_phone_code']")]
        protected IWebElement PhonePrefix;

        [FindsBy(How = How.XPath, Using = "//*[@id='web_user_phone_number']")]
        protected IWebElement PhoneNumber;

        [FindsBy(How = How.XPath, Using = "//*[@class='btn btn-lg btn-success btn-xl']")]
        protected IWebElement SignUpButton;

        /// <summary>
        /// Fills in all fields and accepts privacy policy
        /// </summary>
        public string FillInFieldsWeb()
        {
            var email = SendKeys();
            Thread.Sleep(TimeSpan.FromSeconds(0.5));

            return email;
        }

        /// <summary>
        /// Fills in all fields and accepts privacy policy
        /// </summary>
        public string FillInFieldsMobile()
        {
            var email = SendKeys();
            Thread.Sleep(TimeSpan.FromSeconds(0.5));

            return email;
        }

        private string SendKeys()
        {
            FirstName.SendKeys("my first name");
            LastName.SendKeys("my last name");
            PhoneNumber.SendKeys("12345678");
            string email = RandomGenerator.GenerateRandomString(12) + "@grr.la";
            Email.SendKeys(email);
            Password.SendKeys("11111111");
            ChooseElementInSelect("1", Day, SelectBy.Value);
            ChooseElementInSelect("1", Month, SelectBy.Value);
            ChooseElementInSelect("12", Year, SelectBy.Index);

            return email;
        }

        /// <summary>
        /// Click on sign up button
        /// </summary>
        public SignUpSuccessObj ClickSignUp()
        {
            WaitForElement(SignUpButton, 2).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();

            return new SignUpSuccessObj(Driver);
        }

    }
}
