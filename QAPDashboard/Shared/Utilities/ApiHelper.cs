using Flurl.Http;

namespace QAPDashboard.Shared.Utilities
{
    public class ApiHelper
    {
        protected static string Get(string url, IDictionary<string, string>? headers = null){
            string result;
            try{
                var response = url.WithHeaders(headers)
                .GetAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

                result = response.ResponseMessage.Content.ReadAsStringAsync()
                                        .ConfigureAwait(false)
                                        .GetAwaiter()
                                        .GetResult();
            }catch(FlurlHttpException ex){
                throw new HttpRequestException($"REST API call failed. Attempted to GET from {url}", ex);
            }
            return result;
        }

        protected static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

    }
}
