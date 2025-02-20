using QAPDashboard.Shared.Models;

namespace QAPDashboard.Shared.Services.TestRunResults
{
    public interface ITestRunResultService
    {
        TestRunResult GetTestRunResult(Guid testRunId);
    }
}
