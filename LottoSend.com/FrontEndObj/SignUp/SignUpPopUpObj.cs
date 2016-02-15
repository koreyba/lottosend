using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj.SignUp
{
    public class SignUpPopUpObj : SignUpForm 
    {
        public SignUpPopUpObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.FindElement(By.CssSelector("h4.modal-title.text-center")).Enabled)
            {
                throw new Exception("No pop up is displayed");
            }
            PageFactory.InitElements(Driver, this);
        }
    }
}
