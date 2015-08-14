using LottoSend.com.FrontEndObj;
using LottoSend.com.TestCases;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LottoSend.com
{

    class Test
    {
        IWebDriver Driver = new ChromeDriver();

        public IWebElement InvoiceTable { get { return Driver.FindElement(By.Id("MainContent_gvInvoices")); } }
        public IList<IWebElement> InvoiceRows { get { return InvoiceTable.FindElements(By.CssSelector("tbody tr")); } }
        public IList<IWebElement> ACInvoiceRows { get; set; }

        public void test()
        {
            ACInvoiceRows = new IList<IWebElement>();
            foreach (IWebElement row in InvoiceRows)
            {
                if (row.Text.Contains("AC"))
                {
                    ACInvoiceRows.Add(row);
                }
            }
            Console.WriteLine(ACInvoiceRows.Count);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Test t = new Test();
            t.test();
         
        }
    }
}
