using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj
{
    public class WebUsersPageObj : DriverCover
    {
        public WebUsersPageObj(IWebDriver driver) : base(driver)
        {
            if(!Driver.Url.Contains("web_users"))
            {
                throw new Exception("Sorry it must be not Back - Web_Users page, please check it ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#q_email_eq")]
        private IWebElement _emailInput;

        [FindsBy(How = How.CssSelector, Using = "input[name=commit]")]
        private IWebElement _filterButton;

        [FindsBy(How = How.CssSelector, Using = "table.index_table.index > tbody")]
        private IWebElement _table;

        /// <summary>
        /// Returns store credit money of the first users (at the top)
        /// </summary>
        /// <returns></returns>
        public double GetFirstRecordStoreCredit()
        {
            return _table.FindElement(By.CssSelector("tr > td:nth-child(8)")).Text.ParseDouble();
        }

        /// <summary>
        /// Returns real money of the first users (at the top)
        /// </summary>
        /// <returns></returns>
        public double GetFirstRecordRealMoney()
        {
            return _table.FindElement(By.CssSelector("tr > td:nth-child(7)")).Text.ParseDouble();
        }

        /// <summary>
        /// Inputs email and click Filter button
        /// </summary>
        /// <param name="email"></param>
        public void FilterByEmail(string email)
        {
            _emailInput.SendKeys(email);
            _filterButton.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// Search for a user on the current page
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool FindUserOnCurrentPage(string email)
        {
            bool isFound = false;

            IList<IWebElement> emails = _table.FindElements(By.CssSelector("tr > td.col-email"));
            foreach (var el in emails)
            {
                if (el.Text.Equals(email))
                {
                    isFound = true;
                }
            }

            return isFound;
        }
    }
}
