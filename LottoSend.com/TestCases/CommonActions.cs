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
    /// <summary>
    /// Performs a set of common actions. Actions is not safe so you might need to do previous steps to perform an action
    /// </summary>
    public class CommonActions
    {
        private IWebDriver _driver;
        private DriverCover driver;

        public CommonActions()
        {
            _driver = new ChromeDriver();
            driver = new DriverCover(_driver);
        }

        /// <summary>
        /// Goes to the draw page of a given lottery. Doesn't perform admin panel authorizing. 
        /// </summary>
        /// <param name="lotteryName"></param>
        /// <returns></returns>
        protected DrawObj Find_The_Draw_Page(string lotteryName)
        {
            driver.NavigateToUrl(driver.BaseAdminUrl + "admin/draws ");
            DrawsObj drawsPage = new DrawsObj(_driver);

            DrawObj draw = drawsPage.GoToDrawPage(lotteryName);
            return draw;
        }

        /// <summary>
        /// On front-end play page switch to regular game tab. You have to be on this page to makethis action safely
        /// </summary>
        /// <returns></returns>
        protected RegularGamePageObj Select_regular_game_tab()
        {
            RegularGamePageObj game = new RegularGamePageObj(_driver);
            game.ClickStandartGameButton();
            return game;
        }

        /// <summary>
        /// Goes to the front-end and signs in
        /// </summary>
        public void Log_In_Front()
        {
            driver.NavigateToUrl(driver.BaseUrl + "en/web_users/sign_in");
            SignInPageOneObj signInOne = new SignInPageOneObj(_driver);

            signInOne.FillInFields(driver.Login, driver.Password);
            TopBarObj topBar = signInOne.ClickLogInButton();
        }

        /// <summary>
        /// Goes to admin panel and authorize
        /// </summary>
        public void Authorize_in_admin_panel()
        {
            driver.NavigateToUrl(driver.BaseAdminUrl);

            LoginObj login = new LoginObj(_driver);
            login.LogIn("koreybadenis@gmail.com", "299242909");
        }

        /// <summary>
        /// Goes to client order processing and authorize the first payment. Doesn't perform authorization to admin panel
        /// </summary>
        protected void Authorize_the_first_payment()
        {
            driver.NavigateToUrl(driver.BaseAdminUrl + "admin/orders_processed");

            OrderProcessingObj processing = new OrderProcessingObj(_driver);
            processing.AuthorizeTheLastPayment();
        }

        /// <summary>
        /// Goes to charge panel and approves the last payment. Doesn't perform authorization to admin panel
        /// </summary>
        protected void Approve_offline_payment()
        {
            driver.NavigateToUrl(driver.BaseAdminUrl + "admin/charge_panel_manager");

            ChargePanelObj panel = new ChargePanelObj(_driver);
            ChargeFormObj form = panel.ChargeTheLastPayment();

            form.MakeSuccessfulTransaction();
            form.UpdateTransaction();
        }
    }
}
