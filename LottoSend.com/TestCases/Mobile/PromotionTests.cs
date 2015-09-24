using System.Collections.Generic;
using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Mobile
{
    /// <summary>
    /// Includes tests connected to promotions on mobile
    /// </summary>
    [TestFixture("Apple iPhone 4")]
    [TestFixture("Apple iPhone 6")]
    [TestFixture("Apple iPhone 5")]
    [TestFixture("Samsung Galaxy S4")]
    [TestFixture("Samsung Galaxy Note II")]
    public class PromotionTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BalanceVerifications _verifications;
        private double _totalPrice;
        private string _device;

        public PromotionTests(string device)
        {
            _device = device;
        }
        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second payment if the first one was pendant
        /// </summary>
        [TestCase(WayToPay.Offline)]
        [TestCase(WayToPay.Neteller)]
        public void One_Plus_One_After_Pending_Deposit(WayToPay merchant)
        {
            //Sign up
            _commonActions.Sign_Up();
            _commonActions.DepositMoney(13, WayToPay.Offline, false);
            
            _commonActions.DepositMoney(11, merchant);

            _verifications.CheckBalanceOnDepositPage(22);
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second payment if the first one was failed
        /// </summary>
        [TestCase(WayToPay.Offline)]
        [TestCase(WayToPay.Neteller)]
        public void One_Plus_One_After_Failed_Deposit(WayToPay merchant)
        {
            //Sign up
            _commonActions.Sign_Up();
            _commonActions.DepositMoney(13, WayToPay.Offline, true, true);

            _commonActions.DepositMoney(11, merchant);

            _verifications.CheckBalanceOnDepositPage(22);
        }

        /// <summary>
        /// Cheks if a new user doesn't get 1+1 promotion for the second payment
        /// </summary>
        [TestCase(WayToPay.Offline)]
        [TestCase(WayToPay.Neteller)]
        public void One_Plus_One_Second_Payment(WayToPay merchant)
        {
            _commonActions.Sign_Up();
            _commonActions.BuyRegularOneDrawTicket(merchant); //will get 1+1 promotion
            _commonActions.BuyRaffleTicket(merchant); //this ticket must cost more then the previously bought one
            
            _verifications.CheckBalanceOnDepositPage(0);
        }

        /// <summary>
        /// Checks if a user doesn't get 1+1 promotion for the second deposit
        /// </summary>
        [TestCase(WayToPay.Offline)]
        [TestCase(WayToPay.Neteller)]
        public void One_Plus_One_Second_Deposit(WayToPay merchant)
        {
            //Sign up
            _commonActions.Sign_Up();
            _commonActions.DepositMoney(13, merchant); //is expected to get 1+1 promotion
            _commonActions.DepositMoney(15, merchant);

            _verifications.CheckBalanceOnDepositPage(41); //13*2+15 
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion after buying a ticket
        /// </summary>
        [TestCase(WayToPay.Offline)]
        [TestCase(WayToPay.Neteller)]
        public void One_Plus_One_Promotion_Buying(WayToPay merchant)
        {
            //Sign up
           _commonActions.Sign_Up();
            _totalPrice = _commonActions.BuyRaffleTicket(merchant);

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
        [TestCase(WayToPay.Offline)]
        [TestCase(WayToPay.Neteller)]
        public void One_Plus_One_Promotion_Deposit(WayToPay merchant)
        {
            //Sign up
            _commonActions.Sign_Up();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/deposits/new/");
            
            DepositObj deposit = new DepositObj(_driver);
            deposit.DepositOtherAmount(17, merchant);

            _verifications.CheckBalanceOnDepositPage(34);
        }

        private ChromeOptions CreateOptions(string device)
        {
            var mobileEmulation = new Dictionary<string, string>
            {
                {"deviceName", device}
            };

            ChromeOptions options = new ChromeOptions();
            options.AddAdditionalCapability("mobileEmulation", mobileEmulation);
            return options;
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver(CreateOptions(_device));
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _verifications = new BalanceVerifications(_driver);
        }
    }
}
