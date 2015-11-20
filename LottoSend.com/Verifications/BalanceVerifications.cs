﻿using System.Text;
using LottoSend.com.BackEndObj;
using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.TestCases;
using NUnit.Framework;
using OpenQA.Selenium;

namespace LottoSend.com.Verifications
{
    /// <summary>
    /// Includes all balance verifications
    /// </summary>
    public class BalanceVerifications
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        public StringBuilder Errors;
        private CommonActions _commonActions;

        public BalanceVerifications(IWebDriver newDriver)
        {
            _driver = newDriver;
            _driverCover = new DriverCover(_driver);
            Errors = new StringBuilder();
            _commonActions = new CommonActions(_driver);
        }

        /// <summary>
        /// Checks user's balance in back office on web users page
        /// </summary>
        /// <param name="email"></param>
        /// <param name="expected"></param>
        public void CheckUserBalance_BackOffice(string email, double expected)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/web_users");

            WebUsersPageObj users = new WebUsersPageObj(_driver);
            users.FilterByEmail(email);
            double actual = users.GetFirstRecordBalance();

            Assert.AreEqual(expected, actual, "Sorry but the balance is not as expected on page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks balance on front - account - deposit page. Must be logged in
        /// </summary>
        public void CheckBalanceOnDepositPage_Web(double expected)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "/en/account/deposits/new/");
            DepositObj deposit = new DepositObj(_driver);

            Assert.AreEqual(expected, deposit.Balance, "Sorry but the balance is not as expected on page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks balance on front - account - deposit page. Must be logged in
        /// </summary>
        public void CheckBalanceOnDepositPage_Mobile(double expected)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "/en/account/deposits/new/");
            DepositMobileObj deposit = new DepositMobileObj(_driver);

            Assert.AreEqual(expected, deposit.Balance, "Sorry but the balance is not as expected on page: " + _driverCover.Driver.Url + " ");
        }
    }
}
