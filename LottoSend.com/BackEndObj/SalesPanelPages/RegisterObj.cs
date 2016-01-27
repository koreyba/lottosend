using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.SalesPanelPages
{
    /// <summary>
    /// Page object of the sales panel
    /// </summary>
    public class RegisterObj : TabsObj
    {
        public RegisterObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("admin/orders"))
            {
                throw new Exception("Sorry it must be not the sales panel page. Please check it. Current page is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "input#web_user_user_id")]
        private IWebElement _userID;

        [FindsBy(How = How.CssSelector, Using = "input#web_user_email")]
        private IWebElement _email;

        [FindsBy(How = How.CssSelector, Using = "a.button.find-client")] 
        private IWebElement _okLogInButton;

         [FindsBy(How = How.CssSelector, Using = "a.button.clear-client")] 
        private IWebElement _clearButton;

        [FindsBy(How = How.CssSelector, Using = "input#web_user_first_name")]
        private IWebElement _firstName;

        [FindsBy(How = How.CssSelector, Using = "input#web_user_last_name")]
        private IWebElement _lastName;

        [FindsBy(How = How.CssSelector, Using = "input#web_user_phone")]
        private IWebElement _phone;

        [FindsBy(How = How.CssSelector, Using = "input#web_user_address")]
        private IWebElement _address;

        [FindsBy(How = How.CssSelector, Using = "input#web_user_city")]
        private IWebElement _city;

        [FindsBy(How = How.CssSelector, Using = "input#web_user_zip_code")]
        private IWebElement _zipCode;

        [FindsBy(How = How.CssSelector, Using = "#new_web_user > input[type='submit'][name='commit']")]
        private IWebElement _okSignUpButton;

        [FindsBy(How = How.CssSelector, Using = "div.left > h2")]
        private IWebElement _usersNameH2;

        /// <summary>
        /// Checks if any user logged in the sales panel (if user ID and email fields are not empty)
        /// </summary>
        /// <returns></returns>
        public bool IsSignedIn()
        {
            if (_usersNameH2.Text.Length > 5)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Clicks on "Clear" button
        /// </summary>
        public void SignOut()
        {
            _clearButton.Click();
            WaitForPageLoading();
            WaitjQuery();
        }

        /// <summary>
        /// Signs in using email and checks if sign in was successful
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public void SignIn(string email)
        {
            _email.SendKeys(email);
            _okLogInButton.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// Signs in using id and checks if sign in was successful
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void SignIn(int id)
        {
            _userID.SendKeys(id.ToString(System.Globalization.CultureInfo.InvariantCulture));
            _okLogInButton.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// Signs up with expected email and checks if were signed in
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string SignUp(string email)
        {
            _email.SendKeys(email);
            _firstName.SendKeys(RandomGenerator.GenerateRandomString(5));
            _lastName.SendKeys(RandomGenerator.GenerateRandomString(5));
            _phone.SendKeys("+380664176543");
            _address.SendKeys(RandomGenerator.GenerateRandomString(5));
            _city.SendKeys(RandomGenerator.GenerateRandomString(5));
            _zipCode.SendKeys("04071");
            _okSignUpButton.Click();
            bool b = WaitAjax();
            WaitForPageLoading();

            return email;
        }
    }
}
