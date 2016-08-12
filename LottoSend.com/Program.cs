using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.ChargePanelPages;
using TestFramework.BackEndObj.RegularTicketsPages;

namespace LottoSend.com
{
    class Program
    {

        static void Main(string[] args)
        {

            IWebDriver _driver = new ChromeDriver();
            DriverCover _driverCover = new DriverCover(_driver);
            CommonActions common = new CommonActions(_driver);
            common.SignIn_in_admin_panel();


            _driverCover.NavigateToUrl("http://stgadmin.lottobaba.com/admin/packages");
            PackagesPageObj package = new PackagesPageObj(_driver);
            package.EditPackage("SuperEnalotto (1-90)", 8);

            for (int i = 0; i < 71; ++i)
            {
                ChargePanelObj panel = new ChargePanelObj(_driver);
                ChargeFormObj form = panel.ChargeTheLastPayment();

                form.MakeTransactionFailed();
                form.UpdateTransaction();

            }
        }
    }
}
