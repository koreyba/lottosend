﻿using System;
using System.Configuration;
using OpenQA.Selenium;
using TestFramework.BackEndObj;
using TestFramework.BackEndObj.ChargePanelPages;
using TestFramework.BackEndObj.ClientOrderPricessing;
using TestFramework.BackEndObj.DrawPages;
using TestFramework.BackEndObj.GroupGapePages;
using TestFramework.BackEndObj.SalesPanelPages;
using TestFramework.BackEndObj.WebUsersPages;
using TestFramework.FrontEndObj.Cart;
using TestFramework.FrontEndObj.Common;
using TestFramework.FrontEndObj.GamePages;
using TestFramework.FrontEndObj.Login;
using TestFramework.FrontEndObj.MyAccount;
using TestFramework.FrontEndObj.SignUp;
using CartObj = TestFramework.FrontEndObj.Cart.CartObj;


namespace TestFramework
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
        /// Switches on the combined payment page for a site
        /// </summary>
        public void SwitchOnCombinedPaymentPage()
        {
            SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/sites/1/edit");
            SiteEditingPageObj siteEditing = new SiteEditingPageObj(_driver);
            siteEditing.SwitchCombinedPageOn();
        }

        /// <summary>
        /// Switches on the combined payment page for a site
        /// </summary>
        public void SwitchOffCombinedPaymentPage()
        {
            SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/sites/1/edit");
            SiteEditingPageObj siteEditing = new SiteEditingPageObj(_driver);
            siteEditing.SwitchCombinedPageOff();
        }

        /// <summary>
        /// Goes to admin/sites page and clears cache of a selected site. Doesn't provide with admin login
        /// </summary>
        /// <param name="sitesName"></param>
        public void ClearCache(string sitesName)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/sites/");
            SitesObj siteEdit = new SitesObj(_driver);
            siteEdit.ClearCache(sitesName);
        }

        /// <summary>
        /// Changes a default deposit value for a website. Needs previous log in the admin panel
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public string Change_Amount_Of_Default_Deposit(string siteID)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/sites/" + siteID + "/edit");

            SiteEditingPageObj siteEditing = new SiteEditingPageObj(_driver);

            int[] amounts = new[] { 15, 30, 60, 90, 120, 150 };
            int defaultValue = amounts[RandomGenerator.GenerateNumber(0, 6)];
            siteEditing.SelectDefaultDepositAmount(defaultValue.ToString());

            return defaultValue.ToString();
        }

        /// <summary>
        /// Removes a specific bet (with sent number) from a specific draw
        /// </summary>
        /// <param name="lotteryName"></param>
        /// <param name="betNumber"></param>
        /// <param name="isBulk">Is bet bulk?</param>
        public void RemoveBetFromDraw_BackOffice(string lotteryName, int betNumber, bool isBulk)
        {
            SignIn_in_admin_panel();
            //navigate to the draw's page
            DrawObj draw = Find_The_Draw_Page(lotteryName);
            draw.DeleteBet(betNumber, isBulk);
        }

        /// <summary>
        /// Being in the sales panel method applies a coupon in the cart
        /// </summary>
        /// <param name="code"></param>
        public TestFramework.BackEndObj.SalesPanelPages.CartObj ApplyCouponInCart_SalesPanel(string code)
        {
            TestFramework.BackEndObj.SalesPanelPages.CartObj cart = new TestFramework.BackEndObj.SalesPanelPages.CartObj(_driver);
            cart.ApplyCoupon(code);

            return cart;
        }

        /// <summary>
        /// Being in the cart method proceeds to checkout and applies a coupon
        /// </summary>
        /// <param name="code"></param>
        public CheckoutObj ApplyCouponInCart_Web(string code)
        {
            CartObj cart = new CartSiteObj(_driver);
            cart.ClickProceedToCheckoutButton();
            CheckoutObj checkout = new CheckoutObj(_driver);
            checkout.ApplyCoupon(code);

            return checkout;
        }

        /// <summary>
        /// Buys a raffle ticket in the sales panel (approves payment)
        /// </summary>
        /// <param name="raffleName"></param>
        /// <returns></returns>
        public double BuyRaffleTicket_SalesPanel(string raffleName)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            AddRaffleTicketToCart_SalesPanel(raffleName);

            return PayForTicketsInCart_SalesPanel(WayToPay.Offline);
        }

        /// <summary>
        /// Adds a raffle ticket to the cart in the sales panel. Need previous sign in in the sales panel
        /// </summary>
        /// <param name="raffleName"></param>
        public void AddRaffleTicketToCart_SalesPanel(string raffleName)
        {
            MenuObj menu = new MenuObj(_driver);
            menu.GoToLotteryPage(raffleName);

            RafflePageObj rafflePage = new RafflePageObj(_driver);
            rafflePage.AddShareToTicket(1, 1);
            rafflePage.ClickAddToCartButton();
        }

        /// <summary>
        /// Buys a regular one draw ticket in the sales panel (approves payment). Need previous sign in in the sales panel
        /// </summary>
        /// <param name="lotteryName"></param>
        /// <returns></returns>
        public double BuyRegularOneDrawTicket_SalesPanel(string lotteryName)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            AddRegularOneDrawTicketToCart_SalesPanel(lotteryName);

            return PayForTicketsInCart_SalesPanel(WayToPay.Offline);
        }

        /// <summary>
        /// Adds a regular one draw tickte to the cart in the sales panel
        /// </summary>
        /// <param name="lotteryName"></param>
        public void AddRegularOneDrawTicketToCart_SalesPanel(string lotteryName)
        {
            MenuObj menu = new MenuObj(_driver);
            menu.GoToLotteryPage(lotteryName);
            GroupGameObj group = new GroupGameObj(_driver);
            group.SwitchToSingleTab();

            RegularGameObj game = new RegularGameObj(_driver);
            game.AddToCart();
        }

        /// <summary>
        /// Adds CC Details and deposits money (other amount) (approves payment). You need previously sign in in the sales panel
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="ifProcess"></param>
        /// <param name="isFailed"></param>
        public void DepositMoney_SalesPanel(double amount, bool ifProcess = true, bool isFailed = false)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");
            AddCCDetails_SalesPanel();

            MenuObj menu = new MenuObj(_driver);
            menu.GoToDeposit();

            DepositBoxObj box = new DepositBoxObj(_driver);
            box.DepositOtherAmoun(amount);

            if (ifProcess)
            {
                if (!isFailed)
                {
                    Approve_offline_payment();
                }
                else
                {
                    Fail_offline_payment();
                }
            }
        }

        /// <summary>
        /// Opens CC Details button and input CC details. Doesn't navigate to the sales panel
        /// </summary>
        public void AddCCDetails_SalesPanel()
        {
            TabsObj tabs = new TabsObj(_driver);
            tabs.GoToCcDetailsTab();
            CcDetailsObj form = new CcDetailsObj(_driver);
            form.InputCcDetails("VISA", "4580458045804580");
        }

        /// <summary>
        /// Pays for tickets in the cart (offline or internal balance). To use this method you must be on the sales panel page
        /// </summary>
        /// <param name="merchant"></param>
        /// <param name="ifProcess"></param>
        /// <param name="isFailed"></param>
        public double PayForTicketsInCart_SalesPanel(WayToPay merchant, bool ifProcess = true, bool isFailed = false)
        {
            TestFramework.BackEndObj.SalesPanelPages.CartObj cart = new TestFramework.BackEndObj.SalesPanelPages.CartObj(_driver);
            double totalPrice = cart.TotalPrice;

            if (merchant == WayToPay.Offline)
            {
                AddCCDetails_SalesPanel();

                cart.Charge();

                if (ifProcess)
                {
                    _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/charge_panel_manager");
                    ChargePanelObj chargePanel = new ChargePanelObj(_driver);
                    chargePanel.ChargeTheLastPayment();
                    ChargeFormObj chargeForm = new ChargeFormObj(_driver);

                    if (!isFailed)
                    {
                        chargeForm.MakeTransactionSucceed();
                    }
                    else
                    {
                        chargeForm.MakeTransactionFailed();
                    }

                    chargeForm.UpdateTransaction();
                }
            }

            if (merchant == WayToPay.InternalBalance)
            {
                cart.PayWithInternalBalance();
            }

            return totalPrice;
        }

        /// <summary>
        /// Adds a group lottery ticket to the cart
        /// </summary>
        /// <param name="lottery"></param>
        public void AddGroupBulkBuyTicketToCart_SalesPanel(string lottery)
        {
            //Add ticket to the cart
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");
            MenuObj menu = new MenuObj(_driver);
            menu.GoToLotteryPage(lottery);
            GroupGameObj group = new GroupGameObj(_driver);
            group.AddShareToTicket(1, 1);
            group.ClickAddToCartButton();
        }

        /// <summary>
        /// Creates a new group (for gorup tickets) in the backoffice 
        /// </summary>
        /// <param name="name"></param>
        public void CreateGroup(string name)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/groups/new");
            NewGroupCreationObj newGroup = new NewGroupCreationObj(_driver);
            newGroup.CreateNewGroup(name);
        }

        /// <summary>
        /// Removes a group in backoffice/groups
        /// </summary>
        /// <param name="group"></param>
        public void DeleteGroup(string group)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/groups");
            GroupsPageObj groupsPage = new GroupsPageObj(_driver);
            groupsPage.DeleteGroup(group);
        }

        /// <summary>
        /// Adds a raffle ticket to the cart on front
        /// </summary>
        public void AddRaffleTicketToCart_Front(string address)
        {
            _driverCover.NavigateToUrl(address);
            RafflesPageObj raffle = new RafflesPageObj(_driver);

            raffle.ClickBuyNowButton();
        }

        /// <summary>
        /// Adds a group ticket to the cart
        /// </summary>
        /// <param name="adress"></param>
        public void AddGroupTicketToCart_Front(string adress)
        {
            //Add ticket to the cart
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + adress);
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);
            groupGame.ClickBuyTicketsButton();
        }

        /// <summary>
        /// Removes all ticket from the cart. Needs previous login
        /// </summary>
        public void DeleteAllTicketFromCart_Front()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "carts/");
            CartObj cart = new CartSiteObj(_driver);
            cart.DeleteAllTickets();
        }

        /// <summary>
        /// Removes all ticket from the cart
        /// </summary>
        public void DeleteAllTicketFromCart_SalesPanel()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");
            TestFramework.BackEndObj.SalesPanelPages.CartObj cart = new TestFramework.BackEndObj.SalesPanelPages.CartObj(_driver);
            cart.DeleteAllTickets();
        }

        /// <summary>
        /// Adds a regular ticket to the cart
        /// </summary>
        /// <param name="address"></param>
        public void AddRegularTicketToCart_Front(string address)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + address);
            RegularGamePageObj game = new RegularGamePageObj(_driver);
            game.ClickStandartGameButton();

            game.ClickBuyTicketsButton();
        }

        /// <summary>
        /// Removes a group (of group tickets) in the back office
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
        public double BuyRaffleTicket_Front(WayToPay merchant)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            RafflesPageObj rafflePage = new RafflesPageObj(_driver);
            double totalPrice = rafflePage.TotalPrice;

            rafflePage.ClickBuyNowButton();//
            MerchantsObj merchants = new MerchantsObj(_driver);
            merchants.Pay(merchant);

            return totalPrice;
        }

        /// <summary>
        /// Buys a regular one-draw ticket
        /// </summary>
        /// <returns>Total price to pay</returns>
        public double BuyRegularOneDrawTicket_Front(WayToPay merchant, bool processOrder = true)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/play/powerball/");

            //Pay for tickets
            RegularGamePageObj regularGame = new RegularGamePageObj(_driver);

            //go to single tab
            regularGame.ClickStandartGameButton();

            //Select single draw
            regularGame.SelectOneTimeEntryGame();

            double totalPrice = regularGame.TotalPrice;

            MerchantsObj merchants = regularGame.ClickBuyTicketsButton();

            if (processOrder)
            {
                merchants.Pay(merchant);
            }
            else
            {
                merchants.Pay(merchant, false);
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
        public void DepositMoney_Front(double amount, WayToPay merchant, bool ifProcess = true, bool isFailed = false)
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
        public void DepositMoney_Mobile(double amount, WayToPay merchant, bool ifProcess = true, bool isFailed = false)
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
        public RegularGamePageObj SelectRegularGameTab_Front()
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
        public string Log_In_Front_PageOne(string email, string password)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "web-users/sign-in");
            SignInPageOneObj signInOne = new SignInPageOneObj(_driver);

            signInOne.FillInFields(email, password);
            signInOne.ClickLogInButton();

            return email;
        }

        /// <summary>
        /// Goes to the front-end and signs in
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public string Log_In_Front_PageTwo(string email, string password)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "web-users/sign-in2");
            SignInPageTwoObjcs signInOne = new SignInPageTwoObjcs(_driver);

            signInOne.FillInFields(email, password);
            signInOne.ClickLogInButton();

            return email;
        }

        /// <summary>
        /// Goes to the sales panel and signs in. Needs previous login in backoffice
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string Log_In_SalesPanel(string email)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            RegisterObj regForm = new RegisterObj(_driver);
            regForm.SignIn(email);

            return email;
        }

        /// <summary>
        /// Goes to the sales panel and sign up a new user. Needs previous login in backoffice
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string Sign_Up_SalesPanel(string email)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            RegisterObj regForm = new RegisterObj(_driver);
            return regForm.SignUp(email);
        }

        /// <summary>
        /// Navigates to the sales panel and signs in
        /// </summary>
        /// <param name="email"></param>
        public void Sign_In_SalesPanel(string email)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders");

            RegisterObj regForm = new RegisterObj(_driver);
            regForm.SignIn(email);
        }


        /// <summary>
        /// Goes to admin panel and authorizes
        /// </summary>
        public void SignIn_in_admin_panel()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl);
            if (_driverCover.Driver.FindElements(By.CssSelector("#current_user")).Count == 0)
            {
                LoginObj login = new LoginObj(_driver);
                login.LogIn(ConfigurationManager.AppSettings["AdminLogin"], ConfigurationManager.AppSettings["AdminPassword"]);
            }

            //else already signed in
        }

        /// <summary>
        /// Goes to admin panel and authorize
        /// </summary>
        public void SignIn_in_admin_panel(string email, string password)
        {
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl);
            if (_driverCover.Driver.FindElements(By.CssSelector("#current_user")).Count == 0)
            {
                LoginObj login = new LoginObj(_driver);
                login.LogIn(email, password);
            }
            else
            {
                throw new Exception("Sorry but a user is already signed in ");
            }
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
        /// Goes to charge panel and fails the last payment. Doesn't perform authorization in admin panel
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
        public string Sign_Up_Front_PageOne()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "web-users/sign-up");
            SignUpPageOneObj signUp = new SignUpPageOneObj(_driver);
            string email = signUp.FillInFieldsWeb();
            signUp.ClickSignUp();

            return email;
        }

        /// <summary>
        /// Registrate a new user on new signup page - sign_up 2
        /// <returns>User's email</returns>
        /// </summary>
        public string Sign_Up_Front_PageTwo()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "web-users/sign-up2");
            SignUpPageTwo signUp = new SignUpPageTwo(_driver);
            string email = signUp.FillInFieldsWeb();
            signUp.ClickSignUp();

            return email;
        }

        /// <summary>
        /// Registrate a new user
        /// <returns>User's email</returns>
        /// </summary>
        public string Sign_Up_In_Pop_Up_Front()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl);

            TopBarObj topBar = new TopBarObj(_driver);
            SignUpPopUpObj popUp = topBar.ClickSignUpButton();
            string email = popUp.FillInFieldsWeb();
            popUp.ClickSignUp();

            return email;
        }

        /// <summary>
        /// Registrate a new user on mobile site
        /// <returns>User's email</returns>
        /// </summary>
        public string Sign_Up_Mobile()
        {
            _driverCover.NavigateToUrl(_driverCover.BaseUrl + "web-users/sign-up");
            SignUpPageOneObj signUp = new SignUpPageOneObj(_driver);
            string email = signUp.FillInFieldsMobile();
            signUp.ClickSignUp();

            return email;
        }

    }
}
