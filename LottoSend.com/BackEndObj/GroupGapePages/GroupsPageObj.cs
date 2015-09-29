using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.GroupGapePages
{
    public class GroupsPageObj : DriverCover
    {
        public GroupsPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("groups"))
            {
                throw new Exception("Sorry, it must be not backoffice/groups page. Please check it. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "table#groups.index_table + table.index_table > tbody")]
        private IWebElement _table;

        /// <summary>
        /// Checks if a group exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsGroupExists(string name)
        {
            IWebElement tr = _findTrOfGroup(name);
            if (tr == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Removes a group from "groups" page
        /// </summary>
        /// <param name="name"></param>
        public void DeleteGroup(string name)
        {
            IWebElement tr = _findTrOfGroup(name);

            IWebElement delete = tr.FindElement(By.CssSelector("td:nth-last-child(1) > div > a.view_link.member_link"));

            delete.Click();

            WaitForPageLoading();
        }

        private IWebElement _findTrOfGroup(string name)
        {
            IList<IWebElement> trList = _table.FindElements(By.CssSelector("tr"));
            foreach (IWebElement tr in trList)
            {
                IWebElement id = tr.FindElement(By.CssSelector("td:nth-child(1)"));
                if (id.Text.Contains(name))
                {
                    return tr;
                }
            }

            return null;
        }
    }
}
