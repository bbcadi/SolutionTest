using NUnit.Framework;

using System;

using Newtonsoft.Json;

using AdrianTest.APIs;

namespace AdrianTest
{
    [TestFixture]
    public class API_TCsClass
    {
        private enum SortByType
        {
            primary_release_date //TODO add rest of the sort by options          
        }

        private enum SortByMode
        {
            asc,
            des
        }

        //use here or below as testcase attribute (if we need to run the same TC with multiple sets of data)
        //private readonly string fromDate = "1950-01-01";
        //private readonly string toDate = "2005-12-31";
        //private readonly string genreIds = "28,35";

        [TestCase ("1950-01-01", "2005-12-31", "28,35")]
        public void Tasks_3(string fromDate, string toDate, string genreIds)
        {
            try
            {
                MoviesPage_API serv = new MoviesPage_API();
                
                //deserialize the string response from the unfiltered movies page
                var unfilteredMovieListResponse = serv.GetListOfMovies_Unfiltered();                
                MoviesPageResponse unfilteredMovieList = JsonConvert.DeserializeObject<MoviesPageResponse>(unfilteredMovieListResponse.Content);

                //deserialize the string response from the filtered movies page
                string sortByOptoin = SortByType.primary_release_date.ToString() + "." + SortByMode.asc.ToString();
                var FilteredMovieListResponse = serv.GetListOfMovies_Filtered(fromDate, toDate, genreIds, sortByOptoin); // TODO - create and use a filter model
                MoviesPageResponse filteredMovieList = JsonConvert.DeserializeObject<MoviesPageResponse>(FilteredMovieListResponse.Content);

                //assert steps
                Assert.That(!unfilteredMovieList.total_pages.Equals(filteredMovieList.total_pages)); 
                Assert.That(!unfilteredMovieList.total_results.Equals(filteredMovieList.total_results));
                Assert.That(!unfilteredMovieList.results.Equals(filteredMovieList.results));
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details
            }            
        }
    }
}

