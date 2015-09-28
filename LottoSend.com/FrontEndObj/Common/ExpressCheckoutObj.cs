using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LottoSend.com.FrontEndObj.SignUp;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj.Common
{
    public class ExpressCheckoutObj : DriverCover
    {
        public ExpressCheckoutObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "a.express.new-user.pull-left")]
        private IWebElement _newUser;

        [FindsBy(How = How.CssSelector, Using = "a.express.existing-user.pull-right")]
        private IWebElement _existingUser;

        /// <summary>
        /// Signs UP in express checkout
        /// </summary>
        /// <returns></returns>
        public string SignUp()
        {
            _newUser.Click();
            WaitjQuery();
            SignUpForm form = new SignUpForm(Driver);
            string email = form.FillInFields();
            form.ClickSignUp();

            return email;
        }

        /// <summary>
        /// Signs in in express checkout
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public void SignIn(string email, string password)
        {
            _existingUser.Click();
            WaitjQuery();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            LogInPopUpObj form = new LogInPopUpObj(Driver);
            form.FillInFields(email, password);
            form.ClickLogInButton();
        }
    }
}
