using System;
using LottoSend.com.FrontEndObj;
using OpenQA.Selenium.Chrome;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.FrontEndObj.Login;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;


namespace LottoSend.com
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeDriver _driver = new ChromeDriver();
            
            DriverCover driver = new DriverCover(_driver);
            //driver.NavigateToUrl(driver.BaseUrl + "en/web_users/sign_in");
            IWebElement el = _driver.FindElement(By.CssSelector("#cart > div.order_item"));


            driver.NavigateToUrl("https://stg.lottobaba.com/en/plays/eurojackpot/");
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);



            MerchantsObj merchants = groupGame.ClickBuyTicketsButton();
            merchants.PayWithNeteller();
            
        }
    }
}
