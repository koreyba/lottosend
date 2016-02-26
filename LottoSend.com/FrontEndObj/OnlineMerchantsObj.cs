using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.FrontEndObj
{
    public class OnlineMerchantsObj : DriverCover
    {
        public OnlineMerchantsObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        public void FailPayment()
        {
          //  Driver.SwitchTo().Window(Driver.CurrentWindowHandle);
            WaitForPageLoading();
            //bool w = WaitAjax();
            Driver.FindElement(By.CssSelector(".error11bold")).Click();
            WaitForPageLoading();
            //bool m = WaitAjax();
            Driver.SwitchTo().Window(Driver.WindowHandles[1]);
            Driver.FindElement(By.CssSelector(".btn.btn-danger")).Click();
            Driver.SwitchTo().Window(Driver.WindowHandles[0]);
          //  bool s = WaitAjax();
            //WaitForPageLoading();
           // bool z = WaitAjax();
           // WaitForPageLoading();
        }
    }
}
