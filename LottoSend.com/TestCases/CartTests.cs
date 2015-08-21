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
    public class CartTests
    {
        private IWebDriver _driver;

        [Test]
        public void Delete_Item_From_Cart()
        {
            DriverCover driver = new DriverCover(_driver);
            driver.NavigateToUrl(driver.BaseUrl + "en/plays/eurojackpot/");
            SingleGamePageObj game = new SingleGamePageObj(_driver);
            game.ClickStandartGameButton();

            game.ClickAddToCartButton();

            driver.NavigateToUrl(driver.BaseUrl + "en/carts/");
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket("EuroJackpot");
            if(cart.IsTicketInCart("EuroJackpot"))
            {
                throw new Exception("The ticket was not removed from the cart ");
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
