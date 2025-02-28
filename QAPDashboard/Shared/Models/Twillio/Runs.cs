namespace QAPDashboard.Shared.Models.Twillio
{
    public class Runs
    {
        public required string RunName { get; set; }
        public required string TestName { get; set; }
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
