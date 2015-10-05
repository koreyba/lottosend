using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.SalesPanelPages
{
    public class SalesPanelCart : DriverCover
    {
        public SalesPanelCart(IWebDriver driver) : base(driver)
        {
            //TODO: make validation

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "a#internal-balance")]
        private IWebElement _payWithInternalBalance;

        [FindsBy(How = How.CssSelector, Using = "a#charge")]
        private IWebElement _charge;

        [FindsBy(How = How.CssSelector, Using = "span.internal-balance + div.footer > strong")]
        private IWebElement _total;

        [FindsBy(How = How.CssSelector, Using = "#cart > span.internal-balance")]
        private IWebElement _internalBalace;

        /// <summary>
        /// Returns InternalBalance in the cart
        /// </summary>
        public double InternalBalance
        {
            get { return _internalBalace.Text.ParseDouble(); }
        }

        /// <summary>
        /// Returns total price in the cart
        /// </summary>
        public double TotalBalance
        {
            get { return _total.Text.ParseDouble(); }
        }

        /// <summary>
        /// Pays with internal balance (click button)
        /// </summary>
        public void PayWithInternalBalance()
        {
            _payWithInternalBalance.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// Charge user with credit card (must be previously inputed), click button
        /// </summary>
        public void Charge()
        {
            _charge.Click();
            WaitForPageLoading();
        }
    }
}
