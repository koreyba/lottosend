using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.WebUsersPages
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

        [FindsBy(How = How.CssSelector, Using = "td.col-authentication_token")]
        private IWebElement _generateTokenButton;

        [FindsBy(How = How.CssSelector, Using = ".col-view > a")]
        private IWebElement _viewButton;

        /// <summary>
        /// Click on "View" button for the first record on the page
        /// </summary>
        /// <returns></returns>
        public ViewWebUserPageObj ClickViewButton()
        {
            _viewButton.Click();
            WaitForPageLoading();

            return new ViewWebUserPageObj(Driver);
        } 

        /// <summary>
        /// Gets user token. Generates it if it's not generated
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GetUserToken(string email)
        {
            FilterByEmail(email);
            if (_generateTokenButton.FindElements(By.TagName("a")).Count > 0)
            {
                _generateTokenButton.Click();
                return _generateTokenButton.Text;
            }
            else
            {
                return _generateTokenButton.Text;
            }
        }

        /// <summary>
        /// Clicks on a "View Bets" button for the first user on the page
        /// </summary>
        public BetsPageObj ViewBets()
        {
            _table.FindElement(By.LinkText("View Bets")).Click();
            WaitForPageLoading();

            return new BetsPageObj(Driver);
        }

        /// <summary>
        /// Credits an amount of money to the store credit balance of a user
        /// </summary>
        /// <param name="amount"></param>
        public void CreditToStoreCredit(string amount)
        {
            _table.FindElement(By.LinkText("New Transaction")).Click();
            WaitAjax();
            IWebElement transactionType = GetFirstVisibleElementFromList(By.CssSelector("#transaction_transaction_type"));

            ChooseElementInSelect("credit_to_store_credit", transactionType, SelectBy.Value);

            IWebElement amountField = GetFirstVisibleElementFromList(By.CssSelector("#transaction_amount"));
            amountField.Clear();
            amountField.SendKeys(amount);

            IWebElement addTransactionButton = GetFirstVisibleElementFromList(By.CssSelector("#add_transaction_button"));
            addTransactionButton.Click();

            WaitAjax();
        }

        /// <summary>
        /// Credits an amount of money to the real money balance of the a user
        /// </summary>
        /// <param name="amount"></param>
        public void CreditToRealMoney(string amount)
        {
            _table.FindElement(By.LinkText("New Transaction")).Click();
            WaitAjax();
            IWebElement transactionType = GetFirstVisibleElementFromList(By.CssSelector("#transaction_transaction_type"));

            ChooseElementInSelect("credit_to_real_money", transactionType, SelectBy.Value);

            IWebElement amountField = GetFirstVisibleElementFromList(By.CssSelector("#transaction_amount"));
            amountField.Clear();
            amountField.SendKeys(amount);

            IWebElement addTransactionButton = GetFirstVisibleElementFromList(By.CssSelector("#add_transaction_button"));
            addTransactionButton.Click();

            WaitAjax();
        }

        /// <summary>
        /// Returns store credit money of the first users (at the top)
        /// </summary>
        /// <returns></returns>
        public double GetFirstRecordStoreCredit()
        {
            return _table.FindElement(By.CssSelector("tr > td:nth-child(10)")).Text.ParseDouble();
        }

        /// <summary>
        /// Returns real money of the first users (at the top)
        /// </summary>
        /// <returns></returns>
        public double GetFirstRecordRealMoney()
        {
            return _table.FindElement(By.CssSelector("tr > td:nth-child(9)")).Text.ParseDouble();
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
