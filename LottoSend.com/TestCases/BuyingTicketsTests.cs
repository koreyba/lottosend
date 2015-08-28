using LottoSend.com.BackEndObj;
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
    public class BuyingTicketsTests
    {
        private IWebDriver _driver;
        private DriverCover driver;

        /// <summary>
        /// Buy group game ticket using offline charge
        /// </summary>
        [Test]
        public void BuyGroupGameTicket()
        {
            //Log in     
            Log_In();

            driver.NavigateToUrl(driver.BaseUrl + "en/plays/eurojackpot/");

            //Pay for tickets
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);
            MerchantsObj merchants = groupGame.ClickBuyTicketsButton();
            merchants.PayWithOfflineCharge();

            //Go to admin panel
            Go_to_admin_panel(driver);

            //authorize payment in charge panel
            Authorize_the_first_payment();

            //approve payment
            Approve_offline_payment();

            //check transaction page
            Check_transaction_page();

            //Check draw record
            Check_draw_record(driver, "EuroJackpot");
        }

        /// <summary>
        /// Buy regular game ticket using offline charge. Checks transactions page and draw page.
        /// </summary>
        [Test]
        public void BuyRegularTicket()
        {
            //Log in     
            Log_In();

            driver.NavigateToUrl(driver.BaseUrl + "en/plays/eurojackpot/");

            //Select regular game tab
            RegularGamePageObj game = Select_regular_game_tab();

            //Pay for ticket
            MerchantsObj merchants = game.ClickBuyTicketsButton();
            merchants.PayWithOfflineCharge();

            //Go to admin panel
            Go_to_admin_panel(driver);

            //authorize payment in charge panel
            Authorize_the_first_payment();

            //approve payment
            Approve_offline_payment();

            //check transaction page
            Check_transaction_page();

            //Check draw record
            Check_draw_record(driver, "EuroJackpot");
        }

        private void Check_draw_record(DriverCover driver, string lotteryName)
        {
            driver.NavigateToUrl(driver.BaseAdminUrl + "admin/draws ");
            DrawsObj drawsPage = new DrawsObj(_driver);
            
            DrawObj draw = drawsPage.GoToDrawPage(lotteryName);

            bool correctEmail = draw.CheckEmail(driver.Login);
            if (!correctEmail)
            {
                throw new Exception("Sorry, the email in the first record is wrong, check if a record was added ");
            }

            bool correctTime = draw.CheckTime(3);
            if(!correctTime)
            {
                throw new Exception("Sorry, the time of the first record is not in set interval. Check if record was added ");
            }
            
        }

        private void Check_transaction_page()
        {
            driver.NavigateToUrl(driver.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);
            bool correctEmail = transaction.CheckEmail(driver.Login);
            if (!correctEmail)
            {
                throw new Exception("Sorry, the email in the first record is wrong, check if a record was added ");
            }

            bool correctMerchant = transaction.CheckMerchant("Offline");
            if (!correctMerchant)
            {
                throw new Exception("Sorry, the merchant in the first record is wrong, check if a record was added ");
            }

            bool correctTime = transaction.CheckTime(3);
            if (!correctTime)
            {
                throw new Exception("Sorry, the time of the first record is not in set interval. Check if record was added ");
            }
        }

        private void Approve_offline_payment()
        {
            driver.NavigateToUrl(driver.BaseAdminUrl + "admin/charge_panel_manager");

            ChargePanelObj panel = new ChargePanelObj(_driver);
            ChargeFormObj form = panel.ChargeTheLastPayment();

            form.MakeSuccessfulTransaction();
            form.UpdateTransaction();
        }

        private void Authorize_the_first_payment()
        {
            driver.NavigateToUrl(driver.BaseAdminUrl + "admin/orders_processed");

            OrderProcessingObj processing = new OrderProcessingObj(_driver);
            processing.AuthorizeTheLastPayment();
        }

        private void Go_to_admin_panel(DriverCover driver)
        {
            driver.NavigateToUrl(driver.BaseAdminUrl);

            LoginObj login = new LoginObj(_driver);
            login.LogIn("koreybadenis@gmail.com", "299242909");
        }

        private RegularGamePageObj Select_regular_game_tab()
        {
            RegularGamePageObj game = new RegularGamePageObj(_driver);
            game.ClickStandartGameButton();
            return game;
        }

        private void Log_In()
        {
            driver.NavigateToUrl(driver.BaseUrl + "en/web_users/sign_in");
            SignInPageOneObj signInOne = new SignInPageOneObj(_driver);

            signInOne.FillInFields(driver.Login, driver.Password);
            TopBarObj topBar = signInOne.ClickLogInButton();
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
