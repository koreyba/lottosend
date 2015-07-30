using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Net;
using System.Configuration;

namespace LottoSend.com
{
    /// <summary>
    /// Includes main methods to work with WebDriver
    /// It's a cover for main features
    /// </summary>
    public class DriverCover
    {
         public DriverCover(IWebDriver driver)
        {
            _driver = driver;
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            _driver.Manage().Window.Maximize();
            //_waitDriver = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Returns the main URL of the site (from app.config)
        /// </summary>
        public string BaseUrl
        {
            get { return ConfigurationManager.AppSettings["BaseUrl"].ToString(); }
        }

        /// <summary>
        /// Returns the main admin URL of the site (from app.config)
        /// </summary>
        public string BaseAdminUrl
        {
            get { return ConfigurationManager.AppSettings["BaseAdminUrl"].ToString(); }
        }

        /// <summary>
        /// User name (login) from app.config
        /// </summary>
        public string Login
        {
            get { return ConfigurationManager.AppSettings["Login"].ToString();}
        }

        /// <summary>
        /// Password of a user from app.config
        /// </summary>
        public string Password
        {
            get { return ConfigurationManager.AppSettings["Password"].ToString(); }
        }


        private readonly IWebDriver _driver;

        /// <summary>
        /// Driver
        /// </summary>
        public IWebDriver Driver
        {
            get { return _driver; }
        }
        
        /// <summary>
        /// Title of page
        /// </summary>
        public string Title
        {
            get { return _driver.Title; }
        }

        /// <summary>
        /// Check if a IWebElement exists
        /// </summary>
        /// <param name="element">element</param>
        /// <returns>true/false</returns>
        public bool IsElementExists(IWebElement element)
        {
            try
            {
                if (element.Displayed)
                    return true;
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        
        /// <summary>
        /// Waits for loading of a page
        /// <param name="secondsToWait">Time to wait (default 660 sec)</param>
        /// </summary>
        public void WaitForPageLoading(int secondsToWait = 600)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                while (sw.Elapsed.TotalSeconds < secondsToWait)
                {
                    var pageIsReady = (bool)((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState == 'complete'");
                    if (pageIsReady)
                        break;
                    Thread.Sleep(100);
                }
            }
            catch (Exception)
            {
                throw new TimeoutException("Page loading time out time has passed " + secondsToWait + " seconds");
            }
            finally
            {
                sw.Stop();
            }
        }

        /// <summary>
        /// Refreshes page
        /// </summary>
        public void RefreshPage()
        {
            Driver.Navigate().Refresh();
        }

        /// <summary>
        /// Waits for Ajax executing 
        /// </summary>
        /// <param name="secondsToWait">Time to wait (default 660 sec)</param>
        public void WaitAjax(int secondsToWait = 600)
        {
            int i = 0;
            int j = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                while (sw.Elapsed.TotalSeconds < secondsToWait)
                {
                    var ajaxIsComplete = !(bool)
                        ((IJavaScriptExecutor)Driver).ExecuteScript("return  $.active > 0"); ++i;
                    if (ajaxIsComplete)
                        break;                    
                    Thread.Sleep(100);
                    ++j;
                }
            }
            catch (Exception)
            {
                throw new TimeoutException("Ajax call time out time has passed " + secondsToWait + " seconds");
            }
            finally
            {
                sw.Stop();
            }
        }

        /// <summary>
        /// Scroll to element
        /// </summary>
        /// <param name="webElement">Element</param>
        /// <returns>Element</returns>
        public IWebElement ScrollToElement(IWebElement webElement)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView();", webElement);

            return webElement;
        }

        /// <summary>
        /// Execute JavaScript
        /// </summary>
        /// <param name="javaScript">JS code</param>
        /// <param name="args">Parameters</param>
        /// <returns></returns>
        public object ExecuteJavaScript(string javaScript, params object[] args)
        {
            var javaScriptExecutor = (IJavaScriptExecutor)Driver;

            return javaScriptExecutor.ExecuteScript(javaScript, args);
        }

        /// <summary>
        /// Move mouse to element
        /// </summary>
        /// <param name="target">Element to hover</param>
        public void MoveMouseToElement(IWebElement target)
        {
            Actions action = new Actions(Driver);
            action.MoveToElement(target).Perform();
        }

        /// <summary>
        /// Clicks "OK" in a pop up 
        /// </summary>
        public void AcceptAlert()
        {
            IAlert alert = GetAlert();
            alert.Accept();
            WaitForPageLoading();
        }

        /// <summary>
        /// Returns current alert. If there is no alert then returns null
        /// </summary>
        /// <returns>Alert</returns>
        public IAlert GetAlert()
        {
            try
            {
                IAlert alert = Driver.SwitchTo().Alert();
                return alert;
            }
            catch (NoAlertPresentException)
            {
                // no alert to dismiss, so return null
                return null;
            }
        }

        /// <summary>
        /// Drag one element to another
        /// </summary>
        /// <param name="source">What to drag</param>
        /// <param name="target">Where to drag to</param>
        public void DragAndDropElement(IWebElement source, IWebElement target)
        {
            Actions action = new Actions(Driver);
            action.DragAndDrop(source, target).Perform();
        }

        /// <summary>
        /// Navigates to URL and waits for loading
        /// </summary>
        /// <param name="url">URL</param>
        public void NavigateToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
            WaitForPageLoading(); WebClient client = new WebClient(); 
            WaitAjax();
        }
    }
}
