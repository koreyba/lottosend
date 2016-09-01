using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.GamePages
{
    /// <summary>
    /// Page object for the new regular play page (site - web)
    /// </summary>
    public class NewRegularGamePageObj : NewPlayPageObj
    {
        public NewRegularGamePageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.FindElement(By.XPath("//*[@data-id='single']")).GetAttribute("class").Equals("active"))
            {
                throw new Exception("Sorry, it must be not new regular game page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }


    }
}
