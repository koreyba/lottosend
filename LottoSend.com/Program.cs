using LottoSend.com.FrontEndObj;
using LottoSend.com.TestCases;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoSend.com
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeDriver _driver = new ChromeDriver();
            DriverCover driver = new DriverCover(_driver);
            LogInTests logIn = new LogInTests();
            logIn.SetUp();
            driver.NavigateToUrl("https://stg.lottobaba.com/en/");
            driver.NavigateToUrl("https://stg.lottobaba.com/en/carts/");
            CartObj cart = new CartObj(_driver);
            bool flag = cart.IfTicketIsAdded("EuroJackpot");
            logIn.CleanUp();
        }
    }
}
