using System;
using System.Diagnostics;
using NUnit.Framework;

namespace LottoSend.com.Helpers
{
    public class MessageConsoleCreator
    {
        /// <summary>
        /// Pushes a message in console that includes name of the current run test
        /// and that driver is disposed
        /// </summary>
        /// <returns></returns>
        public void DriverDisposed()
        {
            string testName = TestContext.CurrentContext.Test.FullName;
            Console.WriteLine("Current test: " + testName + " was run. Driver will be disposed now. ");
            Debug.WriteLine("Current test: " + testName + " was run. Driver will be disposed now. ");
        }

        public void TestWillRun()
        {
            string testName = TestContext.CurrentContext.Test.FullName;
            Console.WriteLine("Current test: " + testName + " started running. ");
            Debug.WriteLine("Current test: " + testName + " started running. ");
        }
    }
}
