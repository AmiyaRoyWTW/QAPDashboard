using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models;
using QAPDashboard.Shared.Utilities;
using System.Text.Json;

namespace QAPDashboard.Shared.Services.TestCaseErrors
{
    public class AzureTestCaseErrorService : ITestCaseErrorService
    {
        public void Update(TestCaseError testCaseError)
        {
            Console.Write(testCaseError.ToString());
        }

        public TestCaseError GetTestCaseError(Guid testRunId, Guid testCaseId)
        {
            string testCaseJson = "";
            try
            {
                if (RunnerConfiguration.AZStorageExecutedTestsBlobContainer == null)
                {
                    throw new ArgumentNullException(nameof(RunnerConfiguration.AZStorageExecutedTestsBlobContainer), "The container name cannot be null.");
                }
                testCaseJson = AzureStorageHelper.GetBlob(testRunId.ToString(), testCaseId.ToString(), RunnerConfiguration.AZStorageExecutedTestsBlobContainer);
            }
            catch (Exception)
            {
                throw;
            }
            if (string.IsNullOrEmpty(testCaseJson))
            {
                throw new InvalidOperationException("The test case JSON cannot be null or empty.");
            }
            TestCase testCase = JsonSerializer.Deserialize<TestCase>(testCaseJson) ?? throw new InvalidOperationException("Deserialization returned null.");

            TestCaseError testCaseError = new()
            {
                TestRunId = testCase.TestRunId,
                TestCaseId = testCase.Id,
                Message = testCase.TestCaseError.Message,
                Trace = testCase.TestCaseError.Trace,
                ErrorType = testCase.TestCaseError.ErrorType,
                Url = testCase.Url,
                PageTitle = testCase.PageTitle,
                ScreenshotBase64 = testCase.TestCaseError.ScreenshotBase64,
            };
            return testCaseError;
        }
    }
}
