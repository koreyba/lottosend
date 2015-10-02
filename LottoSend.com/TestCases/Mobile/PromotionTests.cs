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
    [TestFixture("Apple iPhone 4", WayToPay.Neteller)]
    [TestFixture("Apple iPhone 6", WayToPay.Offline)]
    [TestFixture("Apple iPhone 5", WayToPay.TrustPay)]
    [TestFixture("Samsung Galaxy S4", WayToPay.Skrill)]
    public class PromotionTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BalanceVerifications _verifications;
        private double _totalPrice;
        private string _device;
        private WayToPay _merchant;

        public PromotionTests(string device, WayToPay merchant)
        {
            _device = device;
            _merchant = merchant;
        }
        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second payment if the first one was pendant
        /// </summary>
        public void One_Plus_One_After_Pending_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up_Mobile();
            _commonActions.DepositMoneyMobile(13, WayToPay.Offline, false);

            _commonActions.DepositMoneyMobile(11, _merchant);

            _verifications.CheckBalanceOnDepositPageMobile(22);
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second payment if the first one was failed
        /// </summary>
        public void One_Plus_One_After_Failed_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up_Mobile();
            _commonActions.DepositMoneyMobile(13, WayToPay.Offline, true, true);

            _commonActions.DepositMoneyMobile(11, _merchant);

            _verifications.CheckBalanceOnDepositPageMobile(22);
        }

        /// <summary>
        /// Cheks if a new user doesn't get 1+1 promotion for the second payment
        /// </summary>
        public void One_Plus_One_Second_Payment()
        {
            _commonActions.Sign_Up_Mobile();
            _commonActions.BuyRegularOneDrawTicket(_merchant); //will get 1+1 promotion
            _commonActions.BuyRaffleTicket(_merchant); //this ticket must cost more then the previously bought one

            _verifications.CheckBalanceOnDepositPageMobile(0);
        }

        /// <summary>
        /// Checks if a user doesn't get 1+1 promotion for the second deposit
        /// </summary>
        public void One_Plus_One_Second_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up_Mobile();
            _commonActions.DepositMoneyMobile(13, _merchant); //is expected to get 1+1 promotion
            _commonActions.DepositMoneyMobile(15, _merchant);

            _verifications.CheckBalanceOnDepositPageMobile(41); //13*2+15 
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion after buying a ticket
        /// </summary>
        public void One_Plus_One_Promotion_Buying()
        {
            //Sign up
            _commonActions.Sign_Up_Mobile();
            _totalPrice = _commonActions.BuyRaffleTicket(_merchant);

            if (_totalPrice <= 30)
            {
                _verifications.CheckBalanceOnDepositPageMobile(_totalPrice);
            }
            else
            {
                _verifications.CheckBalanceOnDepositPageMobile(30);
            }
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion depositing money
        /// </summary>
        public void One_Plus_One_Promotion_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up_Mobile();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/deposits/new/");

            DepositMobileObj deposit = new DepositMobileObj(_driver);
            deposit.DepositOtherAmount(17, _merchant);

            _verifications.CheckBalanceOnDepositPageMobile(34);
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
