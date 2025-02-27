using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using QAPDashboard.Common.Bases;
using QAPDashboard.Shared.Configurations;

namespace QAPDashboard.Areas.API.Controllers
{
    [Area("API")]
    [Route("api")]
    public class APIController: TestRunnerControllerBase
    {
        public IConfiguration Configuration = new ConfigurationBuilder().Build();

        [HttpGet, Route("audio")]
        public IActionResult GetAudio(string audioId)
        {
            var creds = Base64Encode($"{RunnerConfiguration.TwilioAccountSid}:{RunnerConfiguration.TwilioAuthToken}");
            string audioUrl = $"https://api.twilio.com/2010-04-01/Accounts/{RunnerConfiguration.TwilioAccountSid}/Recordings/RE32153b1d70d795ed930331e1ab0788e7.wav";
            IDictionary<string, string> headers = new Dictionary<string, string>
            {
                { "Authorization", $"Basic {creds}"},
                { "Content-Type", "audio/x-wav" }
            };

            byte[] response;
            try
            {
                response = audioUrl.WithHeaders(headers)
                .GetBytesAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            }
            catch (FlurlHttpException ex)
            {
                throw new HttpRequestException($"REST API call failed. Attempted to GET from {audioUrl}", ex);
            }
            return File(response, "audio/x-wav");
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
