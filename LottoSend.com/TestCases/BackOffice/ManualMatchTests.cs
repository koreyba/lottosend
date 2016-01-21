using System;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using LottoSend.com.BackEndObj;
using LottoSend.com.Verifications;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace LottoSend.com.TestCases.BackOffice
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    class ManualMatchTests<TWebDriver> where TWebDriver : IWebDriver, new() 
    {
        private IWebDriver _driver;
        private DriverCover _driverCover;
        private CommonActions _commonActions;
        private BackOfficeVerifications _backOfficeVerifications;

        /// <summary>
        /// Adds a new scanned image for a group ticket
        /// </summary>
        /// <param name="lottery"></param>
        /// <param name="group"></param>
        [TestCase("El Gordo", "Winning Guarantee")]
        [TestCase("Mega Millions", "Winning Guarantee")]
        [TestCase("SuperLotto Plus", "Winning Guarantee")]
        [TestCase("Powerball", "Winning Guarantee")]
        [TestCase("EuroMillions", "Winning Guarantee")]
        [TestCase("Euromiliony", "Random Selection")]
        [TestCase("EuroJackpot", "Random Selection")]
        [TestCase("EuroMillions", "Random Selection")]
        [TestCase("EuroJackpot", "Random Selection")]
        [TestCase("SuperLotto Plus", "Random Selection")]
        [TestCase("Powerball", "Random Selection")]
        [TestCase("Mega Millions", "Random Selection")]
        [TestCase("El Gordo", "Random Selection")]
        [TestCase("SuperEnalotto", "Random Selection")]
        [TestCase("Euromiliony", "Systematic")]
        [TestCase("Powerball", "Systematic")]
        [TestCase("EuroMillions", "Additionals Guarantee")]
        [TestCase("EuroJackpot", "Additionals Guarantee")]
        [TestCase("Powerball", "New Winning")]
        [TestCase("Powerball", "New Random")]
        [Category("Parallel")]
        public void Add_New_Scanned_Image_Multiple(string lottery, string group)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/scanned_images/new");

            var imgPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Assets\scannedimage2.jpg");
            NewScannedImageObj page = new NewScannedImageObj(_driver); 

            page.AddNewImage_Multiple(imgPath, lottery, group);

            ScannedImagesObj imagesPage = new ScannedImagesObj(_driver);

            imagesPage.SwitchToMultipleTab(); 
            Assert.IsTrue(imagesPage.GetTheFirstPictureName().Contains("scannedimage"), "Sorry but the scannedimage picture doesn't exist, maybe it wasn't uploaded. Please check it ");

            imagesPage.FinishTheFirstMultiple();
        }

        /// <summary>
        /// Adds a new scanned image for a single ticket
        /// </summary>
        /// <param name="lottery"></param>
        [TestCase("El Gordo")]
        [TestCase("Euromiliony")]
        [TestCase("SuperLotto Plus")]
        [TestCase("Mega Millions")]
        [TestCase("Powerball")]
        [TestCase("EuroMillions")]
        [TestCase("SuperEnalotto")]
        public void Add_New_Scanned_Image_Single(string lottery)
        {
            _commonActions.SignIn_in_admin_panel();
            _driverCover.NavigateToUrl(_driverCover.BaseAdminUrl + "admin/scanned_images/new");

            var imgPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Assets\scannedimage2.jpg");
            NewScannedImageObj page = new NewScannedImageObj(_driver);
            page.AddNewImage_Single(imgPath, lottery);
            
            ScannedImagesObj imagesPage = new ScannedImagesObj(_driver);

            Assert.IsTrue(imagesPage.GetTheFirstPictureName().Contains("scannedimage"), "Sorry but the scannedimage picture doesn't exist, maybe it wasn't uploaded. Please check it ");

            //now delete this picture
            imagesPage.RemoveTheFirstPicture();
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new CommonActions(_driver);
            _backOfficeVerifications = new BackOfficeVerifications(_driver);
        }
    }
}
