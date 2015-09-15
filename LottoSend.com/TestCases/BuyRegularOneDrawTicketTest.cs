using LottoSend.com.BackEndObj;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.FrontEndObj.MyAccount;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoSend.com.TestCases
{
    /// <summary>
    /// Buys a regular one-draw ticket and performs all needed assertations 
    /// </summary>
    [TestFixture]
    public class BuyRegularOneDrawTicketTest : CommonActions
    {
        private IWebDriver _driver;
        private DriverCover driver;
        private double _totalPrice;
        private OrderVerifications verifications;

        /// <summary>
        /// Checks date of the first and seconds records in users account - transactions (front-end)
        /// </summary>
        [Test]
        public void Check_Transaction_Date_Front()
        {
            verifications.CheckTransactionDateFront();
        }

        /// <summary>
        /// Checks lottery name of the first and the second records in user's account - transactions in the front-end
        /// </summary>
        [Test]
        public void Check_Transaction_Lottery_Name_Front()
        {
            verifications.CheckTransactionLotteryNameFront();
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transactions_Email_In_Transactions()
        {
            verifications.CheckTransactionsEmailInTransactions();
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Merchant_In_Transactions()
        {
            verifications.CheckTransactionMerchantInTransactions();
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        [Test]
        public void Check_Transaction_Time_In_Transactions()
        {
            verifications.CheckTransactionTimeInTransactions();
        }

        /// <summary>
        /// Checks a time when the last bet (in the first record) was made
        /// </summary>
        [Test]
        public void Check_Record_Time_In_Draw()
        {
            verifications.CheckRecordTimeInDraw();
        }

        /// <summary>
        /// Checks an email in the first record (the last bet) 
        /// </summary>
        [Test]
        public void Check_Record_Email_In_Draw()
        {
            verifications.CheckRecordEmailInDraw();
        }

        /// <summary>
        /// Checks type of the ticket in the first record (must be Single)
        /// </summary>
        [Test]
        public void Check_Record_Type_In_Draw()
        {
            verifications.CheckRecordBetTypeInDraw("Single");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end
        /// </summary>
        [Test]
        public void Check_Record_Price_In_Draw()
        {
            verifications.CheckRecordPriceInDraw(_totalPrice);
        }

        /// <summary>
        /// Performs once before all other tests. Buys a regular single ticket 
        /// </summary>
        [TestFixtureSetUp]
        public void Buy_Regular_Signle_Ticket()
        {
            SetUp();

            // Log in     
            Log_In_Front();

            driver.NavigateToUrl(driver.BaseUrl + "en/plays/eurojackpot/");

            //Pay for tickets
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            _totalPrice = regularGame.TotalPrice;

            //Select single draw
            regularGame.SelectOneTimeEntryGame();

            MerchantsObj merchants = regularGame.ClickBuyTicketsButton();
            merchants.PayWithOfflineCharge();

            //Go to admin panel
            Authorize_in_admin_panel();

            //authorize payment in charge panel
            Authorize_the_first_payment();

            //approve payment
            Approve_offline_payment();

            CleanUp();
        }


        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
            if (verifications._errors.Length > 0)
            {
                Assert.Fail(verifications._errors.ToString());
            }
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            driver = new DriverCover(_driver);
            verifications = new OrderVerifications();
        }
    }
}
