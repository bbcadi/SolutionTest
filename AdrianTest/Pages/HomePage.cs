using System;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

using AdrianTest.Utils;

namespace AdrianTest.Pages
{
    class HomePage : BasePage
    {
        private string _testedUrl = string.Empty;
        private string _expectedUrlTitle = "The Movie Database (TMDB)";
        private string _moviesMenuElementText = "Movies";

        private string _navigationMenuElementPath = "dropdown_menu";
        private string _popularMoviesDropdownItemPath = "//*[@aria-label='Popular']";
        private string _rejectAllCookiesButtonPath = "onetrust-reject-all-handler";

        public HomePage(IWebDriver driver) : base(driver)
        {            
            _testedUrl = new AUTConfigServices().GetTestedUrlPath();
            GoToUrl(_testedUrl);
        }        

        public void OpenMoviesPage()//TODO
        {
            GetCurrentWindowHandle();//to be used later
            OpenPopularMoviesPage();            
        }        

        public void WaitForHomePageToBeLoaded()
        {
            try
            {
                var pageWasLoaded = false;
                var count = 0;

                while (!pageWasLoaded && count < 10)
                {
                    Thread.Sleep(1000);
                    if (driver.Title.Equals(_expectedUrlTitle))
                    {
                        pageWasLoaded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details
            }            
        }

        public void RejectAllCookies()
        {
            try
            {
                //added as sometimes the cookies element appears later on screen 
                Thread.Sleep(2000);
                //TODO - implicit wait for cookies element to appear on screen

                IWebElement rejectAllCookiesButtonElement = driver.FindElement(By.Id(_rejectAllCookiesButtonPath));
                rejectAllCookiesButtonElement.Click();
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details
            }
        }

        private void GoToUrl(string url)
        {
            try
            {
                driver.Navigate().GoToUrl(url);
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details
            }
        }        

        private void GetCurrentWindowHandle()
        {
            try
            {
                String mainWindowHandle = driver.CurrentWindowHandle;
                Console.WriteLine("Crt window handle -> " + mainWindowHandle);
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details
            }
        }

        private void OpenPopularMoviesPage()
        {
            try
            {
                IWebElement navigationMenuElement = driver.FindElement(By.ClassName(_navigationMenuElementPath));
                IWebElement moviesMenuElement = navigationMenuElement.FindElement(By.LinkText(_moviesMenuElementText));
                
                //Creating object of an Actions class
                Actions action = new Actions(driver);

                //Performing the mouse hover action on the target element.
                action.MoveToElement(moviesMenuElement).Perform();
                Thread.Sleep(500);

                IWebElement popularMoviesDropdownItem = navigationMenuElement.FindElement(By.XPath(_popularMoviesDropdownItemPath));
                popularMoviesDropdownItem.Click();
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details
            }
        }        
    }
}
