using LottoSend.com.BackEndObj;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.FrontEndObj.MyAccount;
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
        private StringBuilder _errors;

        /// <summary>
        /// Buy group game ticket using offline charge
        /// </summary>
        [TestCase("Bulk Buy")]
        [TestCase("Single")]
        public void BuyGroupGameTicket(string type)
        {
            //Log in     
            Log_In();

            driver.NavigateToUrl(driver.BaseUrl + "en/plays/eurojackpot/");

            //Pay for tickets
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);

            double totalPrice = groupGame.TotalPrice;
            int numberOfDraws = groupGame.NumberOfDraws;

            //Select single draw
            if (type.Equals("Single"))
            {
                groupGame.SelectOneTimeEntryGame();
            }

            MerchantsObj merchants = groupGame.ClickBuyTicketsButton();
            merchants.PayWithOfflineCharge();

            //Go to admin panel
            Go_to_admin_panel(driver);

            //authorize payment in charge panel
            Authorize_the_first_payment();

            //approve payment
            Approve_offline_payment();

            //check transaction page
            //Check_transaction_page();

            //Check draw record
            Check_draw_record(driver, "EuroJackpot", type, totalPrice, numberOfDraws);

            //Check transaction history page on the front-end
            Check_transaction_on_front();
        }

        /// <summary>
        /// Buy regular game ticket using offline charge. Checks transactions page and draw page.
        /// </summary>
        [TestCase("Bulk Buy")]
        [TestCase("Single")]
        public void BuyRegularTicket(string type)
        {
            //Log in     
            Log_In();

            driver.NavigateToUrl(driver.BaseUrl + "en/plays/eurojackpot/");

            //Select regular game tab
            RegularGamePageObj game = Select_regular_game_tab();

            //Select single draw
            if (type.Equals("Single"))
            {
                game.SelectOneTimeEntryGame();
            }

            double totalPrice = game.TotalPrice;
            int numberOfDraws = game.NumberOfDraws;

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
            Check_draw_record(driver, "EuroJackpot", type, totalPrice, numberOfDraws);

            //Check transaction history page on the front-end
            Check_transaction_on_front();
        }

        private void Check_transaction_on_front()
        {
            driver.NavigateToUrl(driver.BaseUrl + "en/account/balance/");
            TransactionObj myTransactions = new TransactionObj(_driver);
            if (!myTransactions.SecondRecordLottery.Equals("EuroJackpot"))
            {
                _errors.Append("The second record has different lottery name, please check it, page: " + driver.Driver.Url + " ");
               // throw new Exception("The second record has different lottery name, please check ");
            }

            if (!myTransactions.FirstRecordLottery.Equals(""))
            {
                _errors.Append("The first record is supossed to have no lottery name, please check it, page: " + driver.Driver.Url + " ");
                //throw new Exception("The first record is supossed to have no lottery name, please check ");
            }


            //make month to be in right forman
            string digit = DateTime.Now.Month.ToString();
            string month = "";
            if (digit.Length == 1)
                month = "0" + digit;
            else
                month = digit;

            //make day to be in right forman
            digit = DateTime.Now.Day.ToString();
            string day = "";
            if (digit.Length == 1)
                day = "0" + digit;
            else
                day = digit;

            string date = day + "/" + month + "/" + DateTime.Now.Year;
            if (!myTransactions.FirstRecordDate.Equals(date))
            {
                _errors.Append("The first record has not current date, please check it, page: " + driver.Driver.Url + " ");
                //throw new Exception("The first record has not current date, please check ");
            }

            if (!myTransactions.SecondRecordDate.Equals(date))
            {
                _errors.Append("The second record has not current date, please check it, page: " + driver.Driver.Url + " ");
                //throw new Exception("The second record has not current date, please check ");
            }
        }

        private void Check_draw_record(DriverCover driver, string lotteryName, string type, double price, int numberOfDraws)
        {
            driver.NavigateToUrl(driver.BaseAdminUrl + "admin/draws ");
            DrawsObj drawsPage = new DrawsObj(_driver);
            
            DrawObj draw = drawsPage.GoToDrawPage(lotteryName);

            bool correctEmail = draw.CheckEmail(driver.Login);
            if (!correctEmail)
            {
                _errors.Append("Sorry, the email in the first record is wrong, check if a record was added, page: " + driver.Driver.Url + " ");
                //throw new Exception("Sorry, the email in the first record is wrong, check if a record was added ");
            }

            bool correctTime = draw.CheckTime(3);
            if(!correctTime)
            {
                _errors.Append("Sorry, the time of the first record is not in set interval. Check if record was added , page: " + driver.Driver.Url + " ");
               // throw new Exception("Sorry, the time of the first record is not in set interval. Check if record was added ");
            }
            
            if(type.Equals("Bulk Buy"))
            {
                if (!draw.Type.Equals("Bulk buy"))
                {
                    _errors.Append("Sorry, the type of the bet is expected to be Bulk buy but was " + draw.Type + ", page: " + driver.Driver.Url + " ");
                    //throw new Exception("Sorry, the type of the bet is expected to be Bulk buy but was " + draw.Type);
                }

                //If it was bulk buy of 2 draws then price devided by 2
                if (price / numberOfDraws != draw.BetAmount)
                {
                    _errors.Append("Sorry, the price for the bet is  " + draw.BetAmount + " but " + price / numberOfDraws + " was expected, page: " + driver.Driver.Url + " ");
                   // throw new Exception("Sorry, the price for the bet is  " + draw.BetAmount + " but " + price / numberOfDraws + " was expected ");
                }
            }

            if (type.Equals("Single"))
            {
                if (!draw.Type.Equals("Single"))
                {
                    _errors.Append("Sorry, the type of the bet is expected to be Single but was " + draw.Type + ", page: " + driver.Driver.Url + " ");
                    //throw new Exception("Sorry, the type of the bet is expected to be Single but was " + draw.Type);
                }
            }
        }

        private void Check_transaction_page()
        {
            driver.NavigateToUrl(driver.BaseAdminUrl + "admin/transactions");
            TransactionsObj transaction = new TransactionsObj(_driver);
            bool correctEmail = transaction.CheckEmail(driver.Login);
            if (!correctEmail)
            {
                _errors.Append("Sorry, the email in the first record is wrong, check if a record was added, page: " + driver.Driver.Url + " ");
                //throw new Exception("Sorry, the email in the first record is wrong, check if a record was added ");
            }

            bool correctMerchant = transaction.CheckMerchant("Offline");
            if (!correctMerchant)
            {
                _errors.Append("Sorry, the merchant in the first record is wrong, check if a record was added, page: " + driver.Driver.Url + " ");
               // throw new Exception("Sorry, the merchant in the first record is wrong, check if a record was added ");
            }

            bool correctTime = transaction.CheckTime(3);
            if (!correctTime)
            {
                _errors.Append("Sorry, the time of the first record is not in set interval. Check if record was added, page: " + driver.Driver.Url + " ");
                //throw new Exception("Sorry, the time of the first record is not in set interval. Check if record was added ");
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
            if(_errors.Length > 0)
            {
                Assert.Fail(_errors.ToString());
            }
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            driver = new DriverCover(_driver);
            _errors = new StringBuilder();
        }
    }
}
