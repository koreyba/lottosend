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
            DriverCover cover = new DriverCover(_driver);
            cover.NavigateToUrl("https://stg.lottobaba.com/en/");
            BottomMenuObj bottom = new BottomMenuObj(_driver);
            bottom.VisitPlayPages();
            _driver.Dispose();
            int a;
        }
    }
}
