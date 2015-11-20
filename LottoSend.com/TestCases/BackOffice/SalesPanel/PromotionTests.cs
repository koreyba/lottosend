using LottoSend.com.BackEndObj.SalesPanelPages;
using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.BackOffice.SalesPanel
{
    [TestFixture(typeof(ChromeDriver))]
    public class PromotionTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BalanceVerifications _verifications;
        private double _totalPrice;
        private BalanceVerifications _balanceVerifications;



        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second deposit when he had the first one pending and it was failed 
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_For_Second_Deposit_When_First_Failed()
        {

        }

        /// <summary>
        /// Cheks if a new user doesn't get 1+1 promotion for the second deposit if the first one is still pending
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_For_Second_Deposit_When_First_Pending()
        {

        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second order when he had the first one pending and it was failed 
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_For_Second_Order_When_First_Failed()
        {
           
        }

        /// <summary>
        /// Cheks if a new user doesn't get  1+1 promotion for the second order payment if the first one is pending
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_For_Second_Order_When_First_Pending()
        {
            
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second deposit payment if the first one was failed
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_After_Failed_Deposit()
        {
            
        }

        /// <summary>
        /// Cheks if a new user gets 1+1 promotion for the second order payment if the first one was failed
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_After_Failed_Order()
        {
          
        }

        /// <summary>
        /// Cheks if a new user doesn't get 1+1 promotion for the second payment
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_Second_Payment()
        {
          
        }

        /// <summary>
        /// Checks if a user doesn't get 1+1 promotion for the second deposit
        /// </summary>
        [Test]
        public void One_Plus_One_Second_Deposit()
        {
           
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion after buying a ticket
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_Promotion_Buying()
        {
       
        }

        /// <summary>
        /// Checks if a new user gets 1+1 promotion depositing money
        /// </summary>
        [Test]
        [Category("Critical")]
        public void One_Plus_One_Promotion_Deposit()
        {
            _commonActions.SignIn_in_admin_panel();
            string email = _commonActions.Sign_Up_SalesPanel(RandomGenerator.GenerateRandomString(7) + "@grr.la");

            _commonActions.AddCCDetails_SalesPanel();
            
            MenuObj menu = new MenuObj(_driver);
            menu.GoToDeposit();

            DepositBoxObj box = new DepositBoxObj(_driver);
            box.DepositOtherAmoun(18);

            _commonActions.Approve_offline_payment();

            _balanceVerifications.CheckUserBalance_BackOffice(email, 36);
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
            _balanceVerifications = new BalanceVerifications(_driver);
        }
    }
}
