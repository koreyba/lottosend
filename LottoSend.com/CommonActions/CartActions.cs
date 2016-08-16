using OpenQA.Selenium;
using TestFramework;
using TestFramework.FrontEndObj.Cart;

namespace LottoSend.com.CommonActions
{
    public class CartActions
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;

        public CartActions(IWebDriver newDriver)
        {
            _driver = newDriver;
            _driverCover = new DriverCover(_driver);
        }

        /// <summary>
        /// Removes all ticket from the cart. Needs previous login
        /// </summary>
        public void DeleteAllTicketFromCart_Front()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts/");
            CartObj cart = new CartSiteObj(_driver);
            cart.DeleteAllTickets();
        }
    }
}
