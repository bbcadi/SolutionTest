using System.Configuration;

namespace AdrianTest.Utils
{
    public class AUTConfigServices
    {
        private string _testedUrlPathKey = "TestedUrlPath";
        private string _APIUrlPathKey = "APIUrlPath";
        private string _APIKey = "APIKey";
        
        
        private readonly string _testedUrlPath;
        private readonly string _apiUrlPath;
        private readonly string _apiUrlPathWithAPIKey;

        public AUTConfigServices()
        {
            _testedUrlPath = GetValue(_testedUrlPathKey);
            
            _apiUrlPath = GetValue(_APIUrlPathKey);
            _apiUrlPathWithAPIKey = _apiUrlPath + "?api_key=" + GetValue(_APIKey);
        }

        public string GetTestedUrlPath()
        {
            return _testedUrlPath;
        }

        public string GetAPIUrlPath()
        {
            return _apiUrlPathWithAPIKey;
        }

        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
