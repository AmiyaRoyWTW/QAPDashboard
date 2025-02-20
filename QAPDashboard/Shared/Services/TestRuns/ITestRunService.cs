using QAPDashboard.Shared.Models;

namespace QAPDashboard.Shared.Services.TestRuns
{
    public interface ITestRunService
    {
        Guid CreateTestRun(TestRun testRun);
        Guid CreateTestRun(string url, string application, string attribute, string testSuiteType, string browser, int threads, string labels, List<string> testCaseQueryStrings);
        void EndTestRun(Guid testRunId);
        TestRun GetTestRun(Guid testRunId);
        List<TestRun> GetTestRuns();
        List<TestRun> GetTestRuns(string baseUrl = "", string startDate = "", string endDate = "");
        void UpdateTestRun(TestRun testRun);
        void UpdateTestRunEndTimeToNow(TestRun testRun);
    }
}
