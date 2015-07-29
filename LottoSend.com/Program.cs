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
            CartTestCases logIn = new CartTestCases();
            logIn.SetUp();
            logIn.Delete_Item_From_Cart();
            logIn.CleanUp();
        }
    }
}
