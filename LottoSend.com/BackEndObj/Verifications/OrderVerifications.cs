using LottoSend.com.BackEndObj;
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
    public class OrderVerifications : CommonActions
    {
        private IWebDriver _driver;
        private DriverCover driver;
        public StringBuilder _errors;

        public OrderVerifications()
        {
            _driver = new ChromeDriver();
            driver = new DriverCover(_driver);
            _errors = new StringBuilder();
        }

        /// <summary>
        /// Checks is the type of the first bet qeuals to expected one
        /// </summary>
        /// <param name="type"></param>
        public void CheckRecordBetTypeInDraw(string type)
        {
            Authorize_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = Find_The_Draw_Page("EuroJackpot");

            Assert.AreEqual(type, draw.Type, "Sorry, the type of the bet is expected to be " + type + " but was " + draw.Type + ", page: " + driver.Driver.Url + " ");
        }

        /// <summary>
        /// Checks a time when the last bet (in the first record) was made
        /// </summary>
        public void CheckRecordTimeInDraw()
        {
            Authorize_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = Find_The_Draw_Page("EuroJackpot");

            bool correctTime = draw.CheckTime(3);
            Assert.IsTrue(correctTime, "Sorry, the time of the first record is not in set interval. Check if record was added , page: " + driver.Driver.Url + " ");
        }

        /// <summary>
        /// Checks date of the first and seconds records in users account - transactions (front-end)
        /// </summary>
        public void CheckTransactionDateFront()
        {
            Log_In_Front();
            driver.NavigateToUrl(driver.BaseUrl + "en/account/balance/");
            TransactionObj myTransactions = new TransactionObj(_driver);

            //make month to be in right format
            string digit = DateTime.Now.Month.ToString();
            string month = "";
            if (digit.Length == 1)
                month = "0" + digit;
            else
                month = digit;

            //make day to be in right format
            digit = DateTime.Now.Day.ToString();
            string day = "";
            if (digit.Length == 1)
                day = "0" + digit;
            else
                day = digit;

            string date = day + "/" + month + "/" + DateTime.Now.Year;
            if (!myTransactions.FirstRecordDate.Equals(date))
            {
                _errors.Append("The first record has not current date, please check it, page: " + driver.Driver.Url + " ");
            }

            if (!myTransactions.SecondRecordDate.Equals(date))
            {
                _errors.Append("The second record has not current date, please check it, page: " + driver.Driver.Url + " ");
            }
        }

        /// <summary>
        /// Checks lottery name of the first and the second records in user's account - transactions in the front-end
        /// </summary>
        public void CheckTransactionLotteryNameFront()
        {
            Log_In_Front();
            driver.NavigateToUrl(driver.BaseUrl + "en/account/balance/");
            TransactionObj myTransactions = new TransactionObj(_driver);

            //Second record lottery name must be equal to...
            if (!myTransactions.SecondRecordLottery.Equals("EuroJackpot"))
            {
                _errors.Append("The second record has different lottery name, please check it, page: " + driver.Driver.Url + " ");
            }

            //First record lottery name must be empty
            if (!myTransactions.FirstRecordLottery.Equals(""))
            {
                _errors.Append("The first record is supossed to have no lottery name, please check it, page: " + driver.Driver.Url + " ");
            }
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        public void CheckTransactionsEmailInTransactions()
        {
            Authorize_in_admin_panel();
            driver.NavigateToUrl(driver.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);
            bool correctEmail = transaction.CheckEmail(driver.Login);

            Assert.IsTrue(correctEmail, "Sorry, the email in the first record is wrong, check if a record was added, page: " + driver.Driver.Url + " ");
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        public void CheckTransactionMerchantInTransactions()
        {
            Authorize_in_admin_panel();
            driver.NavigateToUrl(driver.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);
            bool correctMerchant = transaction.CheckMerchant("Offline");

            Assert.IsTrue(correctMerchant, "Sorry, the merchant in the first record is wrong, check if a record was added, page: " + driver.Driver.Url + " ");
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        public void CheckTransactionTimeInTransactions()
        {
            Authorize_in_admin_panel();
            driver.NavigateToUrl(driver.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);
            bool correctTime = transaction.CheckTime(3);

            Assert.IsTrue(correctTime, "Sorry, the time of the first record is not in set interval. Check if record was added, page: " + driver.Driver.Url + " ");
        }

        /// <summary>
        /// Checks an email in the first record (the last bet) 
        /// </summary>
        public void CheckRecordEmailInDraw()
        {
            Authorize_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = Find_The_Draw_Page("EuroJackpot");

            bool correctEmail = draw.CheckEmail(driver.Login);
            Assert.IsTrue(correctEmail, "Sorry, the email in the first record is wrong, check if a record was added, page: " + driver.Driver.Url + " ");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end. Works for single ticket (now multi-draw)
        /// </summary>
        public void CheckRecordPriceInDraw(double totalPrice)
        {
            Authorize_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = Find_The_Draw_Page("EuroJackpot");
            Assert.AreEqual(totalPrice, draw.BetAmount, "Sorry, the price for the bet is  " + draw.BetAmount + " but " + totalPrice + " was expected, page: " + driver.Driver.Url + " ");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end. Works for multi-draw ticket (not single)
        /// </summary>
        public void CheckRecordPriceInDraw(double totalPrice, int numberOfDraws)
        {
            Authorize_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = Find_The_Draw_Page("EuroJackpot");
            Assert.AreEqual(totalPrice / numberOfDraws, draw.BetAmount, "Sorry, the price for the bet is  " + draw.BetAmount + " but " + totalPrice / numberOfDraws + " was expected, page: " + driver.Driver.Url + " ");
        }
    }
}
