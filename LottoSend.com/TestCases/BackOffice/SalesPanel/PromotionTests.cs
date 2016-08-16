using System;
using LottoSend.com.Verifications;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver))]
    public class PromotionTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private TestFramework.CommonActions _commonActions;
        private BalanceVerifications _balanceVerifications;
        private DriverCover _driverCover;
        private bool _setUpFailed = false;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second deposit when he had the first one pending and it was failed 
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_For_Second_Deposit_When_First_Failed()
        {
            _commonActions.SignIn_in_admin_panel();
            string email = _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            _commonActions.DepositMoney_SalesPanel(11, false); //will be pending deposit

            _commonActions.DepositMoney_SalesPanel(31); // will be successful deposit

            _commonActions.Fail_offline_payment(); //will faild previous deposit

            _balanceVerifications.CheckUserStoreCredit_BackOffice(email, 30); //Check if for the second payment a user got 1+1 
            _balanceVerifications.CheckUserSRealMoney_BackOffice(email, 31);
        }

        /// <summary>
        /// Cheks if a new user doesn't get 1+1 promotion for the second deposit if the first one is still pending
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_For_Second_Deposit_When_First_Pending()
        {
            _commonActions.SignIn_in_admin_panel();
            string email = _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            _commonActions.DepositMoney_SalesPanel(11, false); //will be pending deposit

            _commonActions.DepositMoney_SalesPanel(31); // will be successful deposit

            _balanceVerifications.CheckUserStoreCredit_BackOffice(email, 0); //Check if there is no 1+1 promotion for the second payment if the first one is pending
            _balanceVerifications.CheckUserSRealMoney_BackOffice(email, 31);
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second order when he had the first one pending and it was failed 
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_For_Second_Order_When_First_Failed()
        {
            _commonActions.SignIn_in_admin_panel();
            string email = _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            _commonActions.AddRegularOneDrawTicketToCart_SalesPanel("SuperEnalotto");
            _commonActions.PayForTicketsInCart_SalesPanel(WayToPay.Offline, false);

            _commonActions.Log_In_SalesPanel(email);

            //double price = _commonActions.BuyRegularOneDrawTicket_SalesPanel("EuroJackpot");

            double price = _commonActions.BuyRaffleTicket_SalesPanel("Loteria de Navidad");

            _commonActions.Fail_offline_payment(); //fail the first payment

            if (price >= 30)
            {
                _balanceVerifications.CheckUserStoreCredit_BackOffice(email, 30);
            }
            else
            {
                _balanceVerifications.CheckUserStoreCredit_BackOffice(email, price);
            }
        }

        /// <summary>
        /// Cheks if a new user doesn't get  1+1 promotion for the second order payment if the first one is pending
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_For_Second_Order_When_First_Pending()
        {
            _commonActions.SignIn_in_admin_panel();
            string email = _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            _commonActions.AddRegularOneDrawTicketToCart_SalesPanel("SuperLotto Plus");
            _commonActions.PayForTicketsInCart_SalesPanel(WayToPay.Offline, false);

            _commonActions.Log_In_SalesPanel(email);

            _commonActions.BuyRegularOneDrawTicket_SalesPanel("El Gordo");

            //double price = _commonActions.BuyRaffleTicket_SalesPanel("Loteria de Navidad");

            _balanceVerifications.CheckUserStoreCredit_BackOffice(email, 0);
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second deposit payment if the first one was failed
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_After_Failed_Deposit()
        {
            _commonActions.SignIn_in_admin_panel();
            string email = _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            _commonActions.DepositMoney_SalesPanel(11, true, true);

            _commonActions.DepositMoney_SalesPanel(31);

            _balanceVerifications.CheckUserStoreCredit_BackOffice(email, 30);
            _balanceVerifications.CheckUserSRealMoney_BackOffice(email, 31);
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second order payment if the first one was failed
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_After_Failed_Order()
        {
            _commonActions.SignIn_in_admin_panel();
            string email = _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            _commonActions.AddRegularOneDrawTicketToCart_SalesPanel("Powerball");
            _commonActions.PayForTicketsInCart_SalesPanel(WayToPay.Offline, true, true);
            
            _commonActions.Log_In_SalesPanel(email);
            double price = _commonActions.BuyRegularOneDrawTicket_SalesPanel("Mega Millions");

            if (price >= 30)
            {
                _balanceVerifications.CheckUserStoreCredit_BackOffice(email, 30);
            }
            else
            {
                _balanceVerifications.CheckUserStoreCredit_BackOffice(email, price);
            }
        }

        /// <summary>
        /// Cheks if a new user doesn't get 1+1 promotion for the second payment
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_Second_Payment()
        {
            _commonActions.SignIn_in_admin_panel();
            string email = _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            _commonActions.BuyRegularOneDrawTicket_SalesPanel("El Gordo");
            
            _commonActions.Sign_In_SalesPanel(email);
            _commonActions.BuyRaffleTicket_SalesPanel("Loteria de Navidad");

            _balanceVerifications.CheckUserStoreCredit_BackOffice(email, 0);
        }

        /// <summary>
        /// Checks if a user doesn't get 1+1 promotion for the second deposit
        /// </summary>
        [Test]
        public void One_Plus_One_Second_Deposit()
        {
            _commonActions.SignIn_in_admin_panel();
            string email = _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            _commonActions.DepositMoney_SalesPanel(11);

            _commonActions.DepositMoney_SalesPanel(18);

            _balanceVerifications.CheckUserStoreCredit_BackOffice(email, 11); //11*2+18 = 40
            _balanceVerifications.CheckUserSRealMoney_BackOffice(email, 29);
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion after buying a ticket
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_Promotion_Buying()
        {
            _commonActions.SignIn_in_admin_panel();
            string email = _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            double totalPrice = _commonActions.BuyRegularOneDrawTicket_SalesPanel("Euromiliony");

            if (totalPrice <= 30)
            {
                _balanceVerifications.CheckUserStoreCredit_BackOffice(email, totalPrice);
            }
            else
            {
                _balanceVerifications.CheckUserStoreCredit_BackOffice(email, 30);
            }
            
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion depositing money
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_Promotion_Deposit()
        {
            _commonActions.SignIn_in_admin_panel();
            string email = _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(10) + "@grr.la");

            _commonActions.DepositMoney_SalesPanel(18);

            _balanceVerifications.CheckUserStoreCredit_BackOffice(email, 18);
            _balanceVerifications.CheckUserSRealMoney_BackOffice(email, 18);
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
            _commonActions = new TestFramework.CommonActions(_driver);
            _balanceVerifications = new BalanceVerifications(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
