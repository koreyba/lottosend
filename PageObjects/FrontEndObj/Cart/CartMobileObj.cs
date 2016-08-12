using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.Cart
{
    public class CartMobileObj : CartObj
    {
        public CartMobileObj(IWebDriver driver) : base(driver)
        {
            if (Driver.FindElements(By.CssSelector("table.table.table-striped")).Count <= 0)
            {
                throw new Exception("It's not the cart page");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#game div > div.total >span > strong")]
        private IWebElement _totalPrice;

        /// <summary>
        /// Gets total price of the order (mobile)
        /// </summary>
        public override double TotalPrice
        {
            get { return StringExtention.ParseDouble(_totalPrice.Text); }
        }

        public override void DeleteTicket(string lottery)
        {
            IWebElement tr = _findTrOfTicket(lottery);
            tr.FindElement(By.CssSelector("td > form > input.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Edits selected ticket (clicks "Edit" button)
        /// </summary>
        /// <param name="lottery"></param>
        public override void EditRaffleTicketMobile(string lottery)
        {
            IWebElement tr = _findTrOfRaffleTicket(lottery);
            tr.FindElement(By.CssSelector("td > p > a.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        public override void DeleteRaffleTicket(string lottery)
        {
            IWebElement tr = _findTrOfRaffleTicket(lottery);
            tr.FindElement(By.CssSelector("td > form > input.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        private IWebElement _findTrOfRaffleTicket(string lottery)
        {
            IList<IWebElement> tr = _cartTable.FindElements(By.TagName("tr"));

            foreach (IWebElement t in tr)
            {
                IList<IWebElement> namesOfLottery = t.FindElements(By.CssSelector("td > strong > img"));

                foreach (IWebElement name in namesOfLottery)
                {
                    if (name.GetAttribute("alt").Contains(lottery))
                        return t;
                }
            }

            return null;
        }

        /// <summary>
        /// Edits selected ticket (clicks "Edit" button)
        /// </summary>
        /// <param name="lottery"></param>
        public override void EditTicket(string lottery)
        {
            IWebElement tr = _findTrOfTicket(lottery);
            ScrollUp();
            tr.FindElement(By.CssSelector("td > p > a.btn.btn-info.btn-block")).Click();
            WaitAjax();
            WaitForPageLoading();
            WaitAjax();
        }

        private IWebElement _findTrOfTicket(string lottery)
        {
            IList<IWebElement> tr = _cartTable.FindElements(By.TagName("tr"));

            foreach (IWebElement t in tr)
            {
                IList<IWebElement> namesOfLottery = t.FindElements(By.CssSelector("td > strong"));

                foreach (IWebElement name in namesOfLottery)
                {
                    if (name.Text.ToLower().Contains(lottery.ToLower()))
                        return t;
                }
            }

            return null;
        }
    }
}
