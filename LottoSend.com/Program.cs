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
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using Selenium;

namespace LottoSend.com
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver chrome = new FirefoxDriver();

            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities = DesiredCapabilities.Firefox();
            capabilities.SetCapability(CapabilityType.BrowserName, "firefox");
            capabilities.SetCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));

            chrome = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capabilities);

            
        }
    }
}
