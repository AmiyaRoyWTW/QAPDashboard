namespace QAPDashboard.Shared.Models
{
    public class TestSuite
    {
        public int Id { get; set; }
        public string Assembly { get; set; }
        public string Namespace { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Framework { get; set; }
        public string Type { get; set; }
        public List<TestCase> TestCases { get; set; }
    }
}
