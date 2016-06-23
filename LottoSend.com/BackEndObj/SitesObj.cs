using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj
{
    /// <summary>
    /// Page Object of admin/sites page with site's grid
    /// </summary>
    public class SitesObj : DriverCover
    {
        public SitesObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.EndsWith("sites/"))
            {
                throw new Exception("It's not admin/sites page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//*[@id='index_table_sites']/tbody/tr")]
        private IList<IWebElement> _records;

        /// <summary>
        /// Clears cache of a specific web site
        /// </summary>
        /// <param name="siteName"></param>
        public void ClearCache(string siteName)
        {
            foreach (var site in _records)
            {
                //IWebElement name = site.FindElement(By.XPath("/td[contains(@class, 'col-name')]"));
                IWebElement name = site.FindElement(By.CssSelector("td.col-name"));
                if (name.Text.Equals(siteName))
                {
                    name.FindElement(By.XPath("../td[contains(@class, 'col-cache')]/button")).Click();
                }
            }

            WaitAjax();
        }
    }
}
