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


        [FindsBy(How = How.CssSelector, Using = "table.index_table > tr:nth-last-child")]
        private IWebElement _theLastPayment;

        public void ChargeTheLastPayment()
        {
            IWebElement charge = _theLastPayment.FindElement(By.LinkText("Charge"));

            charge.Click();
            WaitjQuery();
            WaitAjax();
        }
    }
}
