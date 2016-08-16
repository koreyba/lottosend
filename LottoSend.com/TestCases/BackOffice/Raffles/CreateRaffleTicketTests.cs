using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.RafflePages;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases.BackOffice.Raffles
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    public class CreateRaffleTicketTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private TestFramework.CommonActions _commonActions;
        private TestsSharedCode _sharedCode;

        /// <summary>
        /// Creates a raffle ticket
        /// </summary>
        [Test]
        [Category("Parallel")]
        [Category("Critical")]
        public void Create_Raffle_Lottery()
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/raffles");
            RaffleDashboardPageObj dashboard = new RaffleDashboardPageObj(_driver);
            int numberOfRaffles = dashboard.GetListOfOptions(dashboard.RaffleSelector).Count;

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/raffles/new");
            RaffleLotteryCreationPage raffleCreation = new RaffleLotteryCreationPage(_driver);
            
            string raffleName = "raffle_" + RandomGenerator.GenerateRandomString(6);
            raffleCreation.CreateNewRaffleLottery(raffleName);
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/raffles");

            Assert.AreEqual(numberOfRaffles + 1, dashboard.GetListOfOptions(dashboard.RaffleSelector).Count, "Sorry but the raffle must have been not created. Check it. Curren page is: " + _driver.Url + " ");
        }

        /// <summary>
        /// Creates a raffle ticket
        /// </summary>
        /// <param name="raffleName"></param>
        [TestCase("Loteria de Navidad")]
        [Category("Parallel")]
        [Category("Critical")]
        public void Create_Raffle_Ticket(string raffleName)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "/admin/raffles");
            RaffleDashboardPageObj dashboard = new RaffleDashboardPageObj(_driver);
            dashboard.SelectRaffle(raffleName); 
            int numberOfTickets = dashboard.NumberOfTickets;

            RaffleTicketCreationPageObj raffleCreationPage = dashboard.ClickAddNewTicketButton();
            dashboard = raffleCreationPage.CreateTicket(raffleName, 30);

            dashboard.SelectRaffle(raffleName);

            Assert.AreEqual(numberOfTickets + 1, dashboard.NumberOfTickets, "Sorry but the number of tickets is wrong. Ticket must have been not created. ");
        }

        [TearDown]
        public void CleanUp()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    _driverCover.TakeScreenshot();
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                _sharedCode.CleanUp(ref _driver);
            }
        }

        [SetUp]
        public void SetUp()
        {
            MessageConsoleCreator message = new MessageConsoleCreator();
            message.TestWillRun();
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new TestFramework.CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
