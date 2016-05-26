using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace LottoSend.com.BackEndObj.RegularTicketsPages
{
    /// <summary>
    /// Page Obcject of back/packages page (admin/packages)
    /// </summary>
    public class PackagesPageObj : DriverCover
    {
        public PackagesPageObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("packages"))
            {
                throw new Exception("Sorry it must be not back/packages page. Current URL is: " + Driver.Url + " ");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class, 'scope active')]")]
        private IWebElement _activeTab;

        [FindsBy(How = How.XPath, Using = "//*[contains(@class, 'scope inactive')]")]
        private IWebElement _inactiveTab;

        [FindsBy(How = How.XPath, Using = "//*[@id='q_template_template_name']")]
        private IWebElement _templateName;

        [FindsBy(How = How.XPath, Using = "//div[@class='buttons']/input[@value='Filter']")]
        private IWebElement _filterButton;

        [FindsBy(How = How.XPath, Using = "//*[@class='index_table index']/tbody/tr")]
        private IList<IWebElement> _records;

        /// <summary>
        /// Edits a package with specific number of lines of specific template
        /// </summary>
        /// <param name="template">Template name</param>
        /// <param name="lines">Number of lines in the package</param>
        public NewPackagePageObj EditPackage(string template, int lines)
        {
            ChooseElementInSelect(template,_templateName, SelectBy.Text);
            _filterButton.Click();
            WaitAjax();
            WaitForPageLoading();

            foreach (var record in _records)
            {
                if (record.FindElement(By.XPath("./td[3]")).Text.Equals(lines.ToString()))
                {
                    record.FindElement(By.XPath(".//a[@class='edit_link member_link']")).Click();
                    break;
                }
            }

            return new NewPackagePageObj(Driver);
        }
    }
}
