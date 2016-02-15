using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Web
{
    /// <summary>
    /// Includes tests connected to promotions
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
    public class PromotionTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BalanceVerifications _verifications;
        private double _totalPrice;
        private WayToPay _merchant;
        private bool _setUpFailed = false;

        public PromotionTests(WayToPay merchant)
        {
            _merchant = merchant;
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second deposit when he had the first one pending and it was failed 
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_For_Second_Deposit_When_First_Failed()
        {
            //Sign up
            _commonActions.Sign_Up_Front();
            _commonActions.DepositMoney_Front(13, WayToPay.Offline, false); //will be pending deposit

            _commonActions.DepositMoney_Front(11, _merchant); //will be successful deposit

            _commonActions.SignIn_in_admin_panel();
            _commonActions.Authorize_the_first_payment();
            _commonActions.Fail_offline_payment(); //fail the first payment

            _verifications.CheckBalanceOnDepositPage_Web(22); //Check if for the second payment a user got 1+1 
        }

        /// <summary>
        /// Cheks if a new user doesn't get 1+1 promotion for the second deposit if the first one is still pending
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_For_Second_Deposit_When_First_Pending()
        {
            //Sign up
            _commonActions.Sign_Up_Front();
            _commonActions.DepositMoney_Front(13, WayToPay.Offline, false); //will be pending deposit

            _commonActions.DepositMoney_Front(11, _merchant); //will be successful deposit

            _verifications.CheckBalanceOnDepositPage_Web(11); //Check if there is no 1+1 promotion for the second payment if the first one is pending
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second order when he had the first one pending and it was failed 
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_For_Second_Order_When_First_Failed()
        {
            //Sign up
            _commonActions.Sign_Up_Front();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);
           // double totalPrice = rafflePage.TotalPrice;

            rafflePage.ClickBuyNowButton();//
            MerchantsObj merchants = new MerchantsObj(_driver);
            merchants.Pay(WayToPay.Offline, false); //will be pending order 

            double price = _commonActions.BuyRegularOneDrawTicket_Front(_merchant); //will be successful order

            _commonActions.SignIn_in_admin_panel();
            _commonActions.Authorize_the_first_payment();
            _commonActions.Fail_offline_payment(); //fail the first payment

            if (price >= 30)
            {
                _verifications.CheckBalanceOnDepositPage_Web(30);  //Check if for the second payment a user got 1+1 
            }
            else
            {
                _verifications.CheckBalanceOnDepositPage_Web(price);  //Check if for the second payment a user got 1+1 
            }
        }

        /// <summary>
        /// Cheks if a new user doesn't get  1+1 promotion for the second order payment if the first one is pending
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_For_Second_Order_When_First_Pending()
        {
            //Sign up
            _commonActions.Sign_Up_Front();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);
            // double totalPrice = rafflePage.TotalPrice;

            rafflePage.ClickBuyNowButton();//
            MerchantsObj merchants = new MerchantsObj(_driver);
            merchants.Pay(WayToPay.Offline, false); //will be pending order 

            _commonActions.BuyRegularOneDrawTicket_Front(_merchant); //will be successful order

            _verifications.CheckBalanceOnDepositPage_Web(0); //Check if there is no 1+1 promotion for the second payment if the first one is pending
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second deposit payment if the first one was failed
        /// </summary>
        [Test]
       // [Category("Critical")]
        public void One_Plus_One_After_Failed_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up_Front();
            _commonActions.DepositMoney_Front(13, WayToPay.Offline, true, true);

            _commonActions.DepositMoney_Front(11, _merchant);

            _verifications.CheckBalanceOnDepositPage_Web(22);
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second order payment if the first one was failed
        /// </summary>
        [Test]
      //  [Category("Critical")]
        public void One_Plus_One_After_Failed_Order()
        {
            //Sign up
            _commonActions.Sign_Up_Front();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);
            //double totalPrice = rafflePage.TotalPrice;

            rafflePage.ClickBuyNowButton();//
            MerchantsObj merchants = new MerchantsObj(_driver);
            merchants.Pay(WayToPay.Offline, true, true);

            double price = _commonActions.BuyRegularOneDrawTicket_Front(_merchant);

            if (price >= 30)
            {
                _verifications.CheckBalanceOnDepositPage_Web(30);
            }
            else
            {
                _verifications.CheckBalanceOnDepositPage_Web(price);
            }
        }

        /// <summary>
        /// Cheks if a new user doesn't get 1+1 promotion for the second payment
        /// </summary>
        [Test]
       // [Category("Critical")]
        public void One_Plus_One_Second_Payment()
        {
            _commonActions.Sign_Up_Front();
            _commonActions.BuyRegularOneDrawTicket_Front(_merchant); //will get 1+1 promotion
            _commonActions.BuyRaffleTicket_Front(_merchant); //this ticket must cost more then the previously bought one
            
            _verifications.CheckBalanceOnDepositPage_Web(0);
        }

        /// <summary>
        /// Checks if a user doesn't get 1+1 promotion for the second deposit
        /// </summary>
        [Test]
        public void One_Plus_One_Second_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up_Front();
            _commonActions.DepositMoney_Front(13, _merchant); //is expected to get 1+1 promotion
            _commonActions.DepositMoney_Front(15, _merchant);

            _verifications.CheckBalanceOnDepositPage_Web(41); //13*2+15 
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion after buying a ticket
        /// </summary>
        [Test]
      //  [Category("Critical")]
        public void One_Plus_One_Promotion_Buying()
        {
            //Sign up
           _commonActions.Sign_Up_Front();
           _totalPrice = _commonActions.BuyRaffleTicket_Front(_merchant);

            if (_totalPrice <= 30)
            {
                _verifications.CheckBalanceOnDepositPage_Web(_totalPrice);
            }
            else
            {
                _verifications.CheckBalanceOnDepositPage_Web(30);
            }
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion depositing money
        /// </summary>
        [Test]
     //   [Category("Critical")]
        public void One_Plus_One_Promotion_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up_Front();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/deposits/new/");
            
            DepositObj deposit = new DepositObj(_driver);
            deposit.DepositOtherAmount(17, _merchant);

            _verifications.CheckBalanceOnDepositPage_Web(34);
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
            _commonActions = new CommonActions(_driver);
            _verifications = new BalanceVerifications(_driver);
        }
    }
}
