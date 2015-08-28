using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.GamePages;
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
        private DriverCover driver;

        /// <summary>
        /// Adds two different lottery group ticket to the cart and deletes them. Check if were added and deleted
        /// </summary>
        [Test]
        public void Delete_two_group_ticket_from_cart()
        {
            //Add two tickets from different lotteries
            Add_group_ticket_to_cart("en/plays/eurojackpot/");
            Add_group_ticket_to_cart("en/plays/eurojackpot/");

            Check_number_of_tickets_in_cart(2);

            //Remove tickets
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket("EuroJackpot");
            cart.DeleteTicket("SuperEnalotto");

            //Check if tickets are still present
            Check_if_ticket_is_not_in_cart("EuroJackpot");
            Check_if_ticket_is_not_in_cart("SuperEnalotto");

            Check_number_of_tickets_in_cart(0);

        }

        private void Check_number_of_tickets_in_cart(int expected)
        {
            //Go to the cart 
            driver.NavigateToUrl(driver.BaseUrl + "en/carts/");

            //Check number of elemetns in the cart
            CartObj cart = new CartObj(_driver);
            if (cart.NumberOfGroupTickets != expected)
            {
                throw new Exception("There are " + cart.NumberOfTickets + " group tickets in the cart, but " + expected + " were expected ");
            }
        }

        private void Add_group_ticket_to_cart(string adress)
        {
            //Add ticket to the cart
            driver.NavigateToUrl(driver.BaseUrl + adress);
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);
            groupGame.ClickAddToCartButton();
        }

        /// <summary>
        /// Adds single ticket to cart and removes it. Checks if there is no tickets of a specific lottery game
        /// </summary>
        [Test]
        public void Delete_Single_Ticket_From_Cart()
        {
            driver.NavigateToUrl(driver.BaseUrl + "en/plays/eurojackpot/");
            RegularGamePageObj game = new RegularGamePageObj(_driver);
            game.ClickStandartGameButton();

            game.ClickAddToCartButton();

            driver.NavigateToUrl(driver.BaseUrl + "en/carts/");
            CartObj cart = new CartObj(_driver);
            cart.DeleteTicket("EuroJackpot");
            Check_if_ticket_is_not_in_cart("EuroJackpot");

        }

        private void Check_if_ticket_is_not_in_cart(string lotteryName)
        {
            CartObj cart = new CartObj(_driver);
            if (cart.IsTicketInCart(lotteryName))
            {
                throw new Exception("The ticket " + lotteryName + " was not removed from the cart ");
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
            driver = new DriverCover(_driver);
        }
    }
}
