using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj.GroupGapePages
{
    public class GroupsPageObj : DriverCover
    {
        /// <summary>
        /// Page object of backoffice/groups page
        /// </summary>
        /// <param name="driver"></param>
        public GroupsPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("groups"))
            {
                throw new Exception("Sorry, it must be not backoffice/groups page. Please check it. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "table.index_table + table.index_table > tbody")]
        private IWebElement _table;

        /// <summary>
        /// Checks if a group ticket with lottery name and exact numbers exists
        /// </summary>
        /// <param name="group"></param>
        /// <param name="lottery"></param>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public bool IsTicketExists(string group, string lottery, string numbers)
        {
            IWebElement tr = _findTrOfGroup(group);
            string realLottery = tr.FindElement(By.CssSelector("td:nth-child(2)")).Text;
            //string realNumbers = tr.FindElement(By.CssSelector("td:nth-child(5)")).Text;

            return (realLottery.Contains(lottery)); //&& realNumbers.Contains(numbers));
        }

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

        /// <summary>
        /// Removes the first ticket in the group
        /// </summary>
        /// <param name="groupName"></param>
        public void DeleteTicket(string groupName)
        {
            IWebElement tr = _findTrOfGroup(groupName);

            IWebElement delete = tr.FindElement(By.CssSelector("td:nth-last-child(2) > div > a.view_link.member_link"));

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
