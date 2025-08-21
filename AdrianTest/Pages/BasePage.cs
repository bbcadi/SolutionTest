using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace AdrianTest.Pages
{
    class BasePage
    {
        public IWebDriver driver = null;
        public BasePage(IWebDriver driver)
        { 
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }
    }
}
