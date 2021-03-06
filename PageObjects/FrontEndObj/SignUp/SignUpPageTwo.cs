﻿using System;
using OpenQA.Selenium;

namespace TestFramework.FrontEndObj.SignUp
{
    public class SignUpPageTwo : SignUpNewForm
    {
        public SignUpPageTwo(IWebDriver driver) : base(driver)
        {
            if(!Driver.Url.EndsWith("up2") && !Driver.Url.EndsWith("up2/"))
            {
                throw new Exception("It's not sign_up2 page. Current URL is: " + Driver.Url + " ");
            }
        }
    }
}
