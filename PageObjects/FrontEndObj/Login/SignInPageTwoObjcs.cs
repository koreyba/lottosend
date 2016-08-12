using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.Login
{
    public class SignInPageTwoObjcs : LogInPopUpObj
    {
        public SignInPageTwoObjcs(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        protected override void ValidateObject()
        {
            if (!Driver.Url.Contains("web-users/sign-in2"))
            {
                throw new Exception("It's not log in 2 page (URL is wrong");
            }
        }
    }
}
