using System;
using LottoSend.com.FrontEndObj;
using OpenQA.Selenium.Chrome;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.FrontEndObj.Login;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.Extensions;


namespace LottoSend.com
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeDriver d = new ChromeDriver();
            DriverCover driver = new DriverCover(d);
            driver.TakeScreenshot();
        }
    }
}
