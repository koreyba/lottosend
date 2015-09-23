using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Support.Events;

namespace LottoSend.com
{
    /// <summary>
    /// Allows to listen all events in a WebDriver, writes them in console
    /// </summary>
    public class Debugger
    {
        private IWebDriver _driver;
        private EventFiringWebDriver _firingDriver;

        public EventFiringWebDriver Driver
        {
            get { return _firingDriver; }
        }

        public Debugger(IWebDriver driver)
        {
            _driver = driver;

            _firingDriver = new EventFiringWebDriver(_driver);
            _firingDriver.ExceptionThrown += new EventHandler<WebDriverExceptionEventArgs>(ExceptionThrown);
            _firingDriver.ElementClicked += new EventHandler<WebElementEventArgs>(ElementClicked);
            _firingDriver.ElementClicking += new EventHandler<WebElementEventArgs>(ElementClicking);
            _firingDriver.FindElementCompleted += new EventHandler<FindElementEventArgs>(FindElementCompleted);
            _firingDriver.FindingElement += new EventHandler<FindElementEventArgs>(FindingElement);
            _firingDriver.Navigated += new EventHandler<WebDriverNavigationEventArgs>(Navigated);
            _firingDriver.Navigating += new EventHandler<WebDriverNavigationEventArgs>(Navigating);
            _firingDriver.ScriptExecuted += new EventHandler<WebDriverScriptEventArgs>(ScriptExecuted);
            _firingDriver.ScriptExecuting += new EventHandler<WebDriverScriptEventArgs>(ScriptExecuting);
            _firingDriver.ElementValueChanged += new EventHandler<WebElementEventArgs>(ElementValueChanged);
            _firingDriver.ElementValueChanging += new EventHandler<WebElementEventArgs>(ElementValueChanging);
        }

        public static void ElementValueChanging(object sender, WebElementEventArgs e)
        {
            Console.WriteLine("The value of the next element will be changed now: " + e.Element);
        }

        public static void ElementValueChanged(object sender, WebElementEventArgs e)
        {
            Console.WriteLine("The value of the next element was changed: " + e.Element);
        }

        public static void ScriptExecuting(object sender, WebDriverScriptEventArgs e)
        {
            Console.WriteLine("The next script will be executed: " + e.Script);
        }

        public static void ScriptExecuted(object sender, WebDriverScriptEventArgs e)
        {
            Console.WriteLine("The next script was executed: " + e.Script);
        }

        public static void Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            Console.WriteLine("Will navigate to " + e.Url);
        }

        public static void Navigated(object sender, WebDriverNavigationEventArgs e)
        {
            Console.WriteLine("Navigated to " + e.Url);
        }

        public static void FindingElement(object sender, FindElementEventArgs e)
        {
            Console.WriteLine("Start searching for " + e.Element + " using " + e.FindMethod);
        }

        public static void FindElementCompleted(object sender, FindElementEventArgs e)
        {
            Console.WriteLine(e.Element + " was found using " + e.FindMethod);
        }

        public static void ElementClicking(object sender, WebElementEventArgs e)
        {
            Console.WriteLine("Will click on " + e.Element + "right now ");
        }

        public static void ElementClicked(object sender, WebElementEventArgs e)
        {
            Console.WriteLine(e.Element + " was clicked "); 
        }

        public static void ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            Console.WriteLine("The next exception was thrown: " + e.ThrownException.Message + " InnerException: " + e.ThrownException.InnerException);
        }
    }
}
