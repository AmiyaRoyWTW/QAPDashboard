using QAPDashboard.Shared.Models;

namespace QAPDashboard.Shared.Services.TestRunSummaries
{
    public interface ITestRunSummaryService
    {
        void Create(Guid testRunId, TestRunSummary testRunSummary);
        TestRunSummary GetTestRunSummary(Guid testRunId);
        TestRunSummary GetTestRunSummaryFromRunResults(Guid testRunId);
    }
}
