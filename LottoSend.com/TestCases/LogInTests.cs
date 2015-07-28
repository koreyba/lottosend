﻿using LottoSend.com.FrontEndObj;
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
    [TestFixture]
    public class LogInTests
    {
        private IWebDriver _driver;


        [Test]
        public void Login_On_SignIn_Page_Two()
        {
            DriverCover driver = new DriverCover(_driver);
            driver.NavigateToUrl("https://stg.lottobaba.com/en/web_users/sign_in2");
            SignInPageTwoObj signInOne = new SignInPageTwoObj(_driver);

            signInOne.FillInFields("fiwork@gmail.com", "11111111");
            TopBarObj topBar = signInOne.ClickLogInButton();

            if (!topBar.IsElementExists(topBar.MyAccount))
            {
                throw new NoSuchElementException("Log in was unseccessful ");
            }

            //TODO needs testing
            driver.NavigateToUrl("https://stg.lottobaba.com/en/carts/");
            CartObj cart = new CartObj(_driver);
            bool ticket = cart.IfTicketIsAdded("Powerball");

            if (!ticket)
            {
                throw new Exception("Pushed ticket was not found in the cart ");
            }
        }

        [Test]
        public void Login_On_SignIn_Page_One()
        {
            DriverCover driver = new DriverCover(_driver);
            driver.NavigateToUrl("https://stg.lottobaba.com/en/web_users/sign_in");
            SignInPageOneObj signInOne = new SignInPageOneObj(_driver);

            signInOne.FillInFields("fiwork@gmail.com", "11111111");
            TopBarObj topBar = signInOne.ClickLogInButton();

            if (!topBar.IsElementExists(topBar.MyAccount))
            {
                throw new NoSuchElementException("Log in was unseccessful ");
            }
        }

        [Test]
        public void Login_In_PopUp_Form()
        {
            DriverCover driver = new DriverCover(_driver);
            driver.NavigateToUrl("https://stg.lottobaba.com/en/");
            TopBarObj topBar = new TopBarObj(_driver);
            LogInPopUpObj loginPopUp = topBar.ClickLogInButton();

            loginPopUp.FillInFields("fiwork@gmail.com", "11111111");
            topBar = loginPopUp.ClickLogInButton();

            if (!topBar.IsElementExists(topBar.MyAccount))
            {
                throw new NoSuchElementException("Log in was unseccessful ");
            }
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
        }
    }
}
