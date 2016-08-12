using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj
{
    public class OnlineMerchantsObj : DriverCover
    {
        public OnlineMerchantsObj(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        /// <summary>
        /// Fails online payment
        /// </summary>
        public void FailPayment()
        {
            WaitForPageLoading();
            Driver.FindElement(By.CssSelector(".error11bold")).Click();
            WaitForPageLoading();
            Driver.SwitchTo().Window(Driver.WindowHandles[1]);
            Driver.FindElement(By.CssSelector(".btn.btn-danger")).Click();
            Driver.SwitchTo().Window(Driver.WindowHandles[0]);
        }

        public void ApprovePayment()
        {
            //TODO
        }
    }
}
