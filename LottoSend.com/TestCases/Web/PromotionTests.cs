using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace LottoSend.com.TestCases.Web
{
    /// <summary>
    /// Includes tests connected to promotions
    /// </summary>
    [TestFixture(typeof(ChromeDriver), WayToPay.Neteller)]
    [TestFixture(typeof(FirefoxDriver), WayToPay.Neteller)]
    [TestFixture(typeof(InternetExplorerDriver), (WayToPay.Neteller))]
    [TestFixture(typeof(ChromeDriver), WayToPay.Offline)]
    [TestFixture(typeof(FirefoxDriver), WayToPay.Offline)]
    [TestFixture(typeof(InternetExplorerDriver), WayToPay.Offline)]
    [TestFixture(typeof(ChromeDriver), WayToPay.TrustPay)]
    [TestFixture(typeof(FirefoxDriver), WayToPay.TrustPay)]
    [TestFixture(typeof(InternetExplorerDriver), WayToPay.TrustPay)]
    [TestFixture(typeof(ChromeDriver), WayToPay.Skrill)]
    [TestFixture(typeof(FirefoxDriver), WayToPay.Skrill)]
    [TestFixture(typeof(InternetExplorerDriver), WayToPay.Skrill)]
    public class PromotionTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BalanceVerifications _verifications;
        private double _totalPrice;
        private WayToPay _merchant;

        public PromotionTests(WayToPay merchant)
        {
            _merchant = merchant;
        }

            /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second payment if the first one was pendant
        /// </summary>
        public void One_Plus_One_After_Pending_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up();
            _commonActions.DepositMoney(13, WayToPay.Offline, false);

            _commonActions.DepositMoney(11, _merchant);

            _verifications.CheckBalanceOnDepositPage(22);
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second payment if the first one was failed
        /// </summary>
        public void One_Plus_One_After_Failed_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up();
            _commonActions.DepositMoney(13, WayToPay.Offline, true, true);

            _commonActions.DepositMoney(11, _merchant);

            _verifications.CheckBalanceOnDepositPage(22);
        }

        /// <summary>
        /// Cheks if a new user doesn't get 1+1 promotion for the second payment
        /// </summary>
        public void One_Plus_One_Second_Payment()
        {
            _commonActions.Sign_Up();
            _commonActions.BuyRegularOneDrawTicket(_merchant); //will get 1+1 promotion
            _commonActions.BuyRaffleTicket(_merchant); //this ticket must cost more then the previously bought one
            
            _verifications.CheckBalanceOnDepositPage(0);
        }

        /// <summary>
        /// Checks if a user doesn't get 1+1 promotion for the second deposit
        /// </summary>
        public void One_Plus_One_Second_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up();
            _commonActions.DepositMoney(13, _merchant); //is expected to get 1+1 promotion
            _commonActions.DepositMoney(15, _merchant);

            _verifications.CheckBalanceOnDepositPage(41); //13*2+15 
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion after buying a ticket
        /// </summary>
        public void One_Plus_One_Promotion_Buying()
        {
            //Sign up
           _commonActions.Sign_Up();
           _totalPrice = _commonActions.BuyRaffleTicket(_merchant);

            if (_totalPrice <= 30)
            {
                _verifications.CheckBalanceOnDepositPage(_totalPrice);
            }
            else
            {
                _verifications.CheckBalanceOnDepositPage(30);
            }
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion depositing money
        /// </summary>
        public void One_Plus_One_Promotion_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "account/deposits/new/");
            
            DepositObj deposit = new DepositObj(_driver);
            deposit.DepositOtherAmount(17, _merchant);

            _verifications.CheckBalanceOnDepositPage(34);
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _verifications = new BalanceVerifications(_driver);
        }
    }
}
