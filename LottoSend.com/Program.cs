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
            LogInTests logIn = new LogInTests();
            logIn.SetUp();
            logIn.Login_On_SignIn_Page_Two();
            logIn.CleanUp();
        }
    }
}
