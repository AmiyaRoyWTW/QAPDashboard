namespace QAPDashboard.Shared.Models.Twillio
{
    public class Runs
    {
        public List<string> TestCases { get; set; } = [];
        public string SelectedTestCaseFilter { get; set; } = "All";
        public List<string> DateRanges { get; set; } = ["Last 15 Minutes", "Last Hour", "Last 12 Hours", "Last 24 Hours", "This Week", "This Month", "Last Calendar Month", "Custom Range"];
        public string SelectedDateRange { get; set; } = "This Month";
        public string? CustomStartDate { get; set; }
        public string? CustomEndDate { get; set; }
        public List<Calls> Calls { get; set; } = [];
    }
    public class Calls
    {
        public required string CallId { get; set; }
        public string CallingNumber { get; set; } = String.Empty;
        public string CalledNumber { get; set; } = String.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Result { get; set; } = "Pending";
        public string Duration { get; set; } = "00:00:00";
        public string AudioFile { get; set; } = String.Empty;
        public string DetailedResults { get; set; } = String.Empty;
        public string AudioFileName { get; set; } = String.Empty;
    }
}
