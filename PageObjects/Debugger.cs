using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;

namespace TestFramework
{
    /// <summary>
    /// Allows to listen all events in a WebDriver, writes them in console
    /// </summary>
    public class Debugger
    {
        private EventFiringWebDriver _firingDriver;

        public EventFiringWebDriver Driver => _firingDriver;

        public Debugger(IWebDriver driver)
        {
            _firingDriver = new EventFiringWebDriver(driver);
            _firingDriver.ExceptionThrown += ExceptionThrown;
            _firingDriver.ElementClicked += ElementClicked;
            _firingDriver.ElementClicking += ElementClicking;
            _firingDriver.FindElementCompleted += FindElementCompleted;
            _firingDriver.FindingElement += FindingElement;
            _firingDriver.Navigated += Navigated;
            _firingDriver.Navigating += Navigating;
            _firingDriver.ScriptExecuted += ScriptExecuted;
            _firingDriver.ScriptExecuting += ScriptExecuting;
            _firingDriver.ElementValueChanged += ElementValueChanged;
            _firingDriver.ElementValueChanging += ElementValueChanging;
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
            //Console.WriteLine("The next script will be executed: " + e.Script);
        }

        public static void ScriptExecuted(object sender, WebDriverScriptEventArgs e)
        {
            //Console.WriteLine("The next script was executed: " + e.Script);
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
            Console.WriteLine("Start searching for element using " + e.FindMethod);
        }

        public static void FindElementCompleted(object sender, FindElementEventArgs e)
        {
            Console.WriteLine("Element was found using " + e.FindMethod);
        }

        public static void ElementClicking(object sender, WebElementEventArgs e)
        {
            //Console.WriteLine("Will click on " + e.Element + "right now ");
        }

        public static void ElementClicked(object sender, WebElementEventArgs e)
        {
            //Console.WriteLine(e.Element + " was clicked "); 
        }

        public static void ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            Console.WriteLine("The next exception was thrown: " + e.ThrownException.Message + " InnerException: " + e.ThrownException.InnerException);
        }
    }
}
