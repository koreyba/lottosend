using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj.Login
{
    public class SignInPageOneObj : LogInPopUpObj
    {

        /// <summary>
        /// Page Object for sign in 1 page. It enharits all elements from LogInPopUpObj
        /// </summary>
        /// <param name="driver"></param>
        public SignInPageOneObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        protected override void ValidateObject()
        {
            if (!Driver.Url.Contains("web_users/sign_in"))
            {
                throw new Exception("It's not log in 1 page (URL is wrong");
            }
        }
    }
}
