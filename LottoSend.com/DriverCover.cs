using System;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using NUnit.Framework;

namespace LottoSend.com
{
    /// <summary>
    /// Includes main methods to work with WebDriver
    /// It's a cover for main features
    /// </summary>
    public class DriverCover : Debugger
    {
        public DriverCover(IWebDriver driver) : base (driver)
        {
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
            Driver.Manage().Window.Maximize();
            //_waitDriver = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        // private Debugger debug;

        /// <summary>
        /// Returns the main URL of the site (from app.config)
        /// </summary>
        public string BaseUrl
        {
            get { return ConfigurationManager.AppSettings["BaseUrl"]; }
        }

        /// <summary>
        /// Returns the main admin URL of the site (from app.config)
        /// </summary>
        public string BaseAdminUrl
        {
            get { return ConfigurationManager.AppSettings["BaseAdminUrl"]; }
        }

        /// <summary>
        /// User name (login) from app.config
        /// </summary>
        public string Login
        {
            get { return ConfigurationManager.AppSettings["Login"]; }
        } 

        /// <summary>
        /// User name (login) from app.config
        /// </summary>
        public string LoginTwo
        {
            get { return ConfigurationManager.AppSettings["LoginTwo"]; }
        }

        /// <summary>
        /// User name (login) from app.config
        /// </summary>
        public string LoginThree
        {
            get { return ConfigurationManager.AppSettings["LoginThree"]; }
        }

        /// <summary>
        /// User name (login) from app.config
        /// </summary>
        public string LoginFour
        {
            get { return ConfigurationManager.AppSettings["LoginFour"]; }
        }

        /// <summary>
        /// User name (login) from app.config
        /// </summary>
        public string LoginFive
        {
            get { return ConfigurationManager.AppSettings["LoginFive"]; }
        }

        /// <summary>
        /// User name (login) from app.config
        /// </summary>
        public string LoginSix
        {
            get { return ConfigurationManager.AppSettings["LoginSix"]; }
        }

        /// <summary>
        /// Password of a user from app.config
        /// </summary>
        public string Password
        {
            get { return ConfigurationManager.AppSettings["Password"]; }
        }

        /// <summary>
        /// Title of page
        /// </summary>
        public string Title
        {
            get { return Driver.Title; }
        }

        /// <summary>
        /// Swithces on/off a specific checkbox by it's name from a list of checkboxes
        /// </summary>
        /// <param name="checkboxes">List should contain 'labels', not only inputs</param>
        /// <param name="name"></param>
        public void ClickCheckboxInList(IList<IWebElement> checkboxes, string name)
        {
            foreach (var el in checkboxes)
            {
                if (el.Text.Equals(name))
                {
                    el.FindElement(By.XPath(".//input")).Click();
                }
            }
        }

        /// <summary>
        /// Checks if a checkbox is checked by loking of the value of the "checked" attribute
        /// </summary>
        /// <param name="checkbox"></param>
        /// <returns></returns>
        public bool IfCheckBoxIsChecked(IWebElement checkbox)
        {
            if (checkbox.GetAttribute("checked") != null && checkbox.GetAttribute("checked").Equals("true"))
            {
                return true;
            }
            return false;
        }

        public void ScrollTo(int xPosition = 0, int yPosition = 0)
        {
            var js = String.Format("window.scrollTo({0}, {1})", xPosition, yPosition);
            Driver.ExecuteScript(js);
        }

        public IWebElement ScrollToView(By selector)
        {
            var element = Driver.FindElement(selector);
            ScrollToView(element);
            return element;
        }

        /// <summary>
        /// Scrolls to an element on the page
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public IWebElement ScrollToView(IWebElement element)
        {
            if (element.Location.Y > 200)
            {
                ScrollTo(0, element.Location.Y - 100); // Make sure element is in the view but below the top navigation pane
            }
            return element;
        }

        /// <summary>
        /// Choose an element in IWebElement 'select'. Has 3 ways to select element (value, index, text)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="select"></param>
        /// <param name="by"></param>
        public void ChooseElementInSelect(string value, IWebElement select, SelectBy by)
        {
            SelectElement selector = new SelectElement(select);

            if (by == SelectBy.Text)
            {
                selector.SelectByText(value);
            }

            if (by == SelectBy.Index)
            {
                selector.SelectByIndex(Convert.ToInt32(value));
            }

            if (by == SelectBy.Value)
            {
                selector.SelectByValue(value);
            }
        }

        /// <summary>
        /// Returns options of a SELECT item
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public IList<IWebElement> GetListOfOptions(IWebElement select)
        {
            SelectElement selector = new SelectElement(select);

            return selector.Options;
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
        /// <param name="secondsToWait">Time to wait (default 600 sec)</param>
        /// </summary>
        public void WaitForPageLoading(int secondsToWait = 600)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                while (sw.Elapsed.TotalSeconds < secondsToWait)
                {
                    var pageIsReady = (bool)((IJavaScriptExecutor)Driver).ExecuteScript("return document.readyState == 'complete'");
                    if (pageIsReady)
                        break;
                    Thread.Sleep(100);
                }
            }
            catch (Exception)
            {
               // Driver.Dispose();
                throw new TimeoutException("Ajax call time out time has passed " + secondsToWait + " seconds" + "\n"
                    + "Current URL is: " + Driver.Url);
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
            WaitForPageLoading();
            WaitAjax();
        }

        /// <summary>
        /// Waits for Ajax executing 
        /// </summary>
        /// <param name="secondsToWait">Time to wait (default 320 sec)</param>
        /// <returns>If Ajax was executing</returns>
        public bool WaitAjax(int secondsToWait = 320)
        {
            bool ifExecuted = false;

            //WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(secondsToWait));
            //wait.Until((driver) => (bool)((IJavaScriptExecutor) Driver).ExecuteScript("return  $.active > 0"));
            //return true;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                while (sw.Elapsed.Seconds < secondsToWait)
                {
                    var ajaxIsComplete = !(bool)
                        ((IJavaScriptExecutor)Driver).ExecuteScript("return  $.active > 0");
                    if (ajaxIsComplete)
                    {
                        break;
                    }
                    ifExecuted = true;
                    Thread.Sleep(100);
                }

                return ifExecuted;
            }
            catch (Exception)
            {
                //Driver.Dispose();
                throw new TimeoutException("Ajax call time out time has passed " + secondsToWait + " seconds" + "\n" 
                    + "Current URL is: " + Driver.Url);
            }
            finally
            {
                sw.Stop();
            }
        }

        /// <summary>
        /// Opens a new tab in the browser and switches to it
        /// </summary>
        public void OpenNewTab()
        {
            //IWebElement body = Driver.FindElement(By.XPath(".//body"));
            //body.SendKeys(Keys.Control + "t");
            Driver.ExecuteScript("window.open(' ','_blank');");
            SwitchToTab(2);
        }

        /// <summary>
        /// Swithes to a tab with specific number (starts from 1)
        /// </summary>
        /// <param name="number"></param>
        public void SwitchToTab(int number)
        {
            if (number == 0)
            {
                throw new Exception("The number of the tab must start from 1 ");
            }

            Driver.SwitchTo().Window(Driver.WindowHandles[number - 1]);
        }

        /// <summary>
        /// Waits for jQuery executing 
        /// </summary>
        /// <param name="secondsToWait">Time to wait (default 300 sec)</param>
        /// <returns>If jQuery was executing</returns>
        public bool WaitjQuery(int secondsToWait = 300)
        {
            bool ifExecuted = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                while (sw.Elapsed.Seconds < secondsToWait)
                {
                    var ajaxIsComplete =
                        ((bool) ((IJavaScriptExecutor) Driver).ExecuteScript("return jQuery.active == 0"));
                
                    if (ajaxIsComplete)
                    {
                        break;
                    }
                    ifExecuted = true; 
                    Thread.Sleep(100);
                }

                return ifExecuted;
            }
            catch (Exception)
            {
               // Driver.Dispose();
                throw new TimeoutException("Ajax call time out time has passed " + secondsToWait + " seconds" + "\n"
                    + "Current URL is: " + Driver.Url);
            }
            finally
            {
                sw.Stop();
            }
        }

        /// <summary>
        /// Wait till element is clickable
        /// </summary>
        /// <param name="element"></param>
        /// <param name="timeOutInSeconds"></param>
        /// <returns></returns>
        public IWebElement WaitForElement(IWebElement element, int timeOutInSeconds)
        {
            try
            {
                bool a = WaitAjax();
                bool j = WaitjQuery();

                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0)); //nullify implicitlyWait() 

                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOutInSeconds));
                wait.Until(ExpectedConditions.ElementToBeClickable(element));

                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10)); //reset implicitlyWait

                return element; //return the element	
            }
            catch (Exception e)
            {
                //Driver.Dispose();
                throw new TimeoutException("Ajax call time out time has passed " + timeOutInSeconds + " seconds" + "\n"
                    + "Current URL is: " + Driver.Url);
            }
        }

        /// <summary>
        /// Wait till element is clickable
        /// </summary>
        /// <param name="how">How to find element (locator)</param>
        /// <param name="timeOutInSeconds"></param>
        /// <returns></returns>
        public IWebElement WaitForElement(By how, int timeOutInSeconds)
        {
            try
            {
                bool a = WaitAjax();
                bool j = WaitjQuery();

                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0)); //nullify implicitlyWait() 

                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOutInSeconds));
                IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(how));

                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10)); //reset implicitlyWait

                return element; //return the element	
            }
            catch (Exception e)
            {
                //Driver.Dispose();
                throw new TimeoutException("Ajax call time out time has passed " + timeOutInSeconds + " seconds" + "\n"
                    + "Current URL is: " + Driver.Url); 
            }
        }

        /// <summary>
        /// Scrolls up
        /// </summary>
        /// <returns>Element</returns>
        public void ScrollUp()
        {
            //Actions action = new Actions(Driver);
            //action.MoveToElement(webElement).Build().Perform();
            //((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView();", webElement);
            ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollBy(" + 0 + "," + -10000000 + ");");


            // return webElement;
        }

        /// <summary>
        /// Scrolls to the bottom of the page
        /// </summary>
        /// <returns>Element</returns>
        public void ScrollDown()
        {
            //Actions action = new Actions(Driver);
            //action.MoveToElement(webElement).Build().Perform();
            //((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView();", webElement);
            ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollBy(" + 0 + "," + 10000000 + ");");


           // return webElement;
        }

        /// <summary>
        /// Clicks Ctr + "+" button in order to zoom in
        /// </summary>
        public void ZoomIn()
        {
             new Actions(Driver)
                .SendKeys(Keys.Control).SendKeys(Keys.Add)
                .Perform();
        }

        /// <summary>
        /// Clicks Ctr + "-" button in order to zoom out
        /// </summary>
        public void ZoomOut()
        {
            new Actions(Driver)
               .SendKeys(Keys.Control).SendKeys(Keys.Subtract)
               .Perform();
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
            WaitForPageLoading();
            WaitAjax();
            WaitjQuery();
        }

        /// <summary>
        /// Returns the first visible element from list of elements 
        /// </summary>
        /// <returns>Element/null</returns>
        public IWebElement GetFirstVisibleElementFromList(IList<IWebElement> list)
        {
            foreach (IWebElement button in list)
            {
                if (button.Displayed)
                    return button;
            }

            return null;
        }

        /// <summary>
        /// Returns the first visible element from list of elements that are found by the same cssSelector.
        /// </summary>
        /// <returns>Element/null</returns>
        public IWebElement GetFirstVisibleElementFromList(By selector)
        {
            IList<IWebElement> buttons = Driver.FindElements(selector);
            foreach (IWebElement button in buttons)
            {
                if (button.Displayed)
                    return button;
            }

            return null;
        }

        /// <summary>
        /// Takes a screenshot and puts it in C:\Screenshots\ folder. The folder must be created manually
        /// </summary>
        public void TakeScreenshot()
        {
            string testName = TestContext.CurrentContext.Test.FullName;
            Console.WriteLine("Current test: " + testName + " was run. Driver will be disposed now.  Screenshot will be made. ");
           // Debug.WriteLine("Current test: " + testName + " was run. Driver will be disposed now. Screenshot will be made. ");

            try
            {
                Screenshot ss = ((ITakesScreenshot)Driver).GetScreenshot();
                var filePath = @"C:\Screenshots\" + testName.Replace("<", "(").Replace(">", ")").Replace("\"", "'") + DateTime.Now.DayOfWeek + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + "_Time-" + DateTime.Now.Hour + "." + DateTime.Now.Minute + "_" + RandomGenerator.GenerateRandomString(10) + ".jpg";
                ss.SaveAsFile(filePath, ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Console.WriteLine("Problems with saving a screenshot. Error message: " + e.Message + " ");
                throw;
            }

            try
            {
                using (Bitmap bmpScreenCapture = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                    System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bmpScreenCapture))
                    {
                        g.CopyFromScreen(System.Windows.Forms.Screen.PrimaryScreen.Bounds.X,
                            System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y,
                            0, 0,
                            bmpScreenCapture.Size,
                            CopyPixelOperation.SourceCopy);
                    }
                    var filePath = @"C:\Screenshots\" + testName.Replace("<", "(").Replace(">", ")").Replace("\"", "'") +
                                   DateTime.Now.DayOfWeek + DateTime.Now.Day + "." + DateTime.Now.Month + "." +
                                   DateTime.Now.Year + "_Time-" + DateTime.Now.Hour + "." + DateTime.Now.Minute + "_" +
                                   RandomGenerator.GenerateRandomString(10) + ".jpg";
                    bmpScreenCapture.Save(filePath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Problems with saving a screenshot. Error message: " + e.Message + " ");
                throw;
            }
        }
    }
}
