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
            int a = 0;
            ChromeDriver _driver = new ChromeDriver();
            CommonActions _commonActions = new CommonActions(_driver);
            DriverCover _driverCover = new DriverCover(_driver);


                //If pay with internal balance we need to log in with different user
            _commonActions.Log_In_Front(_driverCover.Login, _driverCover.Password);

            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/charge_panel_manager");

            for (int i = 0; i < 52; ++i)
            {
                ChargePanelObj panel = new ChargePanelObj(_driver);
                ChargeFormObj form = panel.ChargeTheLastPayment();

                form.MakeTransactionSucceed();
                form.UpdateTransaction();

            }
        }
    }
}
