namespace QAPDashboard.Shared.Models
{
    public class TestRunResult
    {
        public List<TestCase> StartedTestCases { get; set; }
        public List<TestCase> BlockedTestCases { get; set; }
        public List<TestCase> FailedTestCases { get; set; }
        public Dictionary<string, List<TestCase>> FailedTestCasesByMessage { get; set; }
        public List<TestCase> PassedTestCases { get; set; }
        public List<string> NonPassedTestCaseQueryStrings { get; set; }
        public List<TestCase> QueuedTestCases { get; set; }
        public string EndTime { get; set; }
    }
}
