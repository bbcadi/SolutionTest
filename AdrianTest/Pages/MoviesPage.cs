using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace AdrianTest.Pages
{
    public class MoviesPage
    {
        string expectedUrlTitle = "Popular Movies — The Movie Database (TMDB)";
        string moviesPageContentClassName= "content";

        private string sortDropDownButtonElementSpan = "//*[@id=\"media_v4\"]/div/div/div[2]/div[1]/div[1]/div[1]/span";
        private string sortResultsByDropDownElementSpan = "//*[@id=\"media_v4\"]/div/div/div[2]/div[1]/div[1]/div[2]/span/button/span";
        private string sortResulsByReleaseDateAscendingElementSpan = "//*[@id=\"sort_by_listbox\"]/li[6]/span";
        private string sortResultsByDropDownDisplayedValueSpan = "//*[@id=\"media_v4\"]/div/div/div[2]/div[1]/div[1]/div[2]/span";
        private string searchButtonPath = "//*[@id=\"media_v4\"]/div/div/div[2]/div[1]/div[4]/p/a";
        private string genresListPath = "//*[@id=\"media_v4\"]/div/div/div[2]/div[1]/div[3]/div[5]/ul/li";
        
        private string releaseFromDateCalendar = "//*[@id=\"media_v4\"]/div/div/div[2]/div[1]/div[3]/div[4]/div[2]/span[2]/button";
        private string releaseToDateCalendar = "//*[@id=\"media_v4\"]/div/div/div[2]/div[1]/div[3]/div[4]/div[3]/span[2]/button";
        private string fromDatePath = "//*[@id=\"media_v4\"]/div/div/div[2]/div[1]/div[3]/div[4]/div[2]/span[2]/input";
        private string toDatePath = "//*[@id=\"media_v4\"]/div/div/div[2]/div[1]/div[3]/div[4]/div[3]/span[2]/input";

        private string minScoreTogglePath = "//*[@id=\"media_v4\"]/div/div/div[2]/div[1]/div[3]/div[8]/div/div/div[1]/span[1]";
        private string maxScoreTogglePath = "//*[@id=\"media_v4\"]/div/div/div[2]/div[1]/div[3]/div[8]/div/div/div[1]/span[2]";

        IWebDriver _webDriver = null;
        IWebElement _moviesPageContentClass = null;

        public MoviesPage(IWebDriver webDriver)
        {
            this._webDriver = webDriver;
            _moviesPageContentClass = _webDriver.FindElement(By.ClassName(moviesPageContentClassName));
        }

        public bool WaitForMoviesPageToBeLoaded()
        {
            bool isPopularMoviesPageDisplayed = false;
            
            try
            {
                var count = 0;

                while (!isPopularMoviesPageDisplayed && count < 10)
                {
                    Thread.Sleep(1000);
                    if (_webDriver.Title.Equals(expectedUrlTitle)) //we can also check the webdriver.url to match "https://www.themoviedb.org/movie"
                    {
                        isPopularMoviesPageDisplayed = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details                
            }

            return isPopularMoviesPageDisplayed;
        }

        public void FilterByReleaseDateAscending()
        {
            try
            {
                var sortDropDownButtonElement = GetSortDropDownButtonElement();
                sortDropDownButtonElement.Click();
                Thread.Sleep(500);

                var sortResultsByDropDownElement = sortDropDownButtonElement.FindElement(By.XPath(sortResultsByDropDownElementSpan));
                sortResultsByDropDownElement.Click();
                Thread.Sleep(1000);

                var sortResulsByReleaseDateAscendingElement = sortResultsByDropDownElement.FindElement(By.XPath(sortResulsByReleaseDateAscendingElementSpan));
                sortResulsByReleaseDateAscendingElement.Click();
                Thread.Sleep(1000);

                //press Search button to apply filter
                PressSearchForFilter();
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details                
            }
        }

        public string GetSortResultsByDisplayedValue()
        {
            string displayedValue = string.Empty;

            try
            {
                displayedValue = GetSortDropDownButtonElement().FindElement(By.XPath(sortResultsByDropDownDisplayedValueSpan)).Text;
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details                
            }

            return displayedValue;
        }

        public void ActivateGenreFilters(List<string> genreListToSelect)
        {
            try
            {
                var moviesGenresList = _moviesPageContentClass.FindElements(By.XPath(genresListPath));

                foreach (var genreToSelect in genreListToSelect)
                {
                    //get the desired genre web element
                    var desiredGenre = moviesGenresList.SingleOrDefault(gn => (gn as IWebElement).Text.ToLower().Equals(genreToSelect.ToLower()));

                    if (desiredGenre != null)
                    {
                        Actions actions = new Actions(_webDriver);
                        actions.MoveToElement(desiredGenre);
                        actions.Perform();

                        desiredGenre.Click();

                        //press Search button to apply filter
                        PressSearchForFilter();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details               
            }
        }

        public bool IsGenreSelected(string genre)
        {
            bool isGenreSelected = false;
            try
            {
                var moviesGenresList = _webDriver.FindElement(By.ClassName(moviesPageContentClassName)).FindElements(By.XPath(genresListPath));

                var desiredGenre = moviesGenresList.SingleOrDefault(gn => (gn as IWebElement).Text.ToLower().Equals(genre.ToLower()));
                if (desiredGenre.GetAttribute("class").Equals("selected"))
                {
                    isGenreSelected = true;
                }
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details               
            }

            return isGenreSelected;
        }

        public void FilterByReleaseDatesUsingCalendar()
        {
            //add from date
            Thread.Sleep(1000);
            var fromDateCalendarPath = _moviesPageContentClass.FindElement(By.XPath(releaseFromDateCalendar));
            fromDateCalendarPath.Click();
            Thread.Sleep(500);

            var calendarFrame = _webDriver.FindElement(By.Id("release_date_gte_dateview"));
            //var calendarFrame = _webDriver.FindElement(By.XPath("/html/body/div[17]"));//*[@id="release_date_gte_dateview"]

            //go to select year mode
            string yearNavigateButtonPath = "/html/body/div[17]/div/div/div/div[1]/button";
            var yearNavigateButton = calendarFrame.FindElement(By.XPath(yearNavigateButtonPath));
            yearNavigateButton.Click();
            Thread.Sleep(500);

            //go to year selection
            string yearButtonPath = "/html/body/div[17]/div/div/div/div[1]/button";
            var yearButtonElement = calendarFrame.FindElement(By.XPath(yearButtonPath));
            yearButtonElement.Click();
            Thread.Sleep(500);

            //go to desired year page
            string yearBackButtonPath = "/html/body/div[17]/div/div/div/div[1]/span[2]/button[1]";
            var yearBackButton = calendarFrame.FindElement(By.XPath(yearBackButtonPath));
            for (int i = 0; i < 7; i++)
            {
                yearBackButton.Click();
                Thread.Sleep(500);
            }

            //select desired year
            string desiredYearPath = "/html/body/div[17]/div/div/div/div[2]/table/tbody/tr[1]/td[1]/span";
            var desiredYearButton = calendarFrame.FindElement(By.XPath(desiredYearPath));
            desiredYearButton.Click();
            Thread.Sleep(500);

            //month
            string desiredMonthPath = "/html/body/div[17]/div/div/div/div[2]/table/tbody/tr[1]/td[1]/span";
            var desiredMonthButton = calendarFrame.FindElement(By.XPath(desiredMonthPath));
            desiredMonthButton.Click();
            Thread.Sleep(500);

            //day
            string desiredDayPath = "/html/body/div[17]/div/div/div/div[2]/table/tbody/tr[1]/td[1]/span";
            var desiredDayButton = calendarFrame.FindElement(By.XPath(desiredDayPath));
            desiredDayButton.Click();
            Thread.Sleep(500);

            //add to date

            //clear existing date
            var toDateTextInputElement = _moviesPageContentClass.FindElement(By.XPath(toDatePath));
            toDateTextInputElement.Clear();
            Thread.Sleep(100);

            var toDateCalendarPath = _moviesPageContentClass.FindElement(By.XPath(releaseToDateCalendar));
            toDateCalendarPath.Click();
            Thread.Sleep(500);
            
            var toDateCalendarFrame = _webDriver.FindElement(By.Id("release_date_lte_dateview"));
            //var toDateCalendarFrame = _webDriver.FindElement(By.XPath("/html/body/div[18]"));

            //go to select year mode
            string yearNavigateButtonPath2 = "/html/body/div[18]/div/div/div/div[1]/button";
            var yearNavigateButton2 = toDateCalendarFrame.FindElement(By.XPath(yearNavigateButtonPath2));
            yearNavigateButton2.Click();
            Thread.Sleep(500);

            //go to year selection
            string yearButtonPath2 = "/html/body/div[18]/div/div/div/div[1]/button";
            var yearButtonElement2 = toDateCalendarFrame.FindElement(By.XPath(yearButtonPath2));
            yearButtonElement2.Click();
            Thread.Sleep(500);

            //go to desired year page
            string yearBackButtonPath2 = "/html/body/div[18]/div/div/div/div[1]/span[2]/button[1]";
            var yearBackButton2 = toDateCalendarFrame.FindElement(By.XPath(yearBackButtonPath2));
            for (int i = 0; i < 2; i++)
            {
                yearBackButton2.Click();
                Thread.Sleep(500);
            }

            //select desired year
            string desiredYearPath2 = "/html/body/div[18]/div/div/div/div[2]/table/tbody/tr[2]/td[2]/span";
            var desiredYearButton2 = toDateCalendarFrame.FindElement(By.XPath(desiredYearPath2));
            desiredYearButton2.Click();
            Thread.Sleep(500);

            //month
            string desiredMonthPath2 = "/html/body/div[18]/div/div/div/div[2]/table/tbody/tr[3]/td[4]/span";
            var desiredMonthButton2 = toDateCalendarFrame.FindElement(By.XPath(desiredMonthPath2));
            desiredMonthButton2.Click();
            Thread.Sleep(500);

            //day
            string desiredDayPath2 = "/html/body/div[18]/div/div/div/div[2]/table/tbody/tr[5]/td[7]/span";
            var desiredDayButton2 = toDateCalendarFrame.FindElement(By.XPath(desiredDayPath2));
            desiredDayButton2.Click();
            Thread.Sleep(500);

            //press Search button to apply filter
            PressSearchForFilter();
            Thread.Sleep(2000);
        }

        public void FilterByReleaseDates(string fromDate, string toDate)
        {

            AddFromDateReleaseValue(fromDate);
            AddToDateReleaseValue(toDate);

            //press Search button to apply filter
            PressSearchForFilter();
            Thread.Sleep(2000);
        }

        private void AddFromDateReleaseValue(string fromDate)
        {
            try
            {
                var fromDateTextInputElement = _moviesPageContentClass.FindElement(By.XPath(fromDatePath));
                fromDateTextInputElement.Clear();
                Thread.Sleep(100);
                fromDateTextInputElement.Click();
                fromDateTextInputElement.SendKeys(fromDate);                

                //releaseFromDateCalendar
            }
            catch (Exception ex)
            {
                throw (ex);               
            }
        }

        public string GetFromReleaseDateDisplayeValue()
        {
            string displayedValue = string.Empty;

            try
            {
               displayedValue = _moviesPageContentClass.FindElement(By.XPath(fromDatePath)).Text;
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details                
            }

            return displayedValue;
        }

        private void AddToDateReleaseValue(string toDate)
        {
            try
            {
                var toDateTextInputElement = _moviesPageContentClass.FindElement(By.XPath(toDatePath));
                toDateTextInputElement.Clear();
                Thread.Sleep(100);
                toDateTextInputElement.Click();
                toDateTextInputElement.SendKeys(toDate);

                //releaseToDateCalendar
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public string GetToReleaseDateDisplayeValue()
        {
            string displayedValue = string.Empty;

            try
            {
                displayedValue = _moviesPageContentClass.FindElement(By.XPath(toDatePath)).Text;
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details                
            }

            return displayedValue;
        }

        public void FilterByScore()
        {
            try
            {
                Actions actions = new Actions(_webDriver);

                //min user score
                var minScoreToggle = _moviesPageContentClass.FindElement(By.XPath(minScoreTogglePath));

                actions.MoveToElement(minScoreToggle);
                actions.ClickAndHold(minScoreToggle).MoveByOffset(70, 0).Release().Build();
                actions.Perform();

                //press Search button to apply filter
                PressSearchForFilter();
                Thread.Sleep(1000);

                //max user score
                var maxScoreToggle = _moviesPageContentClass.FindElement(By.XPath(maxScoreTogglePath));
                actions.ClickAndHold(maxScoreToggle).MoveByOffset(-50, 0).Release().Build();
                actions.Perform();
                Thread.Sleep(200);

                //press Search button to apply filter
                PressSearchForFilter();
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details                
            }            
        }

        private IWebElement GetSortDropDownButtonElement()
        {
            IWebElement sortDropDownButtonElement = null;
            
            try
            {
                sortDropDownButtonElement = _webDriver.FindElement(By.ClassName(moviesPageContentClassName)).FindElement(By.XPath(sortDropDownButtonElementSpan));
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details                
            }

            return sortDropDownButtonElement;
        }

        private void PressSearchForFilter()
        {
            try
            {
                var moviesPageContentClass = _webDriver.FindElement(By.ClassName(moviesPageContentClassName));

                var searchButton = moviesPageContentClass.FindElement(By.XPath(searchButtonPath));
                searchButton.Click();
                Thread.Sleep(1000);


            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details                
            }
        }        
    }
}
