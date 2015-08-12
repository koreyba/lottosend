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
    public class ChargeFormObj : DriverCover
    {
        public ChargeFormObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "select#transaction_state")]
        private IWebElement _transactionStatus;

        public void MakeSuccessfulTransaction()
        {
            SelectElement select = new SelectElement(_transactionStatus);
            select.DeselectByValue("succeed");
        }

        public void UpdateTransaction()
        {

        }
    }
}
