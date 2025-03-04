using Newtonsoft.Json;
using System.Text.Json;

namespace QAPDashboard.Shared.Models.Twillio
{
    public class CallResponse
    {
        [JsonProperty("date_created")]
        public DateTime DateCreated { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("from")]
        public string CallingNumber { get; set; } = string.Empty;
        [JsonProperty("forwarded_from")]
        public string CalledNumber { get; set; } = string.Empty;
        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;
        public string CallId { get; set; } = string.Empty;
    }
}
