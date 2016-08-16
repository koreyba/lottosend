using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestFramework.FrontEndObj.GamePages;

namespace TestFramework.FrontEndObj.Common
{
    /// <summary>
    /// Contains all merchants (ways to pay) on front-end
    /// </summary>
    public class MerchantsObj : DriverCover
    {
        public MerchantsObj(IWebDriver driver) : base(driver)
        {
            //TODO make validation

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "input[id$='merchant_135'] + img.merchant")]
        private IWebElement _neteller;

        [FindsBy(How = How.CssSelector, Using = "input[id$='merchant_72'] + img.merchant")]
        private IWebElement _eKontoePlatby;

        [FindsBy(How = How.CssSelector, Using = "input#credit_card + img.merchant")]
        private IWebElement _offline;

        [FindsBy(How = How.CssSelector, Using = "input[id$='merchant_68'] + img.merchant")]
        private IWebElement _trustPay;

        [FindsBy(How = How.CssSelector, Using = "input[id$='merchant_18'] + img.merchant")]
        private IWebElement _skrill;

        [FindsBy(How = How.CssSelector, Using = "input[id$='merchant_74'] + img.merchant")]
        private IWebElement _poli;
        
        [FindsBy(How = How.CssSelector, Using = "input[id$='merchant_73'] + img.merchant")]
        private IWebElement _moneta;

        public IWebElement Moneta
        {
            get { return _moneta; }
        }

        public IWebElement Poli
        {
            get { return _poli; }
        }

        public IWebElement eKontoePlatby
        {
            get { return _eKontoePlatby; }
        }

        /// <summary>
        /// Pays with Skrill
        /// </summary>
        public void PayWithSkrill()
        {
            //TODO
            Thread.Sleep(TimeSpan.FromSeconds(1));
            WaitForElement((IWebElement) _skrill, 15);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _skrill.Click();
            WaitForPageLoading();
            bool w = WaitjQuery();

            IWebElement alreadyHaveAccount = Driver.FindElement(By.CssSelector("#already_has_account"));
            alreadyHaveAccount.Click();
            bool q = WaitjQuery();

            IWebElement email = Driver.FindElement(By.CssSelector("#email"));
            email.Clear();
            email.SendKeys("ron@numgames.com");

            IWebElement password = Driver.FindElement(By.CssSelector("#password"));
            password.Clear();
            password.SendKeys("qwerty1234");

            IWebElement login = Driver.FindElement(By.CssSelector("div.button_inner"));
            login.Click();
            bool e = WaitjQuery();

            WaitForPageLoading();

            Thread.Sleep(TimeSpan.FromSeconds(1));

            IWebElement play = Driver.FindElement(By.CssSelector("#payConfirm"));
            play.Click();
            bool t = WaitjQuery();

            WaitForPageLoading();

            IWebElement backToMerchant = Driver.FindElement(By.CssSelector("div.button_inner"));
            backToMerchant.Click();
            bool c = WaitjQuery();

            WaitForPageLoading();
        }

        /// <summary>
        /// Pays with trustpay (can success or fail the payment)
        /// </summary>
        /// <param name="isSuccessful"></param>
        public void PayWithTrustPay(bool isSuccessful)
        {
            //TODO
            Thread.Sleep(TimeSpan.FromSeconds(1));
            WaitForElement(_trustPay, 15);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _trustPay.Click();
            WaitForPageLoading();
            bool w = WaitjQuery();

            ScrollToView(WaitForElement(By.CssSelector("#ctl00_cphContent_pStep4Header > div > div > div.jquery-selectbox-moreButton"), 10)).Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IWebElement countrySelect = Driver.FindElement(By.CssSelector("span.jquery-selectbox-item.value-SK.item-12"));
            ScrollToView(WaitForElement(countrySelect, 10)).Click();
           // ChooseElementInSelect("SK", countrySelect, SelectBy.Value);
            Thread.Sleep(TimeSpan.FromSeconds(2));

            IWebElement trustPayAuthorization = Driver.FindElement(By.CssSelector("div.step2Banks > a > img[id^='ct']"));
             ScrollToView(WaitForElement(trustPayAuthorization, 10)).Click();
            Thread.Sleep(TimeSpan.FromSeconds(2));

            IWebElement payNow = Driver.FindElement(By.CssSelector("#ctl00_cphContent_iPayOnline"));
            WaitForElement(ScrollToView(payNow), 10).Click();
           // bool v = WaitjQuery();
            WaitForPageLoading();

            if (isSuccessful)
            {
                IWebElement ok = Driver.FindElement(By.CssSelector("#btnOK"));
                ok.Click();
            }
            else
            {
                IWebElement fail = Driver.FindElement(By.CssSelector("#btnFAIL"));
                fail.Click();
            }

            bool a = WaitjQuery();
            WaitForPageLoading();

            IWebElement returnToMerchant = Driver.FindElement(By.CssSelector("span[id$='ContinueMiddle']"));
            returnToMerchant.Click();

            WaitForPageLoading();

        }

        /// <summary>
        /// Provides payment using Neteller
        /// </summary>
        public void PayWithNeteller()
        {
           // ScrollDown();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            WaitForElement(_neteller, 15);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _neteller.Click();
            WaitForPageLoading();
            bool w = WaitjQuery();
            
            //TODO: change to something
            Thread.Sleep(TimeSpan.FromSeconds(3));
            bool a = WaitAjax();

            IWebElement login = WaitForElement(By.CssSelector("#login_identity"), 10);
            login.SendKeys("451823760529");

            //TODO: change to something
            Thread.Sleep(TimeSpan.FromSeconds(1));
            IWebElement password = WaitForElement(By.CssSelector("#password"), 10);
            password.SendKeys("NTt3st1!");

            //TODO: change to something
            Thread.Sleep(TimeSpan.FromSeconds(1));
            IWebElement continueBtn = WaitForElement(By.CssSelector("#checkout_continue_btn"), 10);
            continueBtn.Click();
            WaitForPageLoading();

            IWebElement completeOrder = Driver.FindElement(By.CssSelector("#checkout_button"));
            completeOrder.Click();
            WaitForPageLoading();

            IWebElement returnToLottosend = Driver.FindElement(By.CssSelector("div.panel-footer > a.button"));
            returnToLottosend.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// Provides payment with offline charge
        /// </summary>
        public void PayWithOfflineCharge(bool newCombinedPage = false)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (newCombinedPage)
            {
                MerchantsCombinedSectionObj newSection = new MerchantsCombinedSectionObj(Driver);
                newSection.ClickOnMerchant(WayToPay.Offline);
            }
            else
            {
                WaitForElement(_offline, 10);
                Thread.Sleep(TimeSpan.FromSeconds(1));
                _offline.Click();
            }

            WaitjQuery();
            _fillInFields();
        }

        /// <summary>
        /// Fills in all necesarry fields 
        /// </summary>
        private void _fillInFields()
        {
            OfflineChargeFormObj form = new OfflineChargeFormObj(Driver);
            form.FilInForm();
            form.ClickNextButton();
        }

        public void ClickOnEKontoEPlatby()
        {
            //TODO
            
            _eKontoePlatby.Click();
           
        }

        public void PayWithInvoice()
        {
            IWebElement SecurityNumber = Driver.FindElement(By.CssSelector("#payment_transaction_nsn"));
            SecurityNumber.SendKeys(RandomGenerator.GenerateNumber(10, 1000000000).ToString());
        }


        /// <summary>
        /// Approves of fails an offline/invoice order
        /// </summary>
        /// <param name="isFailed"></param>
        private void ProcessOrder(bool isFailed)
        {
            CommonActions commonActions = new CommonActions(Driver);

            commonActions.SignIn_in_admin_panel();
             commonActions.Authorize_the_first_payment();
            if (!isFailed)
            {
                commonActions.Approve_offline_payment();
            }
            else
            {
                commonActions.Fail_offline_payment();
            }
        }

        /// <summary>
        /// Pays for whatever using selected merchant (also able to fail payment)
        /// </summary>
        /// <param name="merchant">How to pay</param>
        /// <param name="ifProcess">To process payment (if it was offline)</param>
        /// <param name="isFailed">To fail or not</param>
        public void Pay(WayToPay merchant, bool ifProcess = true, bool isFailed = false)
        {
            if (merchant == WayToPay.Invoice)
            {
                PayWithInvoice();

                if (ifProcess)
                {
                    ProcessOrder(isFailed);
                }
            }

            if (merchant == WayToPay.Skrill)
            {
                PayWithSkrill();
            }

            if (merchant == WayToPay.TrustPay)
            {
                PayWithTrustPay(!isFailed);
            }

            if (merchant == WayToPay.Neteller)
            {
                PayWithNeteller();
            }

            if (merchant == WayToPay.Offline)
            {
                PayWithOfflineCharge();

                if (ifProcess)
                {
                    ProcessOrder(isFailed);
                }
            }
        }
    }
}
