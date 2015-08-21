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
            IWebElement tr = _findTrOfLottery(drawName);

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

        private IWebElement _findTrOfLottery(string drawName)
        {
            IList<IWebElement> trList = _drawsTable.FindElements(By.CssSelector("tr[id^='td-draw-'"));
            foreach(IWebElement tr in trList)
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
