using OpenQA.Selenium.Chrome;


namespace LottoSend.com
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeDriver d = new ChromeDriver();
            DriverCover driver = new DriverCover(d);
            driver.TakeScreenshot();
        }
    }
}
