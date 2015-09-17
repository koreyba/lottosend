﻿using System;
using System.Text;
using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.TestCases;
using NUnit.Framework;
using OpenQA.Selenium;

namespace LottoSend.com.BackEndObj.Verifications
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
        /// Checks is amount in the first transaction equals expected (front - transactions)
        /// </summary>
        /// <param name="expectedAmount"></param>
        public void CheckAmountInTransactionFront(double expectedAmount)
        {
            _commonActions.Log_In_Front();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/balance/");
            TransactionObj transaction = new TransactionObj(_driver);

            Assert.AreEqual(expectedAmount, transaction.FirstRecordAmount, "Sorry, but the amount of the first record is not as expected. Page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks a type of the first transaction on Front-Transaction history
        /// </summary>
        /// <param name="expectedType"></param>
        public void CheckTypeOfTransactionFront(string expectedType)
        {
            _commonActions.Log_In_Front();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/balance/");
            TransactionObj transaction = new TransactionObj(_driver);

            Assert.AreEqual(expectedType, transaction.FirstRecordType, "Sorry, but the type of the first record is not as expected. Page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks is the type of the first bet qeuals to expected one
        /// </summary>
        /// <param name="type"></param>
        /// <param name="lotteryName"></param>
        public void CheckRecordBetTypeInDraw(string type, string lotteryName)
        {
            _commonActions.Authorize_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = _commonActions.Find_The_Draw_Page(lotteryName);

            Assert.AreEqual(type, draw.Type, "Sorry, the type of the bet is expected to be " + type + " but was " + draw.Type + ", page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks a time when the last bet (in the first record) was made
        /// </summary>
        public void CheckRecordTimeInDraw(string lotteryName)
        {
            _commonActions.Authorize_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = _commonActions.Find_The_Draw_Page(lotteryName);

            bool correctTime = draw.CheckTime(3);
            Assert.IsTrue(correctTime, "Sorry, the time of the first record is not in set interval. Check if record was added , page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks date of the first and seconds records in users account - transactions (front-end)
        /// </summary>
        public void CheckTransactionDateFront()
        {
            _commonActions.Log_In_Front();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/balance/");
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
                Errors.Append("The first record has not current date, please check it, page: " + _driverCover.Driver.Url + " ");
            }

            if (!myTransactions.SecondRecordDate.Equals(date))
            {
                Errors.Append("The second record has not current date, please check it, page: " + _driverCover.Driver.Url + " ");
            }
        }

        /// <summary>
        /// Checks lottery name of the first and the second records in user's account - transactions in the front-end
        /// </summary>
        public void CheckTransactionLotteryNameFront()
        {
            _commonActions.Log_In_Front();
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/balance/");
            TransactionObj myTransactions = new TransactionObj(_driver);

            //Second record lottery name must be equal to...
            if (!myTransactions.SecondRecordLottery.Equals("EuroJackpot"))
            {
                Errors.Append("The second record has different lottery name, please check it, page: " + _driverCover.Driver.Url + " ");
            }

            //First record lottery name must be empty
            if (!myTransactions.FirstRecordLottery.Equals(""))
            {
                Errors.Append("The first record is supossed to have no lottery name, please check it, page: " + _driverCover.Driver.Url + " ");
            }
        }

        /// <summary>
        /// Cheks the email of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        public void CheckTransactionsEmailInTransactions()
        {
            _commonActions.Authorize_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);
            bool correctEmail = transaction.CheckEmail(_driverCover.Login);

            Assert.IsTrue(correctEmail, "Sorry, the email in the first record is wrong, check if a record was added, page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Cheks the merchant of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        public void CheckTransactionMerchantInTransactions()
        {
            _commonActions.Authorize_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);
            bool correctMerchant = transaction.CheckMerchant("Offline");

            Assert.IsTrue(correctMerchant, "Sorry, the merchant in the first record is wrong, check if a record was added, page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Cheks the time of the last transaction (the first record) on "Back - Transactions" page
        /// </summary>
        public void CheckTransactionTimeInTransactions()
        {
            _commonActions.Authorize_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);
            bool correctTime = transaction.CheckTime(3);

            Assert.IsTrue(correctTime, "Sorry, the time of the first record is not in set interval. Check if record was added, page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks an email in the first record (the last bet) 
        /// </summary>
        public void CheckRecordEmailInDraw(string lotteryName)
        {
            _commonActions.Authorize_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = _commonActions.Find_The_Draw_Page(lotteryName);

            bool correctEmail = draw.CheckEmail(_driverCover.Login);
            Assert.IsTrue(correctEmail, "Sorry, the email in the first record is wrong, check if a record was added, page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end. Works for single ticket (now multi-draw)
        /// </summary>
        public void CheckRecordPriceInDraw(double totalPrice)
        {
            _commonActions.Authorize_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = _commonActions.Find_The_Draw_Page("EuroJackpot");
            Assert.AreEqual(totalPrice, draw.BetAmount, "Sorry, the price for the bet is  " + draw.BetAmount + " but " + totalPrice + " was expected, page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks price of the last bet (the first record). Must be the same as in the front-end. Works for multi-draw ticket (not single)
        /// </summary>
        public void CheckRecordPriceInDraw(double totalPrice, int numberOfDraws)
        {
            _commonActions.Authorize_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = _commonActions.Find_The_Draw_Page("EuroJackpot");
            Assert.AreEqual(totalPrice / numberOfDraws, draw.BetAmount, "Sorry, the price for the bet is  " + draw.BetAmount + " but " + totalPrice / numberOfDraws + " was expected, page: " + _driverCover.Driver.Url + " ");
        }
    }
}
