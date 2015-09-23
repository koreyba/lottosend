using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases
{
    /// <summary>
    /// Includes tests to check depositing process 
    /// </summary>
    [TestFixture(WaysToPay.Neteller)]
    [TestFixture(WaysToPay.Offline)]
    public class DepositTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private OrderVerifications _verifications;
        private CommonActions _commonActions;
        private string _email;
        private WaysToPay _merchant;

        public DepositTests(WaysToPay merchant)
        {
            _merchant = merchant;
            SetUp();

            _email = _commonActions.Log_In_Front("selenium2@gmail.com", _driverCover.Password);
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/deposits/new/");

            DepositObj deposit = new DepositObj(_driver);
            deposit.DepositStandardAmount(30, merchant);

            CleanUp();
        }

        /// <summary>Te
        /// Checks an amount in the first record in transactions (front)
        /// </summary>
        [Test]
        public void Check_Amount_In_Transaction_Front()
        {
            _verifications.CheckAmountInTransactionFront(30, _email, _driverCover.Password, 1);
        }

        /// <summary>
        /// Checks a type of the first record in transactions (front)
        /// </summary>
        [Test]
        public void Check_Type_Of_Transaction_Front()
        {
            _verifications.CheckTypeOfTransactionFront("Deposit", _email, _driverCover.Password);
        }

        /// <summary>
        /// Checks date of the first and seconds records in users account - transactions (front-end)
        /// </summary>
        [Test]
        public void Check_Transaction_Date_Front()
        {
            _verifications.CheckTransactionDateFront(_email, _driverCover.Password);
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_Email_In_Transactions()
        {
            _verifications.CheckTransactionsEmailInTransactions(_email);
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Merchant_In_Transactions()
        {
            _verifications.CheckTransactionMerchantInTransactions(_merchant);
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Time_In_Transactions()
        {
            _verifications.CheckTransactionTimeInTransactions();
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driverCover = new DriverCover(_driver);
            _verifications = new OrderVerifications(_driver);
            _commonActions = new CommonActions(_driver);
        }
    }
}
