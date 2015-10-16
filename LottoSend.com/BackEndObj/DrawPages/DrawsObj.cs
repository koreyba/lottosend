using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.DrawPages
{
    public class DrawsObj : DriverCover
    {
        public DrawsObj(IWebDriver driver) : base(driver)
        {
            if(!Driver.Url.Contains("draws"))
            {
                throw new Exception("It's not a bets page ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#index_table_draws > tbody:nth-child(4)")]
        private IWebElement _drawsTable;


        /// <summary>
        /// Serch for the first draw in the table with corresponding name and click on "View Bets"
        /// </summary>
        /// <param name="drawName"></param>
        public DrawObj GoToDrawPage(string drawName)
        {
            IWebElement tr = _findLotteryDraw(drawName);

            if(tr == null)
            {
                throw new Exception("Lottery's draws is not found on the first 9 pages! ");
            }

            _clickViewBetsButton(tr);

            WaitForPageLoading();
            WaitjQuery();
            WaitAjax();

            return new DrawObj(Driver);
        }

        /// <summary>
        /// Search for lottery draw navigating to first 5 pages in pagination
        /// </summary>
        /// <param name="drawName"></param>
        /// <returns></returns>
        private IWebElement _findLotteryDraw(string drawName)
        {
            int count = 2;

            while(count < 9) //will go through 9 first pages in order to find needed lottery draw
            {
                IWebElement tr = _findTrOfLottery(drawName);
                if(tr != null)
                {
                    return tr;
                }
                else
                {
                    _navigateToNextPage(count);
                    ++count;
                }
            }
            

            return null;
        }

        /// <summary>
        /// Click to the "count" page in pagination (only if the page is visible)
        /// </summary>
        /// <param name="count"></param>
        private void _navigateToNextPage(int count)
        {
            IList<IWebElement> pagnation = Driver.FindElements(By.CssSelector("nav.pagination > span.page > a"));
            foreach (IWebElement page in pagnation)
            {
                if (page.Text.Contains(count.ToString()))
                {
                    page.Click();
                    WaitForPageLoading();
                    WaitjQuery();
                    break;
                }
            }
        }


        /// <summary>
        /// On the current page search for <tr> of the lottery draw
        /// </summary>
        /// <param name="drawName"></param>
        /// <returns></returns>
        private IWebElement _findTrOfLottery(string drawName)
        {
            IList<IWebElement> trList = _drawsTable.FindElements(By.CssSelector("tr"));
            foreach (IWebElement tr in trList)
            {
                IWebElement name = tr.FindElement(By.CssSelector("td.text-center > img"));
                if (name.GetAttribute("alt").Contains(drawName))
                {
                    return tr;
                }
            }

            return null;
        }

        private void _clickViewBetsButton(IWebElement tr)
        {
            tr.FindElement(By.CssSelector("td:nth-child(6) > a:nth-child(5)")).Click();
        }
    }
}
