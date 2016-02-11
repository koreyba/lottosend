using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj.SignUp
{
    public class SignUpPageOneObj : SignUpForm
    {
        public SignUpPageOneObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("sign_up"))
            {
                throw new Exception("It's not sign up page. Please check ");
            }

            PageFactory.InitElements(Driver, this);
        }
    }
}
