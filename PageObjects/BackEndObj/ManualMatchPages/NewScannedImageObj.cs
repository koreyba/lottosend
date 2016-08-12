using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TestFramework.BackEndObj.ManualMatchPages
{
    /// <summary>
    /// Page Object of New Scanned Image (add) page
    /// </summary>
    public class NewScannedImageObj : DriverCover
    { 
        public NewScannedImageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("scanned_images/new"))
            {
                throw new Exception("Sorry, it must be not New Scanned Image page, check it. The current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#scanned_image_lottery_id")]
        private IWebElement _lottery;

        [FindsBy(How = How.CssSelector, Using = "#scanned_image_kind")]
        private IWebElement _kind;

        [FindsBy(How = How.CssSelector, Using = "#scanned_image_local_officer_id")]
        private IWebElement _localOfficer;

        [FindsBy(How = How.CssSelector, Using = "#scanned_image_group_id")]
        private IWebElement _group;

        [FindsBy(How = How.CssSelector, Using = "#scanned_image_server_image")]
        private IWebElement _chooseFile;

        [FindsBy(How = How.CssSelector, Using = "#scanned_image_submit_action > input")]
        private IWebElement _createScannedImageButton;

        /// <summary>
        /// Adds a new image to the specific lottery
        /// </summary>
        /// <param name="path">path to the picture</param>
        /// <param name="lottery">lottery name</param>
        public ScannedImagesObj AddNewImage_Single(string path, string lottery)
        {
            ChooseElementInSelect(lottery, _lottery, SelectBy.Text);

            ChooseElementInSelect("Single Match", _kind, SelectBy.Text);

            ChooseElementInSelect("3", _localOfficer, SelectBy.Index);
            
            _chooseFile.SendKeys(path);

            _createScannedImageButton.Click();

            WaitForPageLoading();
            
            return new ScannedImagesObj(Driver);
        }

        /// <summary>
        /// Adds a new image to the specific lottery
        /// </summary>
        /// <param name="path">path to the picture</param>
        /// <param name="lottery">lottery name</param>
        /// <param name="group">name of group</param>
        public ScannedImagesObj AddNewImage_Multiple(string path, string lottery, string group)
        {
            ChooseElementInSelect(lottery, _lottery, SelectBy.Text);

            ChooseElementInSelect("Multiple Match", _kind, SelectBy.Text);

            ChooseElementInSelect(group, _group, SelectBy.Text);
            
            _chooseFile.SendKeys(path);

            Thread.Sleep(TimeSpan.FromSeconds(0.5));

            _createScannedImageButton.Click();

            WaitForPageLoading();

            return new ScannedImagesObj(Driver);
        }
    }
}
