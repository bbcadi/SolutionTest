using System;
using RestSharp;
using AdrianTest.Utils;

namespace AdrianTest.APIs
{
    public class MoviesPage_API
    {
        private readonly string _apiUrl = string.Empty;
        AUTConfigServices _aUTConfigServices = new AUTConfigServices();
        private string _releaseDateFrom = "ReleaseDateFrom";
        private string _releaseDateTo = "ReleaseDateTo";

        public MoviesPage_API()
        {
            _apiUrl = _aUTConfigServices.GetAPIUrlPath();
        }

        public RestResponse GetListOfMovies_Unfiltered()
        {
            RestResponse response = new RestResponse();

            try
            {
                var client = new RestClient(_apiUrl);

                var request = new RestRequest();

                response = client.Get(request);
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details
            }

            return response;
        }

        public RestResponse GetListOfMovies_Filtered(string fromDate, string toDate, string genresIdList, string sortBy)
        {
            RestResponse response = new RestResponse();

            try
            {
                var client = new RestClient(_apiUrl);

                var request = new RestRequest();

                //get the param name from either an app config file or use them hardcoded (not the best way)
                request.AddParameter(_aUTConfigServices.GetValue(_releaseDateFrom), fromDate);
                request.AddParameter(_aUTConfigServices.GetValue(_releaseDateTo), toDate);
                request.AddParameter("with_genres", genresIdList);
                request.AddParameter("sort_by", sortBy);

                response = client.Get(request);
            }
            catch (Exception ex)
            {
                throw (ex); //TODO -to be improved for exception details
            }

            return response;
        }
    }
}
