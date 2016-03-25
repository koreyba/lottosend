using System;
using LottoSend.com.BackEndObj;
using LottoSend.com.Helpers;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice
{
    [TestFixture(typeof(ChromeDriver))]
    public class CombinedPaymentsTests <TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BalanceVerifications _balanceVerifications;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;
        private string _lotteryName;
        private string _email;

        public CombinedPaymentsTests()
        {
            SetUp();
            _sharedCode = new TestsSharedCode(_driver);

            try
            {
                Make_Combined_Payment();
            }
            catch (Exception)
            {
                CleanUp();

                try
                {
                    SetUp();
                    Make_Combined_Payment();
                }
                catch (Exception e)
                {
                    _setUpFailed = true;

                    CleanUp();
                    
                    throw new Exception("Exception was thrown while executing: " + e.Message + " ");
                }
            }

            CleanUp();
        }

        /// <summary>
        /// Checks if the first transaction in the back office is "Combined Payment" in details. Buys tickets in the sales panel
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Check_Combined_Payment_Transaction()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/transactions");

            TransactionsObj transactions = new TransactionsObj(_driver);
            
            StringAssert.Contains("Combined Payment", transactions.FirstTransactionDetails, "Sorry but the first record is not combined payment. Please check. Current URL is: " + _driverCover.Driver.Url  + " ");
        }

        /// <summary>
        /// Checks is user's real money balance = 0 and store credit = 30 (new signed up user gets promotion)
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Check_User_Balance()
        {
            //Checks if a user made combined payment and has 0 on real money balance but has 30 on store credit as a promotion

            _balanceVerifications.CheckUserSRealMoney_BackOffice(_email, 0);
            _balanceVerifications.CheckUserStoreCredit_BackOffice(_email, 30);
        }

        private void Make_Combined_Payment()
        {
            _email = _commonActions.Sign_Up_Front();
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/web_users");

            WebUsersPageObj users = new WebUsersPageObj(_driver);

            users.FilterByEmail(_email);
            users.CreditToRealMoney("3");
            users.CreditToStoreCredit("5");

            _commonActions.Log_In_SalesPanel(_email);

            _commonActions.AddRegularOneDrawTicketToCart_SalesPanel("Powerball");
            _commonActions.AddGroupBulkBuyTicketToCart_SalesPanel("EuroMillions");
            _commonActions.AddRaffleTicketToCart_SalesPanel("Loteria de Navidad");

            _commonActions.PayForTicketsInCart_SalesPanel(WayToPay.Offline);
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
                _commonActions.DeleteAllTicketFromCart_SalesPanel();
                MessageConsoleCreator message = new MessageConsoleCreator();
                message.DriverDisposed();
                _driver.Dispose();
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
            _balanceVerifications = new BalanceVerifications(_driver);
        }
    }
}
