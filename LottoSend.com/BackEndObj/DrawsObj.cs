using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoSend.com.BackEndObj
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

        [FindsBy(How = How.CssSelector, Using = "div.index_as_table > table:nth-child(2) > tbody")]
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
                throw new Exception("Lottery's draws is not found, maybe it's not on the first page ");
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

            while(count < 5) //will go through 5 first pages in order to find needed lottery draw
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
            IList<IWebElement> trList = _drawsTable.FindElements(By.CssSelector("tr[id^='td-draw-'"));
            foreach (IWebElement tr in trList)
            {
                IWebElement name = tr.FindElement(By.CssSelector("td:nth-child(1) > div.draw-center-text"));
                if (name.Text.Contains(drawName))
                {
                    return tr;
                }
            }

            return null;
        }

        private void _clickViewBetsButton(IWebElement tr)
        {
            tr.FindElement(By.CssSelector("td:nth-child(5) > ul > li:nth-child(4) > a")).Click();
        }
    }
}
