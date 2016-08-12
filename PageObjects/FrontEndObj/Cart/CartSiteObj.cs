using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.Cart
{
    public class CartSiteObj : CartObj
    {
        public CartSiteObj(IWebDriver driver) : base(driver)
        {
            if (Driver.FindElements(By.CssSelector("table.table.table-striped")).Count <= 0)
            {
                throw new Exception("It's not the cart page");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#game div > table > tbody > tr.blue > td.text-center > strong")]
        private IWebElement _totalPrice;

        /// <summary>
        /// Gets total price of the order (mobile)
        /// </summary>
        public override double TotalPrice
        {
            get { return StringExtention.ParseDouble(_totalPrice.Text); }
        }

        /// <summary>
        /// Removes ticket from the cart found by name of a lottery
        /// </summary>
        /// <param name="lottery"></param>
        public void DeleteTicket(string lottery)
        {
            IWebElement tr = _findTrOfTicket(lottery);
            tr.FindElement(By.CssSelector("td.text-center > form > input.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Edits selected ticket (clicks "Edit" button)
        /// </summary>
        /// <param name="lottery"></param>
        public override void EditTicket(string lottery)
        {
            IWebElement tr = _findTrOfTicket(lottery);
            ScrollToView((IWebElement)tr.FindElement(By.CssSelector("td.text-center > a.btn.btn-info.btn-block.edit"))).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        private IWebElement _findTrOfTicket(string lottery)
        {
            IList<IWebElement> tr = _cartTable.FindElements(By.TagName("tr"));

            foreach (IWebElement t in tr)
            {
                IList<IWebElement> namesOfLottery = t.FindElements(By.CssSelector("td.text-center > img"));

                foreach (IWebElement name in namesOfLottery)
                {
                    if (name.GetAttribute("alt").ToLower().Equals(lottery.ToLower()))
                        return t;
                }
            }

            return null;
        }
    }
}
