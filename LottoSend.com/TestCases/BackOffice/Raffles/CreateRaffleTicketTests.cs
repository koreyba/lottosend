using LottoSend.com.BackEndObj.RafflePages;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoSend.com.TestCases.BackOffice.Raffles
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    public class CreateRaffleTicketTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private bool _setUpFailed = false;
         
        /// <summary>
        /// Creates a raffle ticket
        /// </summary>
        /// <param name="raffleName"></param>
        [TestCase("Lotería del Niño")]
        public void Create_Raffle_Ticket(string raffleName)
        {
            //TODO: add accertation
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/raffles_tickets/new");
            RaffleTicketCreatingPageObj raffleCreationPage = new RaffleTicketCreatingPageObj(_driver);

            raffleCreationPage.CreateTicket(raffleName, 30);
        }

        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
            {
                _driverCover.TakeScreenshot();
            }
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
        }
    }
}
