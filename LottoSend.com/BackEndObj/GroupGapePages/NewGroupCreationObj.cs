using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.GroupGapePages
{
    /// <summary>
    /// Page Object of "New Group Creation" page
    /// </summary>
    public class NewGroupCreationObj : DriverCover
    {
        public NewGroupCreationObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("groups/new"))
            {
                throw new Exception("Sorry, it must be not new group creation page. Check it. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "input#group_name")]
        private IWebElement _nameOfGroup;

        [FindsBy(How = How.CssSelector, Using = "#group_submit_action > input")]
        private IWebElement _createGroup;

        /// <summary>
        /// Creates a new group for group tickets
        /// </summary>
        /// <param name="name"></param>
        public GroupsPageObj CreateNewGroup(string name)
        {
            _nameOfGroup.SendKeys(name);
            _createGroup.Click();
            WaitForPageLoading();

            return new GroupsPageObj(Driver);
        }
    }
}
