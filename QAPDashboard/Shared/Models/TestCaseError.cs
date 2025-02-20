namespace QAPDashboard.Shared.Models
{
    public class TestCaseError
    {
        public int Id { get; set; }
        public Guid TestRunId { get; set; }
        public Guid TestCaseId { get; set; }
        public string Message { get; set; }
        public string Trace { get; set; }
        public string Url { get; set; }
        public string PageTitle { get; set; }
        public string ScreenshotBase64 { get; set; }
        public string ErrorType { get; set; }
    }
}
