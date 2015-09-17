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
using LottoSend.com.BackEndObj.Verifications;

namespace LottoSend.com.TestCases
{
    /// <summary>
    /// Buys a group multi-draw ticket and performs all needed assertations 
    /// </summary>
    public class BuyGroupMultiDrawTicketTest
    {
        private IWebDriver _driver;
        private DriverCover driver;
        private double _totalPrice;
        private int _numberOfDraws;
        public OrderVerifications verifications;
        private CommonActions commonActions;

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
            verifications.CheckRecordTimeInDraw("EuroJackpot");
        }

        /// <summary>
        /// Checks an email in the first record (the last bet) 
        /// </summary>
        [Test]
        public void Check_Record_Email_In_Draw()
        {
            verifications.CheckRecordEmailInDraw("EuroJackpot");
        }

        /// <summary>
        /// Checks type of the ticket in the first record (must be Single)
        /// </summary>
        [Test]
        public void Check_Record_Type_In_Draw()
        {
            verifications.CheckRecordBetTypeInDraw("Bulk buy", "EuroJackpot");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end
        /// </summary>
        [Test]
        public void Check_Record_Price_In_Draw()
        {
            verifications.CheckRecordPriceInDraw(_totalPrice, _numberOfDraws);
        }

        /// <summary>
        /// Performs once before all other tests. Buys a group single ticket 
        /// </summary>
        [TestFixtureSetUp]
        public void Buy_Group_Multi_Draw_Ticket()
        {
            SetUp();

            // Log in     
            commonActions.Log_In_Front();

            driver.NavigateToUrl(driver.BaseUrl + "en/plays/eurojackpot/");

            //Pay for tickets
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);

            _totalPrice = groupGame.TotalPrice;
            _numberOfDraws = groupGame.NumberOfDraws;

            MerchantsObj merchants = groupGame.ClickBuyTicketsButton();
            merchants.PayWithOfflineCharge();

            //Go to admin panel
            commonActions.Authorize_in_admin_panel();

            //authorize payment in charge panel
            commonActions.Authorize_the_first_payment();

            //approve payment
            commonActions.Approve_offline_payment();

            CleanUp();
        }


        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
            if (verifications.Errors.Length > 0)
            {
                Assert.Fail(verifications.Errors.ToString());
            }
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            driver = new DriverCover(_driver);
            verifications = new OrderVerifications(_driver);
            commonActions = new CommonActions(_driver);
        }
    }
}
