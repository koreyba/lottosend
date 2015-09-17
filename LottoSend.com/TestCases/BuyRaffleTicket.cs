using LottoSend.com.BackEndObj.Verifications;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.GamePages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases
{
    [TestFixture]
    public class BuyRaffleTicket
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private double _totalPrice;
        private OrderVerifications _verifications;
        private CommonActions _commonActions;

        /// <summary>
        /// Performs once before all other tests. Buys a raffle ticket
        /// </summary>
        [TestFixtureSetUp]
        public void Buy_Regular_One_Draw_Ticket()
        {
            SetUp();

            // Log in     
            _commonActions.Log_In_Front();

            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);
            _totalPrice = rafflePage.TotalPrice;

            CartObj cart = rafflePage.ClickBuyNowButton();
            MerchantsObj merchants = cart.ClickProceedToCheckoutButton();

            merchants.PayWithOfflineCharge();
            
            _commonActions.Authorize_in_admin_panel();

            _commonActions.Authorize_the_first_payment();

            _commonActions.Approve_offline_payment();

            CleanUp();
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
            if (_verifications.Errors.Length > 0)
            {
                Assert.Fail(_verifications.Errors.ToString());
            }
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
