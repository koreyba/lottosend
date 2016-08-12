using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using TestFramework;
using TestFramework.FrontEndObj.Cart;

namespace NewCombinedPageConfigTests.Verifications
{
    public class CartVerifications
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        public StringBuilder Errors;

        public CartVerifications(IWebDriver newDriver)
        {
            _driver = newDriver;
            _driverCover = new DriverCover(_driver);
            Errors = new StringBuilder();
        }

        /// <summary>
        /// Checks the number of tickets (all types) in the cart
        /// </summary>
        /// <param name="expected"></param>
        public void CheckNumberOfTicketsInCombinedCart_Front(int expected)
        {
            //Go to the cart 
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/payments/new/");

            //Check number of elemetns in the cart
            CartObj cart = new CartCombinedWebObj(_driver);
            Assert.AreEqual(expected, cart.NumberOfTickets, "There are " + cart.NumberOfTickets + " group tickets in the cart, but " + expected + " were expected ");
        }
    }
}
