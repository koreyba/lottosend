using LottoSend.com.FrontEndObj;
using LottoSend.com.TestCases;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LottoSend.com
{
    class Program
    {
        static void Main(string[] args)
        {

            BuyingTicketsTests logIn = new BuyingTicketsTests();
            logIn.SetUp();
            logIn.BuyTicket();
            logIn.CleanUp();
        }
    }
}
