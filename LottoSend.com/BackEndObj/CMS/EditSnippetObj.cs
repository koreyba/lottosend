using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.CMS
{
    /// <summary>
    /// Page object of CMS > Snippets > edit key
    /// </summary>
    public class EditSnippetObj : EditingPages
    {
        public EditSnippetObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.FindElement(By.CssSelector("div.page-header > h2")).Text.Equals("Edit Snippet") &&
                !Driver.FindElement(By.CssSelector("div.page-header > h2")).Text.Equals("Edit Multiple Snippets"))
            {
                throw new Exception("Sorry but it must be not 'Edit Snippet' page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }
    }
}
