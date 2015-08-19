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
    /// <summary>
    /// Page object of a draw page
    /// </summary>
    public class DrawObj : DriverCover
    {
        public DrawObj(IWebDriver driver) : base(driver)
        {
            if(!Driver.Url.Contains("bets") || !Driver.Url.Contains("draws"))
            {
                throw new Exception("Sorry, it's not a draw page ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "table.index_table > tbody > tr:nth-child(2)")]
        private IWebElement _firstRecord;

        /// <summary>
        /// Checks if email in the first record corresponds to the one you send
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool CheckEmail(string email)
        {
            IWebElement a = _firstRecord.FindElement(By.CssSelector("td:nth-child(6) > a"));

            if (!a.Text.Contains(email))
                return false;

            return true; 
        }

        /// <summary>
        /// Check if the time of the first record is more then current time - N minutes
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minuts"></param>
        /// <returns></returns>
        public bool CheckTime(int minBefore)
        {
            IWebElement dateTD = _firstRecord.FindElement(By.CssSelector("td:nth-child(3)"));
            string date = dateTD.Text;
            TimeSpan timeSpan = date.ParseTimeSpan();
            
            PaneMenuObj panel = new PaneMenuObj(Driver);
            string utcData = panel.GetUTCDate();

            TimeSpan extectedTime = utcData.ParseTimeSpan();

            //Check if record time more then current time - "minBefore"
            if (timeSpan > extectedTime - TimeSpan.FromMinutes(minBefore)) 
            {
                return true;
            }

            return false;
        }
    }
}
