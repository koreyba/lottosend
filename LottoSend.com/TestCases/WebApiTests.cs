﻿using System;
using LottoSend.com.CommonActions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.ClientOrderPricessing;
using TestFramework.FrontEndObj.Common;
using TestFramework.Helpers;

namespace LottoSend.com.TestCases
{
    /// <summary>
    /// Tests connected with Web API 
    /// </summary>
    /// <typeparam name="TWebDriver"></typeparam>
    [TestFixture(typeof(ChromeDriver), WayToPay.Offline)]
    //[TestFixture(typeof(ChromeDriver), WayToPay.Neteller)]
    class WebApiTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private TestFramework.CommonActions _commonActions;
        private TestsSharedCode _sharedCode;
        private WayToPay _merchant;
        private CartActions _cartActions;

        public WebApiTests(WayToPay merchant)
        {
            _merchant = merchant;
        }

        /// <summary>
        /// Buys a ticket via the sales panel and checks if TRID was added
        /// </summary>
        [Test]
        public void Trid_Is_Added_Buying_Ticket_Via_Sales_Panel()
        {
            _commonActions.SignIn_in_admin_panel();
            _commonActions.Sign_In_SalesPanel(_driverCover.Login);
            _commonActions.BuyRegularOneDrawTicket_SalesPanel("El Gordo");

            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders_processed");
            OrderProcessingObj orderProcessing = new OrderProcessingObj(_driver);

            StringAssert.Contains("Qa Denis", orderProcessing.Trid, "Sorry but TRID must have not been added. Please check it. ");
        }

        /// <summary>
        /// Adds TRID to the first transaction and check if it is not added to the second transaction
        /// </summary>
        [Test]
        public void Trid_Is_Not_Added_To_Second_Transaction()
        {
            _commonActions.Sign_Up_Front_PageOne();
            _commonActions.AddRegularTicketToCart_Front("en/play/megamillions/");

            string trid = RandomGenerator.GenerateRandomString(5);
            _driverCover.NavigateToUrl(_driverCover.Driver.Url + "?trid=" + trid);
            MerchantsObj merchants = new MerchantsObj(_driver);
            merchants.Pay(_merchant, false);

            _commonActions.AddRegularTicketToCart_Front("en/play/megamillions/");
            merchants.Pay(_merchant, false);

            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders_processed");
            OrderProcessingObj orderProcessing = new OrderProcessingObj(_driver);

            StringAssert.DoesNotContain(trid, orderProcessing.Trid, "Sorry but TRID must have not been added. Please check it. ");
        }

        /// <summary>
        /// Adds TRID to buying a ticket transaction and checks if TRID is displayed in the order processing
        /// </summary>
        [Test]
        public void Add_Trid_Buying_Ticket()
        {
            _commonActions.Sign_Up_Front_PageOne();
            _commonActions.AddRegularTicketToCart_Front("en/play/megamillions/");

            string trid = RandomGenerator.GenerateRandomString(5);
            _driverCover.NavigateToUrl(_driverCover.Driver.Url + "?trid=" + trid);
            MerchantsObj merchants = new MerchantsObj(_driver);
            merchants.Pay(_merchant, false);
            
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/orders_processed");
            OrderProcessingObj orderProcessing = new OrderProcessingObj(_driver);

            StringAssert.Contains(trid, orderProcessing.Trid, "Sorry but TRID must have not been added. Please check it. ");
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
                //Removes all tickets from the cart to make sure all other tests will work well
                try
                {
                    _commonActions.Sign_In_SalesPanel(_driverCover.Login);
                    _cartActions.DeleteAllTicketFromCart_Front();
                }
                catch (Exception)
                {
                    _sharedCode.CleanUp(ref _driver);
                }

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
            _cartActions = new CartActions(_driver);
        }
    }
}
