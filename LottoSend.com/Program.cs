using LottoSend.com.FrontEndObj;
using LottoSend.com.TestCases;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LottoSend.com
{
    class Program
    {
        static void Main(string[] args)
        {
            string date = "August 19, 2015 14:29";
            TimeSpan timeSpan = date.ParseTimeSpan();

            BuyingTicketsTests test = new BuyingTicketsTests();
            test.SetUp();
            test.BuyTicket();
            test.CleanUp();
         
        }
    }
}
