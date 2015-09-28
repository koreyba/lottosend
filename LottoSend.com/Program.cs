using LottoSend.com.FrontEndObj;
using OpenQA.Selenium.Chrome;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.FrontEndObj.GamePages;
using LottoSend.com.FrontEndObj.Login;
using OpenQA.Selenium;


namespace LottoSend.com
{
    class Program
    {
        static void Main(string[] args)
        {

            ChromeDriver _driver = new ChromeDriver();

            IWebElement el = _driver.FindElement(By.ClassName("f"));
            el.Click();
            
            DriverCover driver = new DriverCover(_driver);
            driver.NavigateToUrl(driver.BaseUrl + "en/web_users/sign_in");
            SignInPageOneObj signInOne = new SignInPageOneObj(_driver);

            signInOne.FillInFields(driver.Login, driver.Password);
            signInOne.ClickLogInButton();


            driver.NavigateToUrl("https://stg.lottobaba.com/en/plays/eurojackpot/");
            GroupGamePageObj groupGame = new GroupGamePageObj(_driver);



            MerchantsObj merchants = groupGame.ClickBuyTicketsButton();
            merchants.PayWithNeteller();
            
        }
    }
}
