namespace QAPDashboard.Shared.Models.Twillio
{
    public class TwilioTestCase
    {
        public List<Tests> Tests { get; set; } = [];
    }

    public class Tests
    {
        public string TestName { get; set; } = string.Empty;
        public string TestDescription { get; set; } = string.Empty;
        public List<TestSteps> TestSteps { get; set; } = [];

    }

    public class TestSteps
    {
        public int Id { get; set; }
        public string StepName { get; set; } = string.Empty;
        public string ExpectedResult { get; set; } = string.Empty;
        public string ReplyWith { get; set; } = "N/A";
    }
}
