using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.TestCases.BackOffice
{
    /// <summary>
    /// Page object of back/web_users/view page
    /// </summary>
    public class ViewWebUserPageObj : DriverCover
    {
        public ViewWebUserPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("web_users"))
            {
                throw new Exception("This is not view web_user page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#black-link > a")]
        private IWebElement _blacklistLink;

        [FindsBy(How = How.CssSelector, Using = "#web_user_blacklist + textarea")]
        private IWebElement _commentInput;

        [FindsBy(How = How.CssSelector, Using = ".blacklist > input[type=submit]")]
        private IWebElement _addCommentButton;

        /// <summary>
        /// Removes the user from the blacklist
        /// </summary>
        /// <param name="comment"></param>
        public void RemoveFromBlackList(string comment)
        {
            _blacklistLink.Click();
            WaitAjax();
            AddComment(comment);
        }

        /// <summary>
        /// Adds the user to the blacklist
        /// </summary>
        /// <param name="comment"></param>
        public void AddToBlackList(string comment)
        {
            _blacklistLink.Click();
            WaitAjax();
            AddComment(comment);
        }

        /// <summary>
        /// Adds the comment in Blacklist form and submit the form
        /// </summary>
        /// <param name="text"></param>
        protected void AddComment(string text)
        {
            _commentInput.SendKeys(text);
            _addCommentButton.Click();
            WaitForPageLoading();
        }
    }
}
