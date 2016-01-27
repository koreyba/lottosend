using System;
using System.Text;
using LottoSend.com.BackEndObj;
using LottoSend.com.BackEndObj.DrawPages;
using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.TestCases;
using NUnit.Framework;
using OpenQA.Selenium;

namespace LottoSend.com.Verifications
{
    /// <summary>
    /// Includes all verifications for an order (group/regular/raffle tickets)
    /// </summary>
    public class OrderVerifications
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        public StringBuilder Errors;
        private CommonActions _commonActions;

        public OrderVerifications(IWebDriver newDriver)
        {
            _driver = newDriver;
            _driverCover = new DriverCover(_driver);
            Errors = new StringBuilder();
            _commonActions = new CommonActions(_driver);
        }

        /// <summary>
        /// Check balance of the user based on his previous balance and amount of new transaction
        /// </summary>
        /// <param name="formerBalance">Balance before transaction</param>
        /// <param name="transactionAmount">Amount of money that was deposited</param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public void CheckUserBalance_Front(double formerBalance, double transactionAmount, string email, string password)
        {
            _commonActions.Log_In_Front(email, password);
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/deposits/new/");
            DepositObj depoist = new DepositObj(_driver);

            Assert.AreEqual(depoist.Balance, formerBalance + transactionAmount, "Sorry but balance of user is wrong. ");
        }

        /// <summary>
        /// Checks if amount in the first transaction equals expected (back - transactions)
        /// </summary>
        /// <param name="expectedAmount"></param>
        /// <param name="numberOfRecord">Which record to check, the first one or the second one</param>
        public void CheckAmountInTransactions_Back(double expectedAmount, int numberOfRecord = 1)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);

            if (numberOfRecord == 1)
            {
                Assert.AreEqual(expectedAmount, transaction.FirstRecordAmount,
                    "Sorry, but the amount of the first record is not as expected. Page: " + _driverCover.Driver.Url +
                    " ");
            }

            if (numberOfRecord == 2)
            {
                Assert.AreEqual(expectedAmount, transaction.SecondRecordAmount,
                    "Sorry, but the amount of the first record is not as expected. Page: " + _driverCover.Driver.Url +
                    " ");
            }
        }

        /// <summary>
        /// Checks is amount in the first transaction equals expected (front - transactions)
        /// </summary>
        /// <param name="expectedAmount"></param>
        /// <param name="email">User's email to log in</param>
        /// <param name="password">User's password</param>
        /// <param name="numberOfRecord">Which record to check, the first one or the second one</param>
        public void CheckAmountInTransaction_Front(double expectedAmount, string email, string password, int numberOfRecord = 1)
        {
            _commonActions.Log_In_Front(email, password);
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/balance/");
            TransactionObj transaction = new TransactionObj(_driver);

            if (numberOfRecord == 1)
            {
                Assert.AreEqual(expectedAmount, transaction.FirstRecordAmount,
                    "Sorry, but the amount of the first record is not as expected. Page: " + _driverCover.Driver.Url +
                    " ");
            }

            if (numberOfRecord == 2)
            {
                Assert.AreEqual(expectedAmount, transaction.SecondRecordAmount,
                    "Sorry, but the amount of the first record is not as expected. Page: " + _driverCover.Driver.Url +
                    " ");
            }
        }

        /// <summary>
        /// Checks a type of the first transaction on Front-Transaction history
        /// </summary>
        /// <param name="expectedType"></param>
        /// <param name="email">User's email to log in</param>
        /// <param name="password">User's password</param>
        /// <param name="numberOfRecord">Which record to check, the first one or the second one</param>
        public void CheckTypeOfTransaction_Front(string expectedType, string email, string password, int numberOfRecord = 1)
        {
            _commonActions.Log_In_Front(email, password);
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/balance/");
            TransactionObj transaction = new TransactionObj(_driver);

            if (numberOfRecord == 1)
            {
                Assert.AreEqual(expectedType, transaction.FirstRecordType,
                    "Sorry, but the type of the first record is not as expected. Page: " + _driverCover.Driver.Url + " ");
            }

            if (numberOfRecord == 2)
            {
                Assert.AreEqual(expectedType, transaction.SecondRecordType,
                    "Sorry, but the type of the first record is not as expected. Page: " + _driverCover.Driver.Url + " ");
            }
        }

        /// <summary>
        /// Checks a type of the first transaction on Front-Transaction history
        /// </summary>
        /// <param name="expectedType"></param>
        /// <param name="numberOfRecord">Which record to check, the first one or the second one</param>
        public void CheckPlayTypeInTransactions_Back(string expectedType, int numberOfRecord = 1)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);

            if (numberOfRecord == 1)
            {
                Assert.AreEqual(expectedType, transaction.FirstRecordPlayType,
                    "Sorry, but the type of the first record is not as expected. Page: " + _driverCover.Driver.Url + " ");
            }

            if (numberOfRecord == 2)
            {
                Assert.AreEqual(expectedType, transaction.SecondRecordPlayType,
                    "Sorry, but the type of the first record is not as expected. Page: " + _driverCover.Driver.Url + " ");
            }
        }

        /// <summary>
        /// Checks a type of the first transaction on Front-Transaction history
        /// </summary>
        /// <param name="expectedType"></param>
        /// <param name="numberOfRecord">Which record to check, the first one or the second one</param>
        public void CheckTransactionTypeInTransactions_Back(string expectedType, int numberOfRecord = 1)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);

            if (numberOfRecord == 1)
            {
                Assert.AreEqual(expectedType, transaction.FirstRecordTransactionType,
                    "Sorry, but the type of the first record is not as expected. Page: " + _driverCover.Driver.Url + " ");
            }

            if (numberOfRecord == 2)
            {
                Assert.AreEqual(expectedType, transaction.SecondRecordTransactionType,
                    "Sorry, but the type of the first record is not as expected. Page: " + _driverCover.Driver.Url + " ");
            }
        }

        /// <summary>
        /// Checks date of the first and seconds records in users account - transactions (front-end)
        /// </summary>
        /// <param name="email">User's email to log in</param>
        /// <param name="password">User's password</param>
        /// <param name="numberOfRecord">Which record to check, the first one or the second one</param>
        public void CheckTransactionDate_Front(string email, string password, int numberOfRecord = 1)
        {
            _commonActions.Log_In_Front(email, password);
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/balance/");
            TransactionObj myTransactions = new TransactionObj(_driver);

            //make month to be in right format
            string digit = DateTime.Now.Month.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string month;
            if (digit.Length == 1)
                month = "0" + digit;
            else
                month = digit;

            //make day to be in right format
            digit = DateTime.Now.Day.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string day;
            if (digit.Length == 1)
                day = "0" + digit;
            else
                day = digit;

            string date = day + "/" + month + "/" + DateTime.Now.Year;

            if (numberOfRecord == 1)
            {
                Assert.AreEqual(date, myTransactions.FirstRecordDate, "The first record doesn't have current date, please check it, page: " +
                                  _driverCover.Driver.Url + " ");
            }

            if (numberOfRecord == 2)
            {
                Assert.AreEqual(date, myTransactions.SecondRecordDate, "The second record doesn't have current date, please check it, page: " +
                                  _driverCover.Driver.Url + " ");
            }
        }

        /// <summary>
        /// Checks lottery name of the first and the second records in user's account - transactions in the front-end
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email">User's email to log in</param>
        /// <param name="password">User's password</param>
        /// <param name="numberOfRecord">Which record to check, the first one or the second one</param>
        public void CheckTransactionLotteryName_Front(string name, string email, string password, int numberOfRecord = 1)
        {
            _commonActions.Log_In_Front(email, password);
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/balance/");
            TransactionObj myTransactions = new TransactionObj(_driver);

            if (numberOfRecord == 1)
            {
                Assert.AreEqual(name, myTransactions.FirstRecordLottery, "The first record has different lottery name, please check it, page: " +
                                  _driverCover.Driver.Url + " ");
            }

            if (numberOfRecord == 2)
            {
                Assert.AreEqual(name, myTransactions.SecondRecordLottery, "The second record has different lottery name, please check it, page: " +
                                  _driverCover.Driver.Url + " ");
            }
        }

        /// <summary>
        /// Checks is the type of the first bet qeuals to expected one
        /// </summary>
        /// <param name="type"></param>
        /// <param name="lotteryName"></param>
        public void CheckRecordBetTypeInDraw(string type, string lotteryName)
        {
            _commonActions.SignIn_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = _commonActions.Find_The_Draw_Page(lotteryName);

            Assert.AreEqual(type, draw.Type, "Sorry, the type of the bet is expected to be " + type + " but was " + draw.Type + ", page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks a time when the last bet (in the first record) was made
        /// </summary>
        public void CheckRecordTimeInDraw(string lotteryName)
        {
            _commonActions.SignIn_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = _commonActions.Find_The_Draw_Page(lotteryName);

            TimeSpan transactionDate = draw.GetTransactionDate();
            TimeSpan currentUtcDate = DateTime.UtcNow.TimeOfDay;

            //Cheks if the transaction date placed in correct interval (no longer then 5 min from now)
            Assert.IsTrue(transactionDate > currentUtcDate - TimeSpan.FromMinutes(5), "Sorry, the time of the first record is not in set interval. Check if record was added, page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        public void CheckTransactionsEmailInTransactions_Back(string email)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);
            string realEmail = transaction.GetFirstTransactionEmail();

            Assert.AreEqual(email, realEmail, "Sorry, the email in the first record is wrong, check if a record was added, page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        /// <param name="merchant">Payment method</param>
        public void CheckTransactionMerchantInTransactions_Back(WayToPay merchant)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);

            string transactionMerchant = transaction.GetFirstTransactionMerchant();

            Assert.AreEqual(merchant.ToString(System.Globalization.CultureInfo.InvariantCulture), transactionMerchant, "Sorry, the merchant in the first record is wrong, check if a record was added, page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        public void CheckTransactionTimeInTransactions_Back()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);

            TimeSpan transactionDate = transaction.GetFirstTransactionDate();
            TimeSpan currentUtcDate = DateTime.UtcNow.TimeOfDay;

            //Cheks if the transaction date placed in correct interval (no longer then 5 min from now)
            Assert.IsTrue(transactionDate > currentUtcDate - TimeSpan.FromMinutes(5), "Sorry, the time of the first record is not in set interval. Check if record was added, page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks the state of the first transaction
        /// </summary>
        /// <param name="expectedState"></param>
        public void CheckTransactionsStateInTransactions_Back(string expectedState)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);
            string state = transaction.GetFirstTransactionState();

            Assert.AreEqual(expectedState, state, "Sorry but the transaction state is wrong. ");
        }

        /// <summary>
        /// Checks an email in the first record (the last bet) 
        /// </summary>
        public void CheckRecordEmailInDraw(string lotteryName, string expectedEmail)
        {
            _commonActions.SignIn_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = _commonActions.Find_The_Draw_Page(lotteryName);

            string currentEmail = draw.GetEmail();
            Assert.AreEqual(expectedEmail, currentEmail, "Sorry, the email in the first record is wrong, check if a record was added, page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end. Works for single ticket (now multi-draw)
        /// </summary>
        public void CheckRecordPriceInDraw(double totalPrice, string lottery)
        {
            _commonActions.SignIn_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = _commonActions.Find_The_Draw_Page(lottery);
            Assert.AreEqual(totalPrice, draw.BetAmount, "Sorry, the price for the bet is  " + draw.BetAmount + " but " + totalPrice + " was expected, page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end. Works for multi-draw ticket (not single)
        /// </summary>
        public void CheckRecordPriceInDraw(double totalPrice, int numberOfDraws)
        {
            _commonActions.SignIn_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = _commonActions.Find_The_Draw_Page("Eurojackpot");
            Assert.AreEqual(Math.Round(totalPrice / numberOfDraws, 2, MidpointRounding.ToEven), draw.BetAmount, "Sorry, the price for the bet is  " + draw.BetAmount + " but " + totalPrice / numberOfDraws + " was expected, page: " + _driverCover.Driver.Url + " ");
        }
    }
}
