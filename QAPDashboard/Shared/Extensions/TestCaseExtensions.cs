using QAPDashboard.Shared.Models;
using System.Text.RegularExpressions;

namespace QAPDashboard.Shared.Extensions
{
    public static class TestCaseExtensions
    {
        public static List<TestCase> GetStarted(this IEnumerable<TestCase> testCases)
        {
            return testCases.Where(x => x.Status == "Started").ToList();
        }
        public static List<TestCase> GetBlocked(this IEnumerable<TestCase> testCases)
        {
            return testCases.Where(x => x.Status == "Blocked").OrderBy(x => x.StartTime).ToList();
        }
        public static List<TestCase> GetPassed(this IEnumerable<TestCase> testCases)
        {
            return testCases.Where(x => x.Status == "Passed").OrderBy(x => x.StartTime).ToList();
        }
        public static List<TestCase> GetFailed(this IEnumerable<TestCase> testCases)
        {
            return testCases.Where(x => x.Status == "Failed").ToList();
        }

        public static List<TestCase> GetQueued(this IEnumerable<TestCase> executedTestCases, IEnumerable<TestSuite> testRunTestSuites)
        {
            List<TestCase> queuedCases = [];

            foreach (TestSuite testSuite in testRunTestSuites)
            {
                foreach (TestCase testCase in testSuite.TestCases)
                {
                    if (!executedTestCases.Any(executedCase =>
                        executedCase.Name == testCase.Name &&
                        executedCase.TestSuite.Name == testSuite.Name))
                    {
                        queuedCases.Add(testCase);
                    }
                }
            }
            return queuedCases;
        }

        public static string GetTestCaseQueryString(this TestCase testCase)
        {
            return $"testSuiteName={testCase.TestSuite.Name}&testCaseName={testCase.Name}&testCaseDescription={testCase.Description}";
        }
        public static string GetGeneralizedErrorMessage(this TestCase testCase)
        {
            string? errorMessage = testCase?.TestCaseError?.Message;

            if (testCase?.TestCaseError == null)
            {
                return "No error found in this test case";
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = Regex.Replace(errorMessage, "http://localhost:.*/session/.*/", "http://localhost:*/session/*/", RegexOptions.Multiline);
                errorMessage = Regex.Replace(errorMessage, "'Test.*'", "'Test*", RegexOptions.Multiline);
                errorMessage = Regex.Replace(errorMessage, "\\(\\d*\\)", "(*, *)", RegexOptions.Multiline);
                errorMessage = Regex.Replace(errorMessage, "\\(Session info(.|[\r\n])*", "", RegexOptions.Multiline);
            }
            else
            {
                errorMessage = "No error message available!";
            }

            return errorMessage;
        }
    }
}
