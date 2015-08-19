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
    /// Page Object of BackOffice/Transactions page
    /// </summary>
    public class TransactionsObj : DriverCover
    {
        public TransactionsObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("transactions"))
            {
                throw new Exception("It's not Transaction page ");
            }
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "table.index_table > tbody > tr:nth-child(2)")]
        private IWebElement _firstRecord;

        /// <summary>
        /// Checks if email in the first record is equal with the one you send
        /// </summary>
        /// <returns></returns>
        public bool CheckEmail(string email)
        {
            IWebElement emailTD = _firstRecord.FindElement(By.CssSelector("td:nth-child(3)"));
            if(emailTD.Text.Contains(email))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if marchant in the fist record is equal with the one you send
        /// </summary>
        /// <param name="merchant"></param>
        /// <returns></returns>
        public bool CheckMerchant(string merchant)
        {
            IWebElement merchantTD = _firstRecord.FindElement(By.CssSelector("td:nth-child(4)"));
            IList<IWebElement> link = merchantTD.FindElements(By.CssSelector("a"));
           
            if (link.Count == 0 || link.Count < 0)
                return false;
            
            if(link[0].Text.Contains(merchant))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if the time of the first record is more then current time - N minutes
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minuts"></param>
        /// <returns></returns>
        public bool CheckTime(int minBefore)
        {
            IWebElement dateTD = _firstRecord.FindElement(By.CssSelector("td:nth-child(12)"));

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
