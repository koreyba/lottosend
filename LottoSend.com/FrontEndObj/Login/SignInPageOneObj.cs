using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj.Login
{
    /// <summary>
    /// Page Object for sign in 1 page. It enharits all elements from LogInPopUpObj
    /// </summary>
    public class SignInPageOneObj : LogInPopUpObj
    {
        public SignInPageOneObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        protected override void ValidateObject()
        {
            if (!Driver.Url.Contains("web-users/sign-in"))
            {
                throw new Exception("It's not log in 1 page (URL is wrong");
            }
        }
    }
}
