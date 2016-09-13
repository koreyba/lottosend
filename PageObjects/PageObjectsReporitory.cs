using OpenQA.Selenium;
using TestFramework.BackEndObj.SalesPanelPages;

namespace TestFramework
{
    public static class PageObjectsReporitory
    {
        private static IWebDriver _driver;

        public static CartObj Cart
        {
            get { return new CartObj(_driver); }
        }

        public static RafflePageObj RafflePage
        {
            get { return new RafflePageObj(_driver);}
        }

        public static MenuObj Menu
        {
            get { return new MenuObj(_driver);}
        }

        public static GroupGameObj GroupGame
        {
            get { return new GroupGameObj(_driver); }
        }

        public static RegularGameObj RegularGame
        {
            get { return new RegularGameObj(_driver); }
        }

        public static void Init(IWebDriver driver)
        {
            _driver = driver;
        }
    }
}
