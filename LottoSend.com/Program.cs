using System;
using System.IO;
using LottoSend.com.BackEndObj.ChargePanelPages;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.TestCases;
using LottoSend.com.TestCases.Mobile.Group_ticktes;
using LottoSend.com.TestCases.Web.Group_ticktes;
using NUnit.Framework.Internal;
using OpenQA.Selenium.Chrome;
using NUnit.Engine;
using NUnit.Framework;
using OpenQA.Selenium;

namespace LottoSend.com
{
    class Program
    {

        static void Main(string[] args)
        {
            BuyGroupMultiDrawTicketTests<ChromeDriver> test = 
                new BuyGroupMultiDrawTicketTests<ChromeDriver>(WayToPay.Offline);
            test.Check_Amount_In_Transaction_Back();

            ChromeDriver _driver = new ChromeDriver();
            CommonActions _commonActions = new CommonActions(_driver);
            DriverCover _driverCover = new DriverCover(_driver);


            //    //If pay with internal balance we need to log in with different user
            //_commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);
        
            //_driverCover.NavigateToUrl(_driverCover.BaseUrl + "en/raffles/loteria-de-navidad/");

            //RafflesPageObj rafflePage = new RafflesPageObj(_driver);

            //rafflePage.ClickBuyNowButton();//
            //_driverCover.Driver.FindElement(By.CssSelector("input[id$='merchant_23'] + img.merchant")).Click();
            //OnlineMerchantsObj online = new OnlineMerchantsObj(_driver);
            //online.FailPayment();
            //StringAssert.Contains("failure", _driverCover.Driver.Url);
            //_driverCover.Driver.Dispose();

            //common.SignIn_in_admin_panel();
            //_driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/charge_panel_manager");

            //for (int i = 0; i < 52; ++i)
            //{
            //    ChargePanelObj panel = new ChargePanelObj(d);
            //    ChargeFormObj form = panel.ChargeTheLastPayment();

            //    form.MakeTransactionSucceed();
            //    form.UpdateTransaction();

        
        }
    }
}
