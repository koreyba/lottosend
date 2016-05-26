using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using LottoSend.com.BackEndObj.ChargePanelPages;
using LottoSend.com.BackEndObj.RegularTicketsPages;
using LottoSend.com.FrontEndObj;
using LottoSend.com.FrontEndObj.SignUp;
using LottoSend.com.TestCases;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace LottoSend.com
{
    class Program
    {

        static void Main(string[] args)
        {


            IWebDriver _driver = new ChromeDriver();
            DriverCover _driverCover = new DriverCover(_driver);
            CommonActions common = new CommonActions(_driver);
            common.SignIn_in_admin_panel();


            _driverCover.NavigateToUrl("http://stgadmin.lottobaba.com/admin/packages/new");
            NewPackagePageObj package = new NewPackagePageObj(_driver);
            package.CreatePackage("SuperEnalotto (1-90)", 8, true, true, true, true, "stg1");

            for (int i = 0; i < 71; ++i)
            {
                ChargePanelObj panel = new ChargePanelObj(_driver);
                ChargeFormObj form = panel.ChargeTheLastPayment();

                form.MakeTransactionFailed();
                form.UpdateTransaction();

            }
        }
    }
}
