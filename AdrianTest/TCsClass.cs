using System;
using System.Collections.Generic;
using System.Threading;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using AdrianTest.Pages;

namespace AdrianTest
{
    [TestFixture]
    public class TCsClass
    {
        IWebDriver _webDriver = null;
        HomePage _homePage = null;

        [SetUp]
        public void InitBrowser()
        {
            try
            {
                //open Chrome driver and maximeze it
                _webDriver = new ChromeDriver();
                Thread.Sleep(1000);
                _webDriver.Manage().Window.Maximize();
                Thread.Sleep(3000);

                //go to home page and wait for it to be opened
                _homePage = new HomePage(_webDriver);
                _homePage.WaitForHomePageToBeLoaded();

                _homePage.RejectAllCookies();
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details
            }
        }

        [TearDown]
        public void CloseBrowser()
        {
            _webDriver.Close();
        }

        [Test]
        public void Tasks_1_2()
        {
            //Task 1:  A.Filter by relese date ascending
            _homePage.OpenMoviesPage();

            //wait for Movies page to be loaded 
            MoviesPage moviesPage = new MoviesPage(_webDriver);
            var popularMoviesPageIsDisplayed = moviesPage.WaitForMoviesPageToBeLoaded();

            //Filter by 'Release Date Ascending'
            moviesPage.FilterByReleaseDateAscending();

            //Task 1:  B. Select one or multiple Genres
            List<string> genreList = new List<string>() { "Action", "Comedy", "Horror" };
            moviesPage.ActivateGenreFilters(genreList);

            //Task 1:  C. Search by release date - 1990 - 2005
            string fromDate = "1/1/1950";
            string toDate = "12/31/2005";
            moviesPage.FilterByReleaseDates(fromDate, toDate);//add value in field
            //Task 1:  C. pct 1
            //moviesPage.FilterByReleaseDatesUsingCalendar();

            //Task 1:  C. pct 2
            //filter by score
            moviesPage.FilterByScore();

            //
            //Task 2: Check the filtering was done correctly

            //check 'Sort Results By' displayed value 
            string sortResultsByDisplayedValue = moviesPage.GetSortResultsByDisplayedValue();
            //assert
            Assert.That(sortResultsByDisplayedValue.Equals("Release Date Ascending"));

            //check 'Release Dates' displayed values
            //TODO - investigate why the text property of the field does not contain the displayed value to be able to perform the below assert
            //string fromReleaseDateDisplayeValue = moviesPage.GetFromReleaseDateDisplayeValue();
            //string toDateReleaseDisplayeValue = moviesPage.GetToReleaseDateDisplayeValue();
            ////assert
            //Assert.That(fromReleaseDateDisplayeValue.Equals(fromDate));
            //Assert.That(toDateReleaseDisplayeValue.Equals(toDate));

            //check genres displaye as selected
            bool isActionGenreSelected = moviesPage.IsGenreSelected(genreList[0]);
            Assert.That(isActionGenreSelected.Equals(true));
            bool isCpmedyGenreSelected = moviesPage.IsGenreSelected(genreList[1]);
            Assert.That(isCpmedyGenreSelected.Equals(true));

        }
    }
}
