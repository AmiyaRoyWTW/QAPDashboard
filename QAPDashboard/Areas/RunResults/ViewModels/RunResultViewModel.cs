using QAPDashboard.Common.Bases;

namespace QAPDashboard.Areas.RunResults.ViewModels
{
    public class RunResultViewModel : ViewModelBase
    {
        public string? RunId { get; set; }
        public string RunStatus { get; set; } = String.Empty;
        public List<StepViewModel> Steps { get; set; } = [];
        public string? DialResult { get; set; } = "Answered";
        public DateTime RunDate { get; set; }
        public string RunDuration { get; set; } = "00:00:00";
    }

    public class StepViewModel
    {
        public int StepId { get; set; }
        public string StepName { get; set; } = String.Empty;
        public string ExpectToHear { get; set; } = String.Empty;
        public string? Transcription { get; set; } = String.Empty;
        public string? ReplyWith { get; set; } = "N/A";
        public string? DetailedResult { get; set; } = "N/A";
        public int StepDuration { get; set; }
        public int ResponseTime { get; set; }
        public string Status { get; set; } = String.Empty;
        public double Confidence { get; set; }
    }
}
