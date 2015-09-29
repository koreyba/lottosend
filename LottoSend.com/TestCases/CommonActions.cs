﻿using LottoSend.com.BackEndObj;
using LottoSend.com.BackEndObj.GroupGapePages;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.FrontEndObj.Login;
using LottoSend.com.FrontEndObj.MyAccount;
using LottoSend.com.FrontEndObj.SignUp;
using OpenQA.Selenium;


namespace LottoSend.com.TestCases
{
    /// <summary>
    /// Performs a set of common actions. Actions are not safe so you might need to do previous steps to perform an action
    /// </summary>
    public class CommonActions
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;

        public CommonActions(IWebDriver newDriver)
        {
            _driver = newDriver;
            _driverCover = new DriverCover(_driver);
        }

        /// <summary>
        /// Adds a raffle ticket to the cart
        /// </summary>
        public void AddRaffleTicketToCart()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/");
            RafflesPageObj raffle = new RafflesPageObj(_driver);

            raffle.ClickBuyNowButton();
        }

        /// <summary>
        /// Adds a group ticket to the cart
        /// </summary>
        /// <param name="adress"></param>
        public void AddGroupTicketToCart(string adress)
        {
            //Add ticket to the cart
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + adress);
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);
            groupGame.ClickAddToCartButton();
        }

        /// <summary>
        /// Removes all ticket from the cart
        /// </summary>
        public void DeleteAllTicketFromCart()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/carts/");
            CartObj cart = new CartObj(_driver);
            cart.DeleteAllTickets();
        }

        /// <summary>
        /// Adds a regular ticket to the cart
        /// </summary>
        /// <param name="address"></param>
        public void AddRegularTicketToCart(string address)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + address);
            RegularGamePageObj game = new RegularGamePageObj(_driver);
            game.ClickStandartGameButton();

            game.ClickAddToCartButton();
        }

        /// <summary>
        /// Removes a group (of group tickets)
        /// </summary>
        /// <param name="name"></param>
        public void RemoveGroup(string name)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/groups");
            GroupsPageObj groupsPage = new GroupsPageObj(_driver);

            groupsPage.DeleteGroup(name);
        }

        /// <summary>
        /// Search for web user in back office - WebUsers
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool FindWebUser_BackOffice(string email)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/web_users");
            WebUsersPageObj webUsers = new WebUsersPageObj(_driver);
            webUsers.FilterByEmail(email);
            bool isFound = webUsers.FindUserOnCurrentPage(email);

            return isFound;
        }

        /// <summary>
        /// Buys a raffle ticket 
        /// </summary>
        /// <returns>Total price to pay</returns>
        public double BuyRaffleTicket(WayToPay merchant)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);
            double totalPrice = rafflePage.TotalPrice;

            CartObj cart = rafflePage.ClickBuyNowButton();
            MerchantsObj merchants = cart.ClickProceedToCheckoutButton();

            if (merchant == WayToPay.Neteller)
            {
                merchants.PayWithNeteller();
            }

            if (merchant == WayToPay.Offline)
            {
                merchants.PayWithOfflineCharge();

                Authorize_in_admin_panel();

                Authorize_the_first_payment();

                Approve_offline_payment();
            }

            return totalPrice;
        }

        /// <summary>
        /// Buys a regular one-draw ticket
        /// </summary>
        /// <returns>Total price to pay</returns>
        public double BuyRegularOneDrawTicket(WayToPay merchant)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/plays/eurojackpot/");

            //Pay for tickets
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            //go to single tab
            regularGame.ClickStandartGameButton();

            //Select single draw
            regularGame.SelectOneTimeEntryGame();

            double totalPrice = regularGame.TotalPrice;

            MerchantsObj merchants = regularGame.ClickBuyTicketsButton();

            if (merchant == WayToPay.Neteller)
            {
                merchants.PayWithNeteller();
            }

            if (merchant == WayToPay.Offline)
            {
                merchants.PayWithOfflineCharge();

                Authorize_in_admin_panel();

                Authorize_the_first_payment();

                Approve_offline_payment();
            }

            return totalPrice;
        }

        /// <summary>
        /// Deposits exact amount of money to the previously signed in user's account
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="merchant">Merchant to pay</param>
        /// <param name="ifProcess">Tells if process the payment or leave it pendant</param>
        /// <param name="isFailed">To fail payment of not</param>
        public void DepositMoney(double amount, WayToPay merchant, bool ifProcess = true, bool isFailed = false)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/deposits/new/");

            DepositObj deposit = new DepositObj(_driver);
            deposit.DepositOtherAmount(amount, merchant, ifProcess, isFailed);
        }

        /// <summary>
        /// Deposits exact amount of money to the previously signed in user's account on mobile
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="merchant">Merchant to pay</param>
        /// <param name="ifProcess">Tells if process the payment or leave it pendant</param>
        /// <param name="isFailed">To fail payment of not</param>
        public void DepositMoneyMobile(double amount, WayToPay merchant, bool ifProcess = true, bool isFailed = false)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/account/deposits/new/");

            DepositMobileObj deposit = new DepositMobileObj(_driver);
            deposit.DepositOtherAmount(amount, merchant, ifProcess, isFailed);
        }

        /// <summary>
        /// Goes to the draw page of a given lottery. Doesn't perform admin panel authorizing. 
        /// </summary>
        /// <param name="lotteryName"></param>
        /// <returns></returns>
        public DrawObj Find_The_Draw_Page(string lotteryName)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/draws ");
            DrawsObj drawsPage = new DrawsObj(_driver);

            DrawObj draw = drawsPage.GoToDrawPage(lotteryName);
            return draw;
        }

        /// <summary>
        /// On front-end play page switch to regular game tab. You have to be on this page to makethis action safely
        /// </summary>
        /// <returns></returns>
        public RegularGamePageObj Select_regular_game_tab()
        {
            RegularGamePageObj game = new RegularGamePageObj(_driver);
            game.ClickStandartGameButton();
            return game;
        }

        /// <summary>
        /// Goes to the front-end and signs in
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public string Log_In_Front(string email, string password)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/web_users/sign_in");
            SignInPageOneObj signInOne = new SignInPageOneObj(_driver);

            signInOne.FillInFields(email, password);
            signInOne.ClickLogInButton();

            return email;
        }

        /// <summary>
        /// Goes to admin panel and authorize
        /// </summary>
        public void Authorize_in_admin_panel()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl);
            if (_driverCover.Driver.FindElements(By.ClassName("current_user")).Count == 0)
            {
                LoginObj login = new LoginObj(_driver);
                login.LogIn("koreybadenis@gmail.com", "299242909");
            }

            //else already signed in
        }

        /// <summary>
        /// Goes to client order processing and authorize the first payment. Doesn't perform authorization in admin panel
        /// </summary>
        public void Authorize_the_first_payment()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders_processed");

            OrderProcessingObj processing = new OrderProcessingObj(_driver);
            processing.AuthorizeTheLastPayment();
        }

        /// <summary>
        /// Goes to charge panel and approves the last payment. Doesn't perform authorization in admin panel
        /// </summary>
        public void Approve_offline_payment()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/charge_panel_manager");

            ChargePanelObj panel = new ChargePanelObj(_driver);
            ChargeFormObj form = panel.ChargeTheLastPayment();

            form.MakeTransactionSucceed();
            form.UpdateTransaction();
        }

        /// <summary>
        /// Goes to charge panel and fails the last paymetn. Doesn't perform authorization in admin panel
        /// </summary>
        public void Fail_offline_payment()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/charge_panel_manager");

            ChargePanelObj panel = new ChargePanelObj(_driver);
            ChargeFormObj form = panel.ChargeTheLastPayment();

            form.MakeTransactionFailed();
            form.UpdateTransaction();
        }

        /// <summary>
        /// Registrate a new user
        /// <returns>User's email</returns>
        /// </summary>
        public string Sign_Up()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/web_users/sign_up");
            SignUpPageOneObj signUp = new SignUpPageOneObj(_driver);
            string email = signUp.FillInFieldsWeb();
            signUp.ClickSignUp();

            return email;
        }

        /// <summary>
        /// Registrate a new user on mobile site
        /// <returns>User's email</returns>
        /// </summary>
        public string Sign_Up_Mobile()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/web_users/sign_up");
            SignUpPageOneObj signUp = new SignUpPageOneObj(_driver);
            string email = signUp.FillInFieldsMobile();
            signUp.ClickSignUp();

            return email;
        }

    }
}
