using QAPDashboard.Shared.Models.Twillio;

namespace QAPDashboard.Shared.Services.ExecutionTestList
{
  public interface IExecutionTestService
  {
    List<ExecutedTests> GetLocalExecutedTests();
    List<ExecutedTests> GetLocalExecutedTests(string dateRange = "", string startDate = "", string endDate = "");
  }
}