using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models;
using QAPDashboard.Shared.Utilities;
using System.Collections.Specialized;
using System.Text.Json;
using System.Web;

namespace QAPDashboard.Shared.Services.TestRuns
{
    public class AzureTestRunService : ITestRunService
    {
        private string placeHolder = "";
        private TestRun TestRun { get; set; }
        private List<TestCase> TestCases { get; set; }

        public Guid CreateTestRun(TestRun testRun)
        {
            throw new NotImplementedException();
        }

        public Guid CreateTestRun(string url, string application, string attribute, string testSuiteType, string browser, int threads, string labels, List<string> testCaseQueryStrings)
        {
            Guid testRunId = Guid.NewGuid();

            List<TestSuite> testSuites = [];

            foreach (string testCaseQueryString in testCaseQueryStrings)
            {
                NameValueCollection parsedQuery = HttpUtility.ParseQueryString(testCaseQueryString);

                if (testSuites.Any(testSuite => testSuite.Name == parsedQuery["testSuiteName"]))
                {
                    TestSuite existingTestSuite = testSuites.FirstOrDefault(testSuite => testSuite.Name == parsedQuery["testSuiteName"]);
                    existingTestSuite?.TestCases.Add(new TestCase() { Name = parsedQuery["testCaseName"] });
                }
                else
                {
                    testSuites.Add(new TestSuite()
                    {
                        Name = parsedQuery["testSuiteName"],
                        Assembly = parsedQuery["testSuiteAssembly"],
                        Namespace = parsedQuery["testSuiteNamespace"],
                        Framework = parsedQuery["testSuiteFramework"],
                        Type = parsedQuery["testSuiteType"],
                        TestCases =
                        [
                            new TestCase() {
                                Name =  parsedQuery["testCaseName"],
                                Description = parsedQuery["testCaseDescription"]
                            }
                        ]
                    }); ;
                };
            }

            TestRun testRun = new()
            {
                Id = testRunId,
                Application = application,
                Attribute = attribute,
                TestSuiteType = testSuiteType,
                BaseUrl = url,
                Browser = browser,
                UserName = Environment.UserName,
                StartTime = DateTime.Now,
                MaxThreads = threads,
                Labels = labels,
                TestSuites = testSuites,
                PartitionKey = testRunId.ToString(),
                RowKey = "run",
            };
            AzureStorageHelper.PutBlob($"{testRunId.ToString()}", "run", JsonSerializer.Serialize(testRun), RunnerConfiguration.AZStorageRunSummaryBlobContainer);
            AzureStorageHelper.PutBlob($"{testRunId.ToString()}", "testList", JsonSerializer.Serialize(testCaseQueryStrings), RunnerConfiguration.AZStorageTestDataBlobContainer);
            return testRunId;
        }

        public void EndTestRun(Guid testRunId)
        {
            var runJson = AzureStorageHelper.GetBlob($"{testRunId.ToString()}", "run", RunnerConfiguration.AZStorageRunSummaryBlobContainer);
            TestRun testRun = JsonSerializer.Deserialize<TestRun>(runJson);
            if (testRun == null)
            {
                throw new Exception($"Test run with ID {testRunId} not found.");
            }

            testRun.EndTime = DateTime.Now;

            AzureStorageHelper.PutBlob($"{testRunId.ToString()}", "run", JsonSerializer.Serialize(testRun), RunnerConfiguration.AZStorageRunSummaryBlobContainer);
        }

        public TestRun GetTestRun(Guid testRunId)
        {
            var runJson = AzureStorageHelper.GetBlob($"{testRunId.ToString()}", "run", RunnerConfiguration.AZStorageRunSummaryBlobContainer);
            TestRun testRun = JsonSerializer.Deserialize<TestRun>(runJson);
            return testRun;
        }

        public List<TestRun> GetTestRuns()
        {
            return GetTestRuns(RunnerConfiguration.AZStorageRunSummaryBlobContainer);
        }

        public List<TestRun> GetTestRuns(string containerName)
        {
            List<TestRun> testRuns = new List<TestRun>();
            List<string> testRunsJson = AzureStorageHelper.GetAllBlobsContent("run", containerName);
            foreach (string testRunJson in testRunsJson)
            {
                testRuns.Add(JsonSerializer.Deserialize<TestRun>(testRunJson));
            }
            return testRuns;
        }

        public List<TestRun> GetTestRuns(string baseUrl = "", string startDate = "", string endDate = "")
        {
            throw new NotImplementedException();
        }

        public void UpdateTestRun(TestRun testRun)
        {
            throw new NotImplementedException();
        }

        public void UpdateTestRunEndTimeToNow(TestRun testRun)
        {
            throw new NotImplementedException();
        }        
    }
}
