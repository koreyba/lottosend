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
