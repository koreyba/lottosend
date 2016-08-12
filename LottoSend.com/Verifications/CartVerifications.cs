using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using TestFramework;
using TestFramework.FrontEndObj.Cart;

namespace LottoSend.com.Verifications
{
    public class CartVerifications
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        public StringBuilder Errors;
        private CommonActions _commonActions;

        public CartVerifications(IWebDriver newDriver)
        {
            _driver = newDriver;
            _driverCover = new DriverCover(_driver);
            Errors = new StringBuilder();
            _commonActions = new CommonActions(_driver);
        }

        /// <summary>
        /// Checks the number of tickets (all types) in the cart
        /// </summary>
        /// <param name="expected"></param>
        public void CheckNumberOfTicketsInCart_Front(int expected)
        {
            //Go to the cart 
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts/");

            //Check number of elemetns in the cart
            CartObj cart = new CartObj(_driver);
            Assert.AreEqual(expected, cart.NumberOfTickets, "There are " + cart.NumberOfTickets + " group tickets in the cart, but " + expected + " were expected ");
        }


        /// <summary>
        /// Checks the number of tickets (all types) in the cart
        /// </summary>
        /// <param name="expected"></param>
        public void CheckNumberOfTicketsInCart_SalesPanel(int expected)
        {
            //Go to the sales panel
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            //Check number of elemetns in the cart
            TestFramework.BackEndObj.SalesPanelPages.CartObj cart = new TestFramework.BackEndObj.SalesPanelPages.CartObj(_driver);
            Assert.AreEqual(expected, cart.NumberOfTickets, "There are " + cart.NumberOfTickets + " group tickets in the cart, but " + expected + " were expected ");
        } 
            

        /// <summary>
        /// Checks if ticket is not in the cart
        /// </summary>
        /// <param name="lotteryName"></param>
        public void CheckIfTicketIsNotInCart(string lotteryName)
        {
            //Go to the cart 
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts/");

            CartObj cart = new CartObj(_driver);
            Assert.IsTrue(!cart.IsTicketInCart(lotteryName), "The ticket " + lotteryName + " was not removed from the cart ");
        }
        ///<summary>
        /// Checks if ticket is in the cart
        /// </summary>
        /// <param name="lotteryName"></param>
        public void CheckIfTicketIsInCart(string lotteryName)
        {
            //Go to the cart 
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts/");
            CartObj cart = new CartObj(_driver);
            Assert.IsTrue(cart.IsTicketInCart(lotteryName), "The ticket " + lotteryName + " is not in the cart ");
        }
    }
}
