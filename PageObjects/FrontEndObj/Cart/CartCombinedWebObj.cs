using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.Cart
{
    /// <summary>
    /// PageObject of the cart on the new combined payment page for the site (not mobile!)
    /// </summary>
    public class CartCombinedWebObj : CartObj
    {
        public CartCombinedWebObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("payments/new"))
            {
                throw new Exception("It's not new combined payment page. Current URL is: " + Driver.Url + " ");
            }
            PageFactory.InitElements(Driver, this);
        }

        private string xPathFromTrToTrashIcon = "./td[5]//span[contains(@class, 'trash')]";
        private string xPathForlotteryNames = "//td[2]/div[1]";

        [FindsBy(How = How.XPath, Using = "//td[@class='text-right']//strong")]
        private IWebElement _totalPrice;

        [FindsBy(How = How.XPath, Using = "//tr[@id='#bet-{bet.id}']")]
        private IList<IWebElement> _allTickets;

        [FindsBy(How = How.CssSelector, Using = ".modal-footer > a.btn")]
        private IWebElement _yesImSureButton;

        /// <summary>
        /// Number of tickets in the cart
        /// </summary>
        public override int NumberOfTickets
        {
            get {return _allTickets.Count; }
        }

        /// <summary>
        /// Removes a specific ticket (by lottery name)
        /// </summary>
        /// <param name="lottery"></param>
        public override void DeleteTicket(string lottery)
        {
            IWebElement tr = _findTrOfTicket(lottery);
            tr.FindElement(By.XPath(xPathFromTrToTrashIcon)).Click();
            bool m = WaitAjax();
            WaitForElement(_yesImSureButton, 5).Click();
            WaitAjax();
            WaitForPageLoading();
        }

        private IWebElement _findTrOfTicket(string lottery)
        {
            IList<IWebElement> namesOfLottery = Driver.FindElements(By.XPath(xPathForlotteryNames));

            foreach (IWebElement name in namesOfLottery)
            {
                if (name.Text.ToLower().Contains(lottery.ToLower()))
                    return name.FindElement(By.XPath("../.."));
            }

            return null;
        }
    }
}
