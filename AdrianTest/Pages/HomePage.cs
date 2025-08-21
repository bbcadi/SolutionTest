using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace AdrianTest.Pages
{
    class HomePage : BasePage
    {
        private string testedUrl = "https://themoviedb.org";
        private string expectedUrlTitle = "The Movie Database (TMDB)";
        private string moviesMenuElementText = "Movies";

        private string navigationMenuElementPath = "dropdown_menu";
        private string popularMoviesDropdownItemPath = "//*[@aria-label='Popular']";
        private string rejectAllCookiesButtonPath = "onetrust-reject-all-handler";

        public HomePage(IWebDriver driver) : base(driver)
        {
            GoToUrl(testedUrl);
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
                    if (driver.Title.Equals(expectedUrlTitle))
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

                IWebElement rejectAllCookiesButtonElement = driver.FindElement(By.Id(rejectAllCookiesButtonPath));
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
                driver.Navigate().GoToUrl(testedUrl);
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
                IWebElement navigationMenuElement = driver.FindElement(By.ClassName(navigationMenuElementPath));
                IWebElement moviesMenuElement = navigationMenuElement.FindElement(By.LinkText(moviesMenuElementText));
                
                //Creating object of an Actions class
                Actions action = new Actions(driver);

                //Performing the mouse hover action on the target element.
                action.MoveToElement(moviesMenuElement).Perform();
                Thread.Sleep(500);

                IWebElement popularMoviesDropdownItem = navigationMenuElement.FindElement(By.XPath(popularMoviesDropdownItemPath));
                popularMoviesDropdownItem.Click();
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details
            }
        }        
    }
}
