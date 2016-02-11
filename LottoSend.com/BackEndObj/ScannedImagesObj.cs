using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj
{
    public class ScannedImagesObj : DriverCover
    {
        public ScannedImagesObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("scanned_images"))
            {
                throw new Exception("Sorry, it must be not 'Scanned Images' page, please check it. The current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "li.scope.actives_multiples > a")]
        private IWebElement _activesMultipleTab;

        [FindsBy(How = How.CssSelector, Using = "#index_table_scanned_images > tbody:nth-child(2)")]
        private IWebElement _table;

        /// <summary>
        /// Clicks on "Actives Multiples" tab
        /// </summary>
        public void SwitchToMultipleTab()
        {
            _activesMultipleTab.Click();
            WaitForPageLoading();
        }

        /// <summary>
        /// Finishes the first multiple picture
        /// </summary>
        public void FinishTheFirstMultiple()
        {
            RemoveTheFirstPicture();
        }

        /// <summary>
        /// Removes the first picture (at the top)
        /// </summary>
        public void RemoveTheFirstPicture()
        {
            _table.FindElement(By.CssSelector("tr > td:nth-child(6) > a")).Click();
        }

        /// <summary>
        /// Returns "alt" of the first picture
        /// </summary>
        /// <returns></returns>
        public string GetTheFirstPictureName()
        {
            return _table.FindElement(By.CssSelector("tr > td.col-scanned_image > img")).GetAttribute("alt");
        }

        /// <summary>
        /// In all images looks for "alt" atribute and if it contains name of the picture
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IfPictureExist(string name)
        {
            IList<IWebElement> images = _table.FindElements(By.CssSelector("tr > td.col-scanned_image > img"));

            bool isFound = false;

            foreach (var image in images)
            {
                if (image.GetAttribute("alt").Contains(name))
                {
                    isFound = true;
                }
            }

            return isFound;
        }
    }
}
