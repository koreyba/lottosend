﻿using System.Text;
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
        /// Checks balance on front - account - deposit page. Must be logged in
        /// </summary>
        public void CheckBalanceOnDepositPage(double expected)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "/account/deposits/new/");
            DepositObj deposit = new DepositObj(_driver);

            Assert.AreEqual(expected, deposit.Balance, "Sorry but the balance is not as expected on page: " + _driverCover.Driver.Url + " ");
        }

        /// <summary>
        /// Checks balance on front - account - deposit page. Must be logged in
        /// </summary>
        public void CheckBalanceOnDepositPageMobile(double expected)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "/account/deposits/new/");
            DepositMobileObj deposit = new DepositMobileObj(_driver);

            Assert.AreEqual(expected, deposit.Balance, "Sorry but the balance is not as expected on page: " + _driverCover.Driver.Url + " ");
        }
    }
}
