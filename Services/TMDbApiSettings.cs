namespace Services
{
    public class ApiSettings
    {
        public ApiSettings() {
            ApiKey = string.Empty;
            MovieBaseUrl = string.Empty;
            SearchBaseUrl = string.Empty;
            BaseUrl = string.Empty;
            RestCountriesBaseUrl = string.Empty;
        }
        public string ApiKey { get; set; }
        public string MovieBaseUrl { get; set; }
        public string SearchBaseUrl { get; set; }
        public string BaseUrl { get; set; }
        public string RestCountriesBaseUrl { get; set; }
    }
}