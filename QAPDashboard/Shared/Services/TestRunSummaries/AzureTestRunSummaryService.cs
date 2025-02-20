using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models;
using QAPDashboard.Shared.Services.TestRunResults;
using QAPDashboard.Shared.Services.TestRuns;
using QAPDashboard.Shared.Utilities;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace QAPDashboard.Shared.Services.TestRunSummaries
{
    public class AzureTestRunSummaryService : ITestRunSummaryService
    {
        public void Create(Guid testRunId, TestRunSummary testRunSummary)
        {
            AzureStorageHelper.PutBlob(testRunId.ToString(), "summary", JsonSerializer.Serialize(testRunSummary), RunnerConfiguration.AZStorageRunSummaryBlobContainer);
        }

        public TestRunSummary GetTestRunSummary(Guid testRunId)
        {
            string lastModifiedTestFile = GetLastModifiedRunOrSummaryFile(testRunId);

            return lastModifiedTestFile switch
            {
                "summary" => GetTestRunSummaryFromSummaryResult(testRunId.ToString()),
                "run" => GetTestRunSummaryFromRunResults(testRunId),
                _ => throw new InvalidOperationException("Unknown test file type"),
            };
        }


        public string GetLastModifiedRunOrSummaryFile(Guid testRunId)
        {
            var allBlobs = AzureStorageHelper.GetAllBlobs(RunnerConfiguration.AZStorageRunSummaryBlobContainer);
            var latestBlob = allBlobs.OrderByDescending(x => x.Properties.LastModified).FirstOrDefault();
            if (latestBlob != null)
            {
                if (latestBlob.Name.EndsWith("run"))
                {
                    return "run";
                }
                else
                {
                    return "summary";
                }
            }
            return string.Empty;
        }

        public TestRunSummary GetTestRunSummaryFromRunResults(Guid testRunId)
        {
            AzureTestRunService azureTestRunService = new();
            AZStorageRunResultsService azureRunResultService = new();

            TestRun testRun = azureRunResultService.GetTestRun(testRunId);
            TestRunResult testRunResult = azureRunResultService.GetTestRunResult(testRunId);

            bool isReleaseId = Regex.IsMatch(testRun.Labels, "rid_\\d+");
            string runReport = $"/Report/run/{testRunId}";

            TestRunSummary testRunSummary = new()
            {
                Application = testRun.Application,
                TestSuites = testRun.TestSuites,
                TestSuiteType = testRun.TestSuiteType,
                BaseUrl = testRun.BaseUrl,
                Browser = testRun.Browser,
                TestRunId = testRunId,
                Threads = testRun.MaxThreads,
                TestCasesCount = testRun.TestSuites.SelectMany(testSuite => testSuite.TestCases).Count(),
                StartTime = testRun.StartTime.ToLocalTime(),
                EndTime = testRun.EndTime.ToLocalTime(),
                IsReleaseId = isReleaseId,
                RunReportUrl = runReport,
                PassedTestCases = testRunResult.PassedTestCases,
                PassedCount = testRunResult.PassedTestCases.Count,
                FailedTestCases = testRunResult.FailedTestCases,
                FailedCount = testRunResult.FailedTestCases.Count,
                FailedTestCasesByMessage = testRunResult.FailedTestCasesByMessage,
                BlockedTestCases = testRunResult.BlockedTestCases,
                BlockedCount = testRunResult.BlockedTestCases.Count,
                StartedTestCases = testRunResult.StartedTestCases,
                StartedCount = testRunResult.StartedTestCases.Count,
                QueuedTestCases = testRunResult.QueuedTestCases,
                QueuedCount = testRunResult.QueuedTestCases.Count(),
                NonPassedTestCaseQueryStrings = testRunResult.NonPassedTestCaseQueryStrings,
                DashboardUrl = testRun.ServerDashboardId != null
                    ? $"http://automation.grpeapp.com/run-result/{testRun.ServerDashboardId}"
                    : null,
            };

            return testRunSummary;
        }

        public TestRunSummary GetTestRunSummaryFromSummaryResult(string testRunId)
        {
            string summaryJson = AzureStorageHelper.GetBlob(testRunId, "summary", RunnerConfiguration.AZStorageRunSummaryBlobContainer);
            TestRunSummary summaryVM = JsonSerializer.Deserialize<TestRunSummary>(summaryJson);
            return summaryVM;
        }
    }
}
