namespace QAPDashboard.Shared.Models.Twillio
{
    public class TestStepStats
    {
        public int StepId { get; set; }
        public string StepName { get; set; } = string.Empty;
        public string ExpectToHear { get; set; } = string.Empty;
        public string Transcription { get; set; } = string.Empty;
        public string ReplyWith { get; set; } = "N/A";
        public string DetailedResult { get; set; } = "N/A";
        public int StepDuration { get; set; }
        public int ResponseTime { get; set; }
        public double Confidence { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
