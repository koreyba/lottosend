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
