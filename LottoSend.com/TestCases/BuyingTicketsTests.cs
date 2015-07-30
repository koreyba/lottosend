using LottoSend.com.FrontEndObj;
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
    public class BuyingTicketsTests
    {
        private IWebDriver _driver;

        [Test]
        public void BuyTicket()
        {

            DriverCover driver = new DriverCover(_driver);
            driver.NavigateToUrl(driver.BaseUrl + "en/web_users/sign_in");
            SignInPageOneObj signInOne = new SignInPageOneObj(_driver);

            signInOne.FillInFields(driver.Login, driver.Password);
            TopBarObj topBar = signInOne.ClickLogInButton();

            driver.NavigateToUrl(driver.BaseUrl + "en/plays/eurojackpot/");
            GamePageObj game = new GamePageObj(_driver);
            MerchantsObj merchants = game.ClickBuyTicketsButton();
            merchants.PayWithOfflineCharge();
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
