namespace QAPDashboard.Shared.Models
{
    public class TestRun
    {
        public Guid Id { get; set; }
        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Browser { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Labels { get; set; }
        public string TestSuiteType { get; set; }
        public List<TestSuite> TestSuites { get; set; }
        public string TestsJson { get; set; }
        public int TestsCount { get; set; }
        public int MaxThreads { get; set; }
        public string Application { get; set; }
        public string Attribute { get; set; }
        public string ServerDashboardId { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
