using QAPDashboard.Common.Bases;
using QAPDashboard.Shared.Models;

namespace QAPDashboard.Areas.RunResults.ViewModels
{
    public class TestResultViewModel : ViewModelBase
    {
        public string TestId { get; set; }
        public string RunId { get; set; }
        public string Assembly { get; set; }
        public string TestSuiteNamespace { get; set; }
        public TestSuite TestSuite { get; set; }
        public string Method { get; set; }
        public int Attempts { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public List<string> Trace { get; set; }
        public string Url { get; set; }
        public string BaseUrl { get; set; }
        public string PageTitle { get; set; }
        public string ScreenshotBase64 { get; set; }
        public string ErrorType { get; set; }
        public List<DataTree> DataTrees { get; set; }
        public string DataTreeHtml { get; set; }
        public string Name => $"Test Suite: {TestSuite}, Test: {Method}"; // $"Project: {Assembly}, TestSuite: {Fixture}, Test: {Method}";
        public List<TestCase> ResultList { get; set; }
    }
}
