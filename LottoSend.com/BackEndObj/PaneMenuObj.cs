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
    /// Page Object of the top panel (menu)
    /// </summary>
    public class PaneMenuObj : DriverCover
    {
        public PaneMenuObj(IWebDriver driver) : base(driver)
        {
            if(!Driver.Url.Contains("admin"))
            {
                throw new Exception("It's not admin panel ");
            }

            PageFactory.InitElements(Driver, this);
        }
    }
}
