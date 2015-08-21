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

            BuyingTicketsTests test = new BuyingTicketsTests();
            test.SetUp();
            test.BuyGroupGameTicket();
            test.CleanUp();
         
        }
    }
}
