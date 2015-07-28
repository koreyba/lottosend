using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoSend.com.FrontEndObj
{
    public class BottomMenuObj : DriverCover
    {
        public BottomMenuObj(IWebDriver driver) : base(driver)
        {
            if (!Driver.Url.Contains("lottobaba"))
            {
                throw new Exception("No footer is found. Check if the correct url is opened");
            }

            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "body > footer:nth-child(7)")]
        private IWebElement _playColumn;

        public void VisitPlayPages()
        {
            IList<IWebElement> links = _playColumn.FindElements(By.CssSelector("ul a"));
            List<string> urls = new List<string>();
            foreach(IWebElement link in links)
            {
                urls.Add(link.GetAttribute("href"));
            }

            foreach(string url in urls)
            {
                NavigateToUrl(url);

                //TODO: write checking for 200 status

                //    IList<IWebElement> errors = Driver.FindElements(By.CssSelector("div.container + h1.text-center"));
                //    if(errors.Count > 0)
                //    {
                //        throw new Exception("The respons status of the page is 404");
                //    }
            }
        }
    }
}
