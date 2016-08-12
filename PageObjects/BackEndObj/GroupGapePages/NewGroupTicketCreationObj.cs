using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj.GroupGapePages
{
    public class NewGroupTicketCreationObj : DriverCover
    {
        public NewGroupTicketCreationObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("aggrupations/new"))
            {
                throw new Exception("Sorry it must be not 'New Ticket' page. Pleaes check it. Curren page is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "select#aggrupation_lottery_id")]
        private IWebElement _lottery;

        [FindsBy(How = How.CssSelector, Using = "select#aggrupation_group_id")]
        private IWebElement _group;

        [FindsBy(How = How.CssSelector, Using = "input#aggrupation_shares")]
        private IWebElement _numberOfShares;

        [FindsBy(How = How.CssSelector, Using = "input#aggrupation_active")]
        private IWebElement _activeCheckBox;

        [FindsBy(How = How.CssSelector, Using = "input#aggrupation_discount")]
        private IWebElement _discount;

        [FindsBy(How = How.CssSelector, Using = "input#aggrupation_price")]
        private IWebElement _price;

        [FindsBy(How = How.CssSelector, Using = "table#lines + a.button")]
        private IWebElement _newLineButton;

        [FindsBy(How = How.CssSelector, Using = "li#aggrupation_submit_action > input")]
        private IWebElement _createAggrupationButton;

        [FindsBy(How = How.CssSelector, Using = "input[id$='specials'")]
        private IList<IWebElement> _specialsFields;

        [FindsBy(How = How.CssSelector, Using = "input[id$='commons'")]
        private IList<IWebElement> _commonsFields;

        /// <summary>
        /// Clicks "Create Aggrupation" button
        /// </summary>
        /// <returns>GroupsPageObj page object</returns>
        public GroupsPageObj ClickCreateButton()
        {
            _createAggrupationButton.Click();

            WaitForPageLoading();

            return new GroupsPageObj(Driver);
        }
        
        /// <summary>
        /// Fills in all fields except of lines
        /// </summary>
        /// <param name="lottery">Lottery name</param>
        /// <param name="group">Group name</param>
        /// <param name="numberOfShares"></param>
        /// <param name="isActive"></param>
        /// <param name="discount"></param>
        /// <param name="price"></param>
        public void FillInFields(string lottery, string group, int numberOfShares, bool isActive, double discount, double price)
        {
            ChooseElementInSelect(lottery, _lottery, SelectBy.Text); //select lottery
            ChooseElementInSelect(group, _group, SelectBy.Text); //select group
            _numberOfShares.SendKeys(numberOfShares.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!isActive)
            {
                _activeCheckBox.Click();
            }
            _discount.Clear();
            _discount.SendKeys(discount.ToString(System.Globalization.CultureInfo.InvariantCulture));
            _price.Clear();
            _price.SendKeys(price.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Adds new line with set commons and special
        /// </summary>
        /// <param name="commons">common numbers</param>
        /// <param name="special">special numbers</param>
        public void AddNewLine(string commons, string special)
        {
            _newLineButton.Click();

           _commonsFields[_commonsFields.Count - 1].SendKeys(commons);
           _specialsFields[_commonsFields.Count - 1].SendKeys(special);
        }
    }
}
