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
    public class SignUpTests
    {
        private IWebDriver _driver;

        /// <summary>
        /// Sign up in pop up form
        /// </summary>
        [Test]
        public void Sign_Up_In_Pop_Up()
        {
            DriverCover driver = new DriverCover(_driver);
            driver.NavigateToUrl(driver.BaseUrl + "en/");

            TopBarObj topBar = new TopBarObj(_driver);
            SignUpPopUpObj popUp = topBar.ClickSignUpButton();
            popUp.FillInFields();
            SignUpSuccessObj success = popUp.ClickSignUp();

            if (!topBar.IsElementExists(topBar.MyAccount))
            {
                throw new NoSuchElementException("Sign up was unseccessful ");
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
