using System;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.WebUsersPages;
using TestFramework.FrontEndObj;
using TestFramework.FrontEndObj.Common;
using TestFramework.FrontEndObj.GamePages;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.Web.Payments
{
    /// <summary>
    /// Tests that are connected with failed payments
    /// </summary>
    /// <typeparam name="TWebDriver"></typeparam>
    [TestFixture(typeof (ChromeDriver))]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    class FailedPaymentTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private CartVerifications _cartVerifications;
        private BalanceVerifications _balanceVerifications;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Credit real money and store credit for a new user and makes a payment. Then failed the payment and checks if the balance was returned back.
        /// </summary>
        [Test]
        public void Balance_Returned_To_User_After_Failed_Payment()
        {
            string email = _commonActions.Sign_Up_Front_PageOne();
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/web_users");
            WebUsersPageObj users = new WebUsersPageObj(_driver);

            users.FilterByEmail(email);
            users.CreditToRealMoney("3");
            users.CreditToStoreCredit("4");

            _commonActions.AddRegularTicketToCart_Front("en/play/superenalotto/");
            MerchantsObj merchants = new MerchantsObj(_driver);

            merchants.PayWithOfflineCharge();

            _commonActions.SignIn_in_admin_panel();
            _commonActions.Authorize_the_first_payment();
            _commonActions.Fail_offline_payment();

            _balanceVerifications.CheckUserSRealMoney_BackOffice(email, 3);
            _balanceVerifications.CheckUserStoreCredit_BackOffice(email, 4);
        }

        /// <summary>
        /// Fails payment and check if URL displayes the word "failure"
        /// </summary>
        /// <param name="merchant"></param>
        [TestCase(WayToPay.eKonto)]
        [TestCase(WayToPay.Moneta)]
        [TestCase(WayToPay.Poli)]
        [TestCase(WayToPay.Offline)]
        [Category("Critical")]
        public void Fail_Payment_Check_URL(WayToPay merchant)
        {
            /* 
             * Works with payments that can be failed (eKonto, Moneta, Poli, Offline)
             * The test just fails payment and checks URL. 
             * If it includes the word "failure" then test is passed.
             */

            _commonActions.Log_In_Front_PageOne(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);

            rafflePage.ClickBuyNowButton();

            MerchantsObj merchants = new MerchantsObj(_driver);

            switch (merchant)
            {
                case WayToPay.eKonto:
                {
                    merchants.eKontoePlatby.Click();
                }
                    break;

                case WayToPay.Poli:
                {
                    merchants.Poli.Click();
                }
                    break;

                case WayToPay.Moneta:
                    {
                        merchants.Moneta.Click();
                    }
                    break;

                case WayToPay.Offline:
                    {
                        merchants.PayWithOfflineCharge();
                        _driverCover.OpenNewTab();
                        _commonActions.SignIn_in_admin_panel();
                        _commonActions.Authorize_the_first_payment();
                        _commonActions.Fail_offline_payment();
                        _driverCover.SwitchToTab(1);
                        _driverCover.RefreshPage();
                    }
                    break;
            }

            if (merchant == WayToPay.Offline)
            {   
                StringAssert.Contains("failure", _driverCover.Driver.Url);
            }
            else
            {
                OnlineMerchantsObj online = new OnlineMerchantsObj(_driver);
                online.FailPayment();
                StringAssert.Contains("failed", _driverCover.Driver.Url);
            }
            

            _commonActions.DeleteAllTicketFromCart_Front();
        }

        /// <summary>
        /// Checks if a raffle ticket stays in the cart after payment was failed
        /// </summary>
        [Test]
        [Category("Critical")]
        public void If_Raffle_Ticket_Stays_In_Cart_When_Payment_Was_Failed()
        {
            // Log in     
            _commonActions.Log_In_Front_PageOne(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);

            rafflePage.ClickBuyNowButton();//
            MerchantsObj merchants = new MerchantsObj(_driver);

            merchants.PayWithOfflineCharge();

            //Go to admin panel
            _commonActions.SignIn_in_admin_panel();

            //authorize payment in charge panel
            _commonActions.Authorize_the_first_payment();

            //approve payment
            _commonActions.Fail_offline_payment();

            _cartVerifications.CheckIfTicketIsInCart("Cart Raffle");

            _commonActions.DeleteAllTicketFromCart_Front();
        }

        /// <summary>
        /// Checks if a regular ticket stays in the cart after payment was failed
        /// </summary>
        [Test]
        public void If_Regular_Ticket_Stays_In_Cart_When_Payment_Was_Failed()
        {
            // Log in     
            _commonActions.Log_In_Front_PageOne(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/eurojackpot/");

            //Pay for tickets
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            //Go to single tab
            regularGame.ClickStandartGameButton();

            MerchantsObj merchants = regularGame.ClickBuyTicketsButton();
            merchants.PayWithOfflineCharge();

            //Go to admin panel
            _commonActions.SignIn_in_admin_panel();

            //authorize payment in charge panel
            _commonActions.Authorize_the_first_payment();

            //approve payment
            _commonActions.Fail_offline_payment();

            _cartVerifications.CheckIfTicketIsInCart("EuroJackpot");

            _commonActions.DeleteAllTicketFromCart_Front();
        }

        /// <summary>
        /// Checks if a grup ticket stays in the cart after payment was failed
        /// </summary>
        [Test]
        public void If_Group_Ticket_Stays_In_Cart_When_Payment_Was_Failed()
        {
            // Log in     
            _commonActions.Log_In_Front_PageOne(_driverCover.Login, _driverCover.Password);

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/powerball/");

            //Pay for tickets
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);

            MerchantsObj merchants = groupGame.ClickBuyTicketsButton();
            merchants.PayWithOfflineCharge();

            //Go to admin panel
            _commonActions.SignIn_in_admin_panel();

            //authorize payment in charge panel
            _commonActions.Authorize_the_first_payment();

            //approve payment
            _commonActions.Fail_offline_payment();

            _cartVerifications.CheckIfTicketIsInCart("Powerball");

            _commonActions.DeleteAllTicketFromCart_Front();
        }

        [TearDown]
        public void CleanUp()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
                {
                    _driverCover.TakeScreenshot();
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                //Removes all tickets from the cart to make sure all other tests will work well
                try
                {
                    _commonActions.DeleteAllTicketFromCart_Front();
                }
                catch (Exception)
                {
                    _sharedCode.CleanUp(ref _driver);
                }

                _sharedCode.CleanUp(ref _driver);
            }
        }

        [SetUp]
        public void SetUp()
        {
            MessageConsoleCreator message = new MessageConsoleCreator();
            message.TestWillRun();
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _cartVerifications = new CartVerifications(_driver);
            _balanceVerifications = new BalanceVerifications(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
