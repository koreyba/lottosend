using System.Text;
using LottoSend.com.FrontEndObj;
using LottoSend.com.TestCases;
using NUnit.Framework;
using OpenQA.Selenium;

namespace LottoSend.com.Verifications
{
    public class WebUserVerifications
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        public StringBuilder Errors;
        private CommonActions _commonActions;

        public WebUserVerifications(IWebDriver newDriver)
        {
            _driver = newDriver;
            _driverCover = new DriverCover(_driver);
            Errors = new StringBuilder();
            _commonActions = new CommonActions(_driver);
        }

        /// <summary>
        /// Checks if in the current session a web user is signed in
        /// </summary>
        public void CheckIfSignedIn_Web()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl);
            TopBarObj topBar = new TopBarObj(_driver);

            Assert.IsTrue(topBar.IsElementExists(topBar.MyAccount), "Sign up was unseccessful ");
        }

        /// <summary>
        /// Checks if in the current session a web user is signed in
        /// </summary>
        public void CheckIfSignedIn_Mobile()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl);
            Assert.IsTrue(_driverCover.IsElementExists(_driverCover.Driver.FindElement(By.CssSelector(".navbar-toggle.wallet"))), "Sign up was unseccessful ");
        }

        /// <summary>
        /// Searches for a web user
        /// </summary>
        /// <param name="email"></param>
        public void CheckBackOfficeUser(string email)
        {
            _commonActions.SignIn_in_admin_panel();
            bool isFound = _commonActions.FindWebUser_BackOffice(email);
            
            Assert.IsTrue(isFound, "Sorry but web user was not found on " + _driverCover.Driver.Url + " page. Please check if it were registered ");
        }
    }
}
