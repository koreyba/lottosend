﻿using OpenQA.Selenium;

namespace TestFramework.FrontEndObj.SignUp
{
    public class SignUpSuccessObj : DriverCover
    {
        public SignUpSuccessObj(IWebDriver driver) : base(driver)
        {
            //if(!Driver.Url.Contains("signup/success"))
            //{
            //    throw new Exception("It might be not signup/success page or sign up was unseccessful ");
            //}
        }
    }
}
