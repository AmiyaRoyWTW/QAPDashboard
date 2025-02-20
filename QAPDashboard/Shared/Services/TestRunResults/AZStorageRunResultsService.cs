using QAPDashboard.Shared.Models;
using QAPDashboard.Shared.Extensions;
using QAPDashboard.Shared.Services.TestCases;
using QAPDashboard.Shared.Services.TestRuns;
using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Utilities;
using System.Text.Json;

namespace QAPDashboard.Shared.Services.TestRunResults
{
    public class AZStorageRunResultsService : ITestRunResultService
    {
        public TestRun GetTestRun(Guid testRunId)
        {
            var runJson = AzureStorageHelper.GetBlob(testRunId.ToString(), "run", RunnerConfiguration.AZStorageRunSummaryBlobContainer);
            TestRun testRun = JsonSerializer.Deserialize<TestRun>(runJson);
            return testRun;
        }
        public TestRunResult GetTestRunResult(Guid testRunId)
        {
            AzureTestRunService azureTestRunService = new();
            AzureTestCaseService azureTestCaseService = new();
            TestRun testRun = azureTestRunService.GetTestRun(testRunId);
            IEnumerable<TestCase> executedTestCases = azureTestCaseService.GetExecutedTestCases(testRunId);
            List<TestCase> startedTestCases = executedTestCases.GetStarted();
            List<TestCase> blockedTestCases = executedTestCases.GetBlocked();
            List<TestCase> failedTestCases = executedTestCases.GetFailed();
            List<TestCase> passedTestCases = executedTestCases.GetPassed();
            List<TestCase> queuedTestCases = executedTestCases.GetQueued(testRun.TestSuites);

            Dictionary<string, List<TestCase>> failedTestCasesByMessage = executedTestCases.Where(x => x.Status == "Failed").GroupBy(testCase => testCase.GetGeneralizedErrorMessage()).ToDictionary(group => group.Key, group => group.ToList());
            List<string> nonPassedTestCaseQueryStrings = GetNonPassedTestCaseQueryStrings(
                    failedTestCases,
                    queuedTestCases,
                    startedTestCases,
                    executedTestCases);

            TestRunResult testRunResult = new()
            {
                StartedTestCases = startedTestCases,
                BlockedTestCases = blockedTestCases,
                FailedTestCases = failedTestCases,
                FailedTestCasesByMessage = failedTestCasesByMessage,
                PassedTestCases = passedTestCases,
                NonPassedTestCaseQueryStrings = nonPassedTestCaseQueryStrings,
                QueuedTestCases = queuedTestCases,
                EndTime = testRun.EndTime.ToUniversalTime().ToString("o")
            };
            return testRunResult;
        }

        public static List<string> GetNonPassedTestCaseQueryStrings(List<TestCase> failedTestCases, List<TestCase> queuedTestCases, List<TestCase> startedTestCases, IEnumerable<TestCase> executedTestCases)
        {
            // Add failed test cases
            List<string> nonPassedTestCaseQueryStrings = failedTestCases.Select(testCase => testCase.GetTestCaseQueryString()).ToList();

            // Add queued test cases if they have been executed
            foreach (TestCase queuedTestCase in queuedTestCases)
            {
                TestCase matchingTestCase = executedTestCases.FirstOrDefault(executedTestCase => queuedTestCase == executedTestCase);
                if (matchingTestCase != null)
                {
                    nonPassedTestCaseQueryStrings.Add(matchingTestCase.GetTestCaseQueryString());
                }
            }

            // Add started test cases
            nonPassedTestCaseQueryStrings.AddRange(startedTestCases.Select(testCase => testCase.GetTestCaseQueryString()));

            return nonPassedTestCaseQueryStrings;
        }
    }
}
