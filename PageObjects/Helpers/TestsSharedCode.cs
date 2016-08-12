using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestFramework.Helpers
{
    /// <summary>
    /// Includes code shared between different classes of tests cases
    /// </summary>
    public class TestsSharedCode
    {
        public TestsSharedCode(IWebDriver driver)
        {
            _driver = driver;
            CommonActions = new CommonActions(_driver);
        }

        private IWebDriver _driver;
        public CommonActions CommonActions;

        /// <summary>
        /// If a test was failed or inconclusive then the user's cart will be cleaned up
        /// </summary>
        public void CleanCart(string email, string password)
        {
            SetUp();
            try
            {
                CommonActions.Log_In_Front_PageOne(email, password);
                //Removes all tickets from the cart to make sure all other tests will work well
                CommonActions.DeleteAllTicketFromCart_Front();
            }
            catch (Exception)
            {
                Console.WriteLine("CleanCart method was failed.");
            }
            finally
            {
                CleanUp(ref _driver);
            }          
        }

        /// <summary>
        /// Dispose WebDriver and pushes erros to console from OrderVerifications class
        /// </summary>
        public void CleanUp(ref IWebDriver driver)
        {
            MessageConsoleCreator message = new MessageConsoleCreator(); 
            message.DriverDisposed(); 
            driver.Dispose();
        }

        private void SetUp()
        {
            _driver = new ChromeDriver();
            CommonActions = new CommonActions(_driver);
        }
    }
}
