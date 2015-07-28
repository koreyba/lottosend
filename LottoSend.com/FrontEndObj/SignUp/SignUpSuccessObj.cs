using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj
{
    public class SignUpSuccessObj : DriverCover
    {
        public SignUpSuccessObj(IWebDriver driver) : base(driver)
        {
            if(!Driver.Url.Contains("signup/success"))
            {
                throw new Exception("It might be not signup/success page or sign up was unseccessful");
            }
        }
    }
}
