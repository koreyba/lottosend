using System;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.Helpers;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Mobile
{
    /// <summary>
    /// Includes tests connected to promotions on mobile
    /// </summary>
    //[TestFixture("Apple iPhone 4", WayToPay.Neteller)]
    [TestFixture("Apple iPhone 6", WayToPay.Offline)]
    //[TestFixture("Apple iPhone 5", WayToPay.TrustPay)]
   //[TestFixture("Samsung Galaxy S4", WayToPay.Skrill)]
    public class PromotionTests
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BalanceVerifications _verifications;
        private double _totalPrice;
        private string _device;
        private WayToPay _merchant;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;

        public PromotionTests(string device, WayToPay merchant)
        {
            _device = device;
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
            _commonActions.Sign_Up_Mobile();
            _commonActions.DepositMoney_Mobile(13, WayToPay.Offline, false); //will be pending deposit

            _commonActions.DepositMoney_Mobile(11, _merchant); //will be successful deposit

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
            _commonActions.Sign_Up_Mobile();
            _commonActions.DepositMoney_Mobile(13, WayToPay.Offline, false); //will be pending deposit

            _commonActions.DepositMoney_Mobile(11, _merchant); //will be successful deposit

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
            _commonActions.Sign_Up_Mobile();
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
            _commonActions.Sign_Up_Mobile();
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
        /// Cheks if a new user gets 1+1 promotion for the second order payment if the first one was failed
        /// </summary>
        [Test]
      //  [Category("Critical")]
        public void One_Plus_One_After_Failed_Order()
        {
            //Sign up
            _commonActions.Sign_Up_Mobile();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);
            double totalPrice = rafflePage.TotalPrice;

            rafflePage.ClickBuyNowButton();//
            MerchantsObj merchants = new MerchantsObj(_driver);
            merchants.Pay(WayToPay.Offline, true, true);

            double price = _commonActions.BuyRegularOneDrawTicket_Front(_merchant);

            if (price + totalPrice >= 30)
            {
                _verifications.CheckBalanceOnDepositPage_Web(30);
            }
            else
            {
                _verifications.CheckBalanceOnDepositPage_Web(price + totalPrice);
            }
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second payment if the first one was failed
        /// </summary>
        [Test]
      //  [Category("Critical")]
        public void One_Plus_One_After_Failed_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up_Mobile();
            _commonActions.DepositMoney_Mobile(13, WayToPay.Offline, true, true);

            _commonActions.DepositMoney_Mobile(11, _merchant);

            _verifications.CheckBalanceOnDepositPage_Mobile(22);
        }

        /// <summary>
        /// Cheks if a new user doesn't get 1+1 promotion for the second payment
        /// </summary>
        [Test]
        public void One_Plus_One_Second_Payment()
        {
            _commonActions.Sign_Up_Mobile();
            _commonActions.BuyRegularOneDrawTicket_Front(_merchant); //will get 1+1 promotion
            _commonActions.BuyRaffleTicket_Front(_merchant); //this ticket must cost more then the previously bought one

            _verifications.CheckBalanceOnDepositPage_Mobile(0);
        }

        /// <summary>
        /// Checks if a user doesn't get 1+1 promotion for the second deposit
        /// </summary>
        [Test]
        public void One_Plus_One_Second_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up_Mobile();
            _commonActions.DepositMoney_Mobile(13, _merchant); //is expected to get 1+1 promotion
            _commonActions.DepositMoney_Mobile(15, _merchant);

            _verifications.CheckBalanceOnDepositPage_Mobile(41); //13*2+15 
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion after buying a ticket
        /// </summary>
        [Test]
        public void One_Plus_One_Promotion_Buying()
        {
            //Sign up
            _commonActions.Sign_Up_Mobile();
            _totalPrice = _commonActions.BuyRaffleTicket_Front(_merchant);

            if (_totalPrice <= 30)
            {
                _verifications.CheckBalanceOnDepositPage_Mobile(_totalPrice);
            }
            else
            {
                _verifications.CheckBalanceOnDepositPage_Mobile(30);
            }
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion depositing money
        /// </summary>
        [Test]
        public void One_Plus_One_Promotion_Deposit()
        {
            //Sign up
            _commonActions.Sign_Up_Mobile();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/deposits/new/");

            DepositMobileObj deposit = new DepositMobileObj(_driver);
            deposit.DepositOtherAmount(17, _merchant);

            _verifications.CheckBalanceOnDepositPage_Mobile(34);
        }

        /// <summary>
        /// Creates and returns ChromeOptions for a mobile device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public ChromeOptions CreateOptions(string device)
        {
            ChromeOptions options = new ChromeOptions();
            options.EnableMobileEmulation(device);
            return options;
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
                _commonActions.DeleteAllTicketFromCart_Front();

                _sharedCode.CleanUp(ref _driver);
            }
        }

        [SetUp]
        public void SetUp()
        {
            MessageConsoleCreator message = new MessageConsoleCreator();
            message.TestWillRun();
            _driver = new ChromeDriver(CreateOptions(_device));
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _verifications = new BalanceVerifications(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
