using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;

namespace LottoSend.com.BackEndObj
{
    /// <summary>
    /// Client order processing page
    /// </summary>
    public class OrderProcessingObj : DriverCover
    {
        public OrderProcessingObj(IWebDriver driver) : base (driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.LinkText, Using = "Authorize")]
        private IWebElement _authorizeButton;

        [FindsBy(How = How.CssSelector, Using = ".index_table > tbody > tr > td:nth-child(2) > table > body > tr > td:nth-child(1) > nth-child(6)")]
        private IWebElement _transID;

        [FindsBy(How = How.CssSelector, Using = "td[id*=payment]  tbody > tr:nth-child(1) > td:nth-child(1)")]
        private IWebElement _trid;

        /// <summary>
        /// TRID of the first record
        /// </summary>
        public string Trid
        {
            get { return _trid.Text; }
        }

        /// <summary>
        /// Click "Authorize" button for the last payment (the one at the top)
        /// </summary>
        public void AuthorizeTheLastPayment()
        {
            try
            {
                _authorizeButton.Click();
            }
            catch(NoSuchElementException e)
            {
                IList<IWebElement> unAuthorize = Driver.FindElements(By.LinkText("UnAuthorize"));
                if(unAuthorize.Count < 1)
                {
                    throw new NoSuchElementException("There is no \"Authorize\" button in Cliend Order Processing ");
                }
            }

            WaitjQuery();
        }
    }
}
