using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
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
    public class SignInPageTwoObj : LogInPopUpObj
    {
        public SignInPageTwoObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        protected override void ValidateObject()
        {
            if (!Driver.Url.Contains("web_users/sign_in2"))
            {
                throw new Exception("It's not log in 1 page (URL is wrong");
            }
        }

        [FindsBy(How = How.CssSelector, Using = "div.control > input#web_user_coupon")]
        private IWebElement _coupon;

        public void FillInFields(string email, string password)
        {
            _coupon.SendKeys("POKUPON2015");
            base.FillInFields(email, password);
        }
    }
}
