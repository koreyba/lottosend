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
    public class CartTestCases
    {
        private IWebDriver _driver;

        public void Delete_Item_From_Cart()
        {
            DriverCover driver = new DriverCover(_driver);
            driver.NavigateToUrl(driver.BaseUrl + "en/plays/eurojackpot/");
            GamePageObj game = new GamePageObj(_driver);

            game.ClickAddToCartButton();

            driver.NavigateToUrl(driver.BaseUrl + "en/carts/");
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket("EuroJackpot");


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
