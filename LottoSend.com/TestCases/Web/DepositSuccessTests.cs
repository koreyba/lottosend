using System;
using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace LottoSend.com.TestCases.Web
{
    /// <summary>
    /// Includes tests to check depositing process 
    /// </summary>
    [TestFixture(typeof(ChromeDriver), WayToPay.Neteller)]
    ////[TestFixture(typeof(FirefoxDriver), WayToPay.Neteller)]
    //[TestFixture(typeof(InternetExplorerDriver), (WayToPay.Neteller))]
    [TestFixture(typeof(ChromeDriver), WayToPay.Offline)]
    ////[TestFixture(typeof(FirefoxDriver), WayToPay.Offline)]
    //[TestFixture(typeof(InternetExplorerDriver), WayToPay.Offline)]
    //[TestFixture(typeof(ChromeDriver), WayToPay.TrustPay)]
    ////[TestFixture(typeof(FirefoxDriver), WayToPay.TrustPay)]
    //[TestFixture(typeof(InternetExplorerDriver), WayToPay.TrustPay)]
    //[TestFixture(typeof(ChromeDriver), WayToPay.Skrill)]
    ////[TestFixture(typeof(FirefoxDriver), WayToPay.Skrill)]
    //[TestFixture(typeof(InternetExplorerDriver), WayToPay.Skrill)]

    public class DepositSuccessTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private OrderVerifications _orderVerifications;
        private CommonActions _commonActions;
        private string _email;
        private WayToPay _merchant;
        private double _balanceBeforePayment;
        private bool _setUpFailed = false;
        private double depositAmount;

        public DepositSuccessTests(WayToPay merchant)
        {
            _merchant = merchant;
            depositAmount = 30;

            SetUp();
            try
            {
                Deposit_Money(merchant);
            }
            catch (Exception e)
            {
                _setUpFailed = true;
                CleanUp();
                throw new Exception("Exception was thrown while executing: " + e.Message + " ");
            }
            CleanUp();
        }

        /// <summary>
        /// Deposits money to the user's balance
        /// </summary>
        /// <param name="merchant"></param>
        private void Deposit_Money(WayToPay merchant)
        {
            _email = _commonActions.Log_In_Front(_driverCover.LoginTwo, _driverCover.Password);
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/deposits/new/");

            DepositObj deposit = new DepositObj(_driver);
            _balanceBeforePayment = deposit.Balance;
            deposit.DepositStandardAmount(depositAmount, merchant);
        }

        /// <summary>
        /// Checks user's current balance
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Check_User_Balance()
        {
            _orderVerifications.CheckUserBalance_Front(_balanceBeforePayment, depositAmount, _driverCover.LoginTwo, _driverCover.Password);
        }

        /// <summary>Te
        /// Checks an amount in the first record in transactions (front)
        /// </summary>
        [Test]
        [Category("Critical")]
        public void Check_Amount_In_Transaction_Front()
        {
            _orderVerifications.CheckAmountInTransaction_Front(30, _email, _driverCover.Password, 1);
        }

        /// <summary>
        /// Checks a type of the first record in transactions (front)
        /// </summary>
        [Test]
       // [Category("Critical")]
        public void Check_Type_Of_Transaction_Front()
        {
            _orderVerifications.CheckTypeOfTransaction_Front("Deposit", _email, _driverCover.Password);
        }

        /// <summary>
        /// Checks date of the first and seconds records in users account - transactions (front-end)
        /// </summary>
        [Test]
       // [Category("Critical")]
        public void Check_Transaction_Date_Front()
        {
            _orderVerifications.CheckTransactionDate_Front(_email, _driverCover.Password);
        }

        /// <summary>
        /// Checks if the transaction has correct status on "Transactions" page
        /// </summary>
        [Test]
       // [Category("Critical")]
        public void Check_Transactions_State_In_Transactions()
        {
            _orderVerifications.CheckTransactionsStateInTransactions_Back("succeed");
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_Email_In_Transactions()
        {
            _orderVerifications.CheckTransactionsEmailInTransactions_Back(_email);
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
      //  [Category("Critical")]
        public void Check_Transaction_Merchant_In_Transactions()
        {
            _orderVerifications.CheckTransactionMerchantInTransactions_Back(_merchant);
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
      //  [Category("Critical")]
        public void a_Check_Transaction_Time_In_Transactions()
        {
            _orderVerifications.CheckTransactionTimeInTransactions_Back();
        }

        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
            {
                _driverCover.TakeScreenshot();
            }
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _orderVerifications = new OrderVerifications(_driver);
            _commonActions = new CommonActions(_driver);
        }
    }
}
