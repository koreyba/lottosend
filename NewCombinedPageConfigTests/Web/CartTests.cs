using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NewCombinedPageConfigTests.Web
{
    /// <summary>
    /// Includes tests of the cart (front)
    /// </summary>
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    public class CartTests<TWebDriver> : LottoSend.com.TestCases.Web.CartTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        /// <summary>
        /// Adds single ticket to cart and removes it. Checks if there is no tickets of a specific lottery game
        /// </summary>
        [TestCase(false)]
        [TestCase(true)]
        [Category("Critical")]
        [Category("Parallel")]
        public new void Delete_Single_Ticket_From_Cart(bool toLogin)
        {
            base.Delete_Single_Ticket_From_Cart(toLogin);
        }

        [TearDown]
        public new void CleanUp()
        {
            base.CleanUp();
        }
    }
}
