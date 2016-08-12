using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj.CMS
{
    /// <summary>
    /// Page object of CMS > Pages > edit key
    /// </summary>
    public class EditPageObj : EditingPages
    {
        public EditPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.FindElement(By.CssSelector("div.page-header > h2")).Text.Equals("Edit Page") &&
                !Driver.FindElement(By.CssSelector("div.page-header > h2")).Text.Equals("Edit Multiple Pages"))
            {
                throw new Exception("Sorry but it must be not 'Edit Page' page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }
    }
}
