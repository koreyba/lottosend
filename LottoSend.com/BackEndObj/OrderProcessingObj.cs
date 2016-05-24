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

        [FindsBy(How = How.CssSelector, Using = "#q_web_user_email_eq")]
        private IWebElement _webUserEmailFilter;

        [FindsBy(How = How.CssSelector, Using = ".filter_date_range + input")]
        private IWebElement _filterButton;

        [FindsBy(How = How.CssSelector, Using = ".history > img")]
        private IList<IWebElement> _blButton;

        [FindsBy(How = How.XPath, Using = "//*[@class='index_table']//tbody//td/a[@class='button']")]
        private IWebElement _chargeBackButton;


        [FindsBy(How = How.XPath, Using = "//*[@class='index_table']//tbody//tr[1]/td[@class='history']//img")]
        private IWebElement _chargeBackImage;

        /// <summary>
        /// Returns "alt" attribute of the first charge back image
        /// </summary>
        public string ChargeBackImageText
        {
            get { return _chargeBackImage.GetAttribute("alt"); }
        }

        /// <summary>
        /// Checks if on the current page the BL button exist
        /// </summary>
        /// <returns></returns>
        public bool isBLImageExist()
        {
            if (_blButton.Count >= 1)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Filters orders by user's email
        /// </summary>
        /// <param name="email"></param>
        public void FilterByEmail(string email)
        {
            _webUserEmailFilter.SendKeys(email);
            _filterButton.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// TRID of the first record
        /// </summary>
        public string Trid
        {
            get { return _trid.Text; }
        }

        /// <summary>
        /// Click ChargeBack button for the first order
        /// </summary>
        public ChargeBackFormObj ClickChargeBackButton()
        {
            _chargeBackButton.Click();
            WaitAjax();

            return new ChargeBackFormObj(Driver);
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
