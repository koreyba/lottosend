using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework;
using TestFramework.BackEndObj.ManualMatchPages;
using TestFramework.Helpers;

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
        private TestFramework.CommonActions _commonActions;
        private TestsSharedCode _sharedCode;
        private bool _setUpFailed = false;

        /// <summary>
        /// Adds a new scanned image for a group ticket
        /// </summary>
        /// <param name="lottery"></param>
        /// <param name="group"></param>
        [TestCase("El Gordo", "Winning Guarantee - Backup")]
        [TestCase("Mega Millions", "Winning Guarantee - Backup")]
        [TestCase("SuperLotto Plus", "Winning Guarantee - Backup")]
        [TestCase("Powerball", "Winning Guarantee - Backup")]
        [TestCase("EuroMillions", "Winning Guarantee - Backup")]
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
        [TestCase("EuroMillions", "Additionals Guarantee ")]
        [TestCase("EuroJackpot", "Additionals Guarantee ")]
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
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || _setUpFailed == true)
                {
                    _driverCover.TakeScreenshot();
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                _sharedCode.CleanUp(ref _driver);
            }
        }

        [SetUp]
        public void SetUp()
        {
            MessageConsoleCreator message = new MessageConsoleCreator();
            message.TestWillRun();
            _driver = new TWebDriver();
            _driverCover = new DriverCover(_driver);
            _commonActions = new TestFramework.CommonActions(_driver);
            _sharedCode = new TestsSharedCode(_driver);
        }
    }
}
