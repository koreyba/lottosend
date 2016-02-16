using LottoSend.com.BackEndObj.ChargePanelPages;
using LottoSend.com.FrontEndObj.Common;
using LottoSend.com.TestCases;
using OpenQA.Selenium.Chrome;


namespace LottoSend.com
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeDriver d = new ChromeDriver();
            CommonActions common = new CommonActions(d);
            DriverCover _driverCover = new DriverCover(d);
            common.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/charge_panel_manager");
            
            for (int i = 0; i < 52; ++i)
            {
                ChargePanelObj panel = new ChargePanelObj(d);
                ChargeFormObj form = panel.ChargeTheLastPayment();

                form.MakeTransactionSucceed();
                form.UpdateTransaction();
                
            }
        }
    }
}
