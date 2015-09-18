using LottoSend.com.BackEndObj.Verifications;
using LottoSend.com.FrontEndObj.GamePages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LottoSend.com.TestCases.Group_ticktes
{
    /// <summary>
    /// Buys a group multi-draw ticket and performs all needed assertations 
    /// </summary>
    [TestFixture]
    public class BuyGroupMultiDrawTicketTest
    {
        private IWebDriver _driver;
        private DriverCover driver;
        private double _totalPrice;
        private int _numberOfDraws;
        private OrderVerifications _verifications;
        private CommonActions _commonActions;

        /// <summary>
        /// Checks an amount in the first record in transactions (front)
        /// </summary>
        [Test]
        public void Check_Amount_In_Transaction_Front()
        {
            _verifications.CheckAmountInTransactionFront(_totalPrice);
        }

        /// <summary>
        /// Checks a type of the first record in transactions (front)
        /// </summary>
        [Test]
        public void Check_Type_Of_Transaction_Front()
        {
            _verifications.CheckTypeOfTransactionFront("Play - Bulk buy");
        }

        /// <summary>
        /// Checks date of the first and seconds records in users account - transactions (front-end)
        /// </summary>
        [Test]
        public void Check_Transaction_Date_Front()
        {
            _verifications.CheckTransactionDateFront();
        }

        /// <summary>
        /// Checks lottery name of the first and the second records in user's account - transactions in the front-end
        /// </summary>
        [Test]
        public void Check_Transaction_Lottery_Name_Front()
        {
            _verifications.CheckTransactionLotteryNameFront();
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_Email_In_Transactions()
        {
            _verifications.CheckTransactionsEmailInTransactions();
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Merchant_In_Transactions()
        {
            _verifications.CheckTransactionMerchantInTransactions();
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Time_In_Transactions()
        {
            _verifications.CheckTransactionTimeInTransactions();
        }

        /// <summary>
        /// Checks a time when the last bet (in the first record) was made
        /// </summary>
        [Test]
        public void Check_Record_Time_In_Draw()
        {
            _verifications.CheckRecordTimeInDraw("EuroJackpot");
        }

        /// <summary>
        /// Checks an email in the first record (the last bet) 
        /// </summary>
        [Test]
        public void Check_Record_Email_In_Draw()
        {
            _verifications.CheckRecordEmailInDraw("EuroJackpot");
        }

        /// <summary>
        /// Checks type of the ticket in the first record (must be Single)
        /// </summary>
        [Test]
        public void Check_Record_Type_In_Draw()
        {
            _verifications.CheckRecordBetTypeInDraw("Bulk buy", "EuroJackpot");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end
        /// </summary>
        [Test]
        public void Check_Record_Price_In_Draw()
        {
            _verifications.CheckRecordPriceInDraw(_totalPrice, _numberOfDraws);
        }

        /// <summary>
        /// Performs once before all other tests. Buys a group single ticket 
        /// </summary>
        [TestFixtureSetUp]
        public void Buy_Group_Multi_Draw_Ticket()
        {
            SetUp();

            // Log in     
            _commonActions.Log_In_Front();

            driver.NavigateToUrl(driver.BaseUrl + "en/plays/eurojackpot/");

            //Pay for tickets
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);

            _totalPrice = groupGame.TotalPrice;
            _numberOfDraws = groupGame.NumberOfDraws;

            MerchantsObj merchants = groupGame.ClickBuyTicketsButton();
            merchants.PayWithOfflineCharge();

            //Go to admin panel
            _commonActions.Authorize_in_admin_panel();

            //authorize payment in charge panel
            _commonActions.Authorize_the_first_payment();

            //approve payment
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
            driver = new DriverCover(_driver);
            _verifications = new OrderVerifications(_driver);
            _commonActions = new CommonActions(_driver);
        }
    }
}
