using QAPDashboard.Common.Bases;
using QAPDashboard.Common.DTOs;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models;
using QAPDashboard.Shared.Services.TestCaseErrors;
using QAPDashboard.Shared.Services.TestCases;
using System.Text.RegularExpressions;

namespace QAPDashboard.Areas.RunResults.ViewModels.Builders
{
    public class TestResultViewModelBuilder : ViewModelBuilderBase, IViewModeBuilder<TestResultViewModel>
    {
        private readonly ITestCaseService _testCaseService;
        private readonly ITestCaseErrorService _testCaseErrorService;

        public TestResultViewModelBuilder(ITestCaseService testCaseService, ITestCaseErrorService testCaseErrorService)
        {
            _testCaseService = testCaseService;
            _testCaseErrorService = testCaseErrorService;
        }

        public TestResultViewModel Build()
        {
            throw new NotImplementedException();
        }

        public TestResultViewModel Build(List<BuilderParameterDTO> parameters)
        {
            SetBuilderParameters(parameters);
            string buildMode = GetBuilderParameterValue("buildMode") ?? string.Empty;

            switch (buildMode)
            {
                case "previous":
                    string testSuite = GetBuilderParameterValue("testSuite") ?? string.Empty;
                    string testMethod = GetBuilderParameterValue("testMethod") ?? string.Empty;
                    string baseUrl = GetBuilderParameterValue("baseUrl") ?? string.Empty;
                    string testStartTime = GetBuilderParameterValue("testStartTime") ?? string.Empty;
                    return BuildFromPreviousResults(testSuite, testMethod, baseUrl, DateTime.Parse(testStartTime));
                case "byId":
                    string testRunId = GetBuilderParameterValue("testRunId") ?? string.Empty;
                    string testId = GetBuilderParameterValue("testId") ?? string.Empty;
                    return BuildFromTestId(testRunId, testId);
                default:
                    throw new ArgumentException($"Invalid buildMode: {buildMode}");
            }
        }

        public TestResultViewModel BuildFromTestId(string testRunId, string testId)
        {
            TestCase testCase = _testCaseService.GetTestCase(Guid.Parse(testRunId), Guid.Parse(testId));

            TestResultViewModel testResultViewModel = new()
            {
                Attempts = testCase.Attempts,
                EndTime = testCase.EndTime.ToLocalTime(),
                Assembly = testCase.TestSuite.Assembly,
                TestSuiteNamespace = testCase.TestSuite.Namespace,
                TestSuite = testCase.TestSuite,
                Method = testCase.Name,
                RunId = testRunId.ToString(),
                StartTime = testCase.StartTime.ToLocalTime(),
                Status = testCase.Status,
                TestId = testId.ToString(),
                BaseUrl = testCase.Url,
                ResultList = null,
            };

            if (!string.Equals(testCase.Status, "passed", StringComparison.OrdinalIgnoreCase))
            {
                TestCaseError testCaseError = _testCaseErrorService.GetTestCaseError(Guid.Parse(testRunId), Guid.Parse(testId));
                string traceStr = testCaseError.Trace;

                if (testCaseError.ErrorType != null)
                {
                    testResultViewModel.ErrorType = testCaseError.ErrorType;
                    testResultViewModel.Message = testCaseError.Message;
                    testResultViewModel.PageTitle = testCaseError.PageTitle;
                    testResultViewModel.ScreenshotBase64 = testCaseError.ScreenshotBase64;
                    testResultViewModel.Url = testCaseError.Url;
                    traceStr = testCaseError.Trace;
                }

                string stackTraceRegexPattern = @"at.*(?:Framework.*(?:PageObject|Roles|PageObjects|Actions)|Tests|Utility).*";
                List<string> matches = Regex.Matches(traceStr, stackTraceRegexPattern)
                    .Cast<Match>()
                    .Select(match => match.Value)
                    .ToList();

                if (matches.Count == 0)
                {
                    stackTraceRegexPattern = @"(at.*)";
                    matches = Regex.Matches(traceStr, stackTraceRegexPattern)
                        .Cast<Match>()
                        .Select(match => match.Value)
                        .ToList();
                }
                testResultViewModel.Trace = matches;
            }

            testResultViewModel.AppType = RunnerConfiguration.ApplicationType ?? "defaultAppType";

            return testResultViewModel;
        }

        public TestResultViewModel BuildFromPreviousResults(string testSuite, string testMethod, string baseUrl, DateTime testStartTime)
        {
            List<TestCase> lastTestCases = _testCaseService.GetTestCasesInRange(testSuite, testMethod, baseUrl, testStartTime);
            return new TestResultViewModel
            {
                ResultList = lastTestCases
            };
        }
    }
}
