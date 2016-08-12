using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.FrontEndObj.Common
{
    /// <summary>
    /// Page object of merchants section on the new combined payment page
    /// </summary>
    public class MerchantsCombinedSectionObj : DriverCover
    {
        public  MerchantsCombinedSectionObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("payments/new/"))
            {
                throw new Exception("It's not new combined payment page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//input[@id='payment_transaction_site_merchant_id_208'] /following::a[1]")]
        private IWebElement _offlineMerchant;

        [FindsBy(How = How.XPath, Using = "//input[@id='payment_transaction_site_merchant_id_212'] /following::a[1]")]
        private IWebElement _invoiceMerchant;

        [FindsBy(How = How.XPath, Using = "//input[@id='payment_transaction_site_merchant_id_202'] /following::a[1]")]
        private IWebElement _poliMerchant;

        /// <summary>
        /// Click on a merchant in the left section to display its details
        /// </summary>
        /// <param name="merchant"></param>
        public void ClickOnMerchant(WayToPay merchant)
        {
            switch (merchant)
            {
                case WayToPay.Offline:
                {
                    _offlineMerchant.Click();
                }
                break;

                case WayToPay.Poli:
                {
                    _poliMerchant.Click();
                }
                break;

                case WayToPay.Invoice:
                {
                    _invoiceMerchant.Click();
                }
                break;
            }
        }
    }
}
