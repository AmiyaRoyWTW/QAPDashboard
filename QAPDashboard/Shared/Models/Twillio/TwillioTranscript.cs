using Newtonsoft.Json;

namespace QAPDashboard.Shared.Models.Twillio
{
    public class TwillioTranscript
    {
        [JsonProperty("status")]
        public required string Status { get; set; }
        [JsonProperty("transcription")]
        public required List<Transcription> Transcription { get; set; }
    }

    public class Transcription
    {
        [JsonProperty("transcript")]
        public required string Transcript { get; set; }
        [JsonProperty("confidence")]
        public required double Confidence { get; set; }
        [JsonProperty("timestamp")]
        public required DateTime Timestamp { get; set; }
    }
}
