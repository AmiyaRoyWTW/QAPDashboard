namespace QAPDashboard.Areas.TestCasesLibrary.Models
{

  public class TwilioTestCaseForm
  {
    public string OldTestName { get; set; } = string.Empty;
    public string TestName { get; set; } = string.Empty;
    public string TestDescription { get; set; } = string.Empty;
    public List<TestSteps> TestSteps { get; set; } = [];

  }

  public class TestSteps
  {
    public string? Id { get; set; }
    public string StepName { get; set; } = string.Empty;
    public string ExpectedResult { get; set; } = string.Empty;
    public string ReplyWith { get; set; } = "N/A";
  }
}
