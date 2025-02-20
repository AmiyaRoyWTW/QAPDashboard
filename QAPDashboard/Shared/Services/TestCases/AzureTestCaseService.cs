using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models;
using QAPDashboard.Shared.Utilities;
using System.Text.Json;

namespace QAPDashboard.Shared.Services.TestCases
{
    public class AzureTestCaseService : ITestCaseService
    {
        private string placeHolder = "";
        public Guid StartTestCase(Guid testRunId, TestSuite testSuite, string name)
        {
            Guid testCaseId = Guid.NewGuid();
            TestCase testCase = new()
            {
                Id = testCaseId,
                TestSuite = testSuite,
                Name = name,
                StartTime = DateTime.Now,
                Status = "Started",
                PartitionKey = testRunId.ToString(),
                RowKey = testCaseId.ToString(),
            };
            AzureStorageHelper.PutBlob(testRunId.ToString(), testCaseId.ToString(), JsonSerializer.Serialize(testCase), RunnerConfiguration.AZStorageExecutedTestsBlobContainer);
            return testCaseId;
        }

        public void EndTestCase(Guid testRunId, Guid testCaseId, int attempts, string status, TestCaseError testCaseError, string testData = "")
        {

            string testJson = AzureStorageHelper.GetBlob(testRunId.ToString(), testCaseId.ToString(), RunnerConfiguration.AZStorageExecutedTestsBlobContainer);
            TestCase executedTestCase = JsonSerializer.Deserialize<TestCase>(testJson);

            executedTestCase.Attempts = attempts;
            executedTestCase.EndTime = DateTime.Now;
            executedTestCase.Status = status;
            executedTestCase.Description = GetTestCaseDescription(executedTestCase.Name);
            executedTestCase.TestCaseError = testCaseError;

            if (status.ToLower() == "failed" && !string.IsNullOrEmpty(testData))
            {
                AzureStorageHelper.PutBlob(testRunId.ToString(), testCaseId.ToString(), testData, RunnerConfiguration.AZStorageTestDataBlobContainer);
            }

            AzureStorageHelper.PutBlob(testRunId.ToString(), testCaseId.ToString(), JsonSerializer.Serialize(executedTestCase), RunnerConfiguration.AZStorageExecutedTestsBlobContainer);
        }

        public TestCase GetTestCase(Guid testRunId, Guid testCaseId)
        {
            var testJson = AzureStorageHelper.GetBlob($"{testRunId.ToString()}", $"{testCaseId.ToString()}", RunnerConfiguration.AZStorageExecutedTestsBlobContainer);
            return JsonSerializer.Deserialize<TestCase>(testJson);
        }

        public IEnumerable<TestCase> GetExecutedTestCases(Guid testRunId)
        {
            List<TestCase> executedTestCases = new();
            var testsdataBlobs = AzureStorageHelper.GetAllBlobs(RunnerConfiguration.AZStorageExecutedTestsBlobContainer);
            var executedTestsBlob = testsdataBlobs.Where(x => x.Name.StartsWith($"{testRunId.ToString()}/"));

            foreach (var blob in executedTestsBlob)
            {
                string testJson = AzureStorageHelper.GetBlob(testRunId.ToString(), blob.Name.Split("/")[1], RunnerConfiguration.AZStorageExecutedTestsBlobContainer);
                executedTestCases.Add(JsonSerializer.Deserialize<TestCase>(testJson));
            }

            return executedTestCases;
        }

        public List<TestCase> GetTestCasesInRange(string testSuiteName, string testMethodName, string baseUrl, DateTime currentTestTime)
        {
            throw new NotImplementedException();
        }

        public Guid CreateTestCase(Guid testRunId, TestSuite testSuite, string testCaseName)
        {
            Guid testCaseId = Guid.NewGuid();
            TestCase testCase = new()
            {
                Id = testCaseId,
                TestSuite = testSuite,
                Name = testCaseName,
                StartTime = DateTime.Now,
                Status = "Started",
                TestCaseError = new TestCaseError(),
                PartitionKey = testRunId.ToString(),
                RowKey = testCaseId.ToString(),
            };
            AzureStorageHelper.PutBlob(testRunId.ToString(), testCaseId.ToString(), JsonSerializer.Serialize(testCase), RunnerConfiguration.AZStorageExecutedTestsBlobContainer);
            return testCaseId;
        }

        public void Update(TestCase testCase)
        {
            throw new NotImplementedException();
        }

        public string GetTestCaseDescription(string testCaseName)
        {
            string solutionPath = PathUtility.GetSolutionPath();
            List<string> testAssemblyPaths = PathUtility.GetTestAssemblyPaths(solutionPath);

            return testAssemblyPaths
                .Select(testAssemblyPath =>
                {
                    string docPath = testAssemblyPath.Replace(".dll", ".xml");
                    return XMLUtility.GetTestDescriptionFromXmlFile(docPath, testCaseName);
                })
                .FirstOrDefault(description => !string.IsNullOrEmpty(description)) ?? "NO DOCUMENTATION FOR THIS TEST!";
        }
    }
}
