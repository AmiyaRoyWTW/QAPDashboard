namespace QAPDashboard.Shared.Models
{
    public class TestCase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TestSuite TestSuite { get; set; }
        public TestCaseError TestCaseError { get; set; }
        public int Attempts { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string PageTitle { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Info => $"{(TestSuite?.Name != null ? $"Test Suite: {TestSuite.Name}, " : "")}Test: {Name}";
        public string Description { get; set; }
        public List<string> Attributes { get; set; }
        public Guid TestRunId { get; set; }
    }
}
