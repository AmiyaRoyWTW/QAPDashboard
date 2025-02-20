namespace QAPDashboard.Shared.Models
{
    public class TestRunSummary
    {
        public Guid TestRunId { get; set; }
        public string BaseUrl { get; set; }
        public string Browser { get; set; }
        public string Application { get; set; }
        public List<TestSuite> TestSuites { get; set; }
        public string TestSuiteType { get; set; }
        public string DashboardUrl { get; set; }
        public int Threads { get; set; }
        public int TestCasesCount { get; set; }
        public int QueuedCount { get; set; }
        public int PassedCount { get; set; }
        public int BlockedCount { get; set; }
        public int FailedCount { get; set; }
        public int StartedCount { get; set; }
        public bool IsReleaseId { get; set; }
        public string RunReportUrl { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<TestCase> FailedTestCases { get; set; }
        public Dictionary<string, List<TestCase>> FailedTestCasesByMessage { get; set; }
        public List<TestCase> PassedTestCases { get; set; }
        public List<TestCase> BlockedTestCases { get; set; }
        public List<TestCase> StartedTestCases { get; set; }
        public List<TestCase> QueuedTestCases { get; set; }
        public List<string> NonPassedTestCaseQueryStrings { get; set; }
    }
}
