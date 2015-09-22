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
    /// Charge panel elemets
    /// </summary>
    public class ChargePanelObj : DriverCover
    {
        public ChargePanelObj(IWebDriver driver) : base(driver)
        {
            if(!Driver.Url.Contains("charge_panel_manager"))
            {
                throw new Exception("It's not charge panel page");
            }

            PageFactory.InitElements(Driver, this);
        }


        [FindsBy(How = How.CssSelector, Using = "table.index_table > tbody:nth-child(2) > tr:last-child")]
        private IWebElement _theLastPayment;

        /// <summary>
        /// Clicks on "Charge" button of the last payment on the first page
        /// </summary>
        /// <returns></returns>
        public ChargeFormObj ChargeTheLastPayment()
        {
            IList<IWebElement> lastButton = Driver.FindElements(By.CssSelector("span.last > a"));
            if (lastButton.Count > 0)
            {
                lastButton[0].Click(); // Go to the last page
                WaitForPageLoading();
            }

            IWebElement charge = _theLastPayment.FindElement(By.LinkText("Charge"));

            charge.Click();
            WaitjQuery();
            WaitAjax();

            return new ChargeFormObj(Driver);
        }
    }
}
