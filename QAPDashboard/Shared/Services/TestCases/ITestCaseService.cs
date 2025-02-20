using QAPDashboard.Shared.Models;

namespace QAPDashboard.Shared.Services.TestCases
{
    public interface ITestCaseService
    {
        Guid CreateTestCase(Guid testRunId, TestSuite testSuite, string testCaseName);
        void EndTestCase(Guid testRunId, Guid testCaseId, int attempts, string status, TestCaseError testCaseError, string testData = "");
        TestCase GetTestCase(Guid testRunId, Guid testCaseId);

        IEnumerable<TestCase> GetExecutedTestCases(Guid testRunId);
        List<TestCase> GetTestCasesInRange(string testSuiteName, string testMethodName, string baseUrl, DateTime currentTestTime);

        void Update(TestCase testCase);
        string GetTestCaseDescription(string testCaseName);
    }
}
