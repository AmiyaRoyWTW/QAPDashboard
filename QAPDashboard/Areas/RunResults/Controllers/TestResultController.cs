using Microsoft.AspNetCore.Mvc;
using QAPDashboard.Areas.RunResults.ViewModels;
using QAPDashboard.Common.Bases;
using QAPDashboard.Common.Interfaces;

namespace QAPDashboard.Areas.RunResults.Controllers
{
    [Area("RunResults")]
    [Route("test-result")]
    public class TestResultController : TestRunnerControllerBase
    {
        private readonly IViewModeBuilder<TestResultViewModel> _testResultViewModelBuilder;

        public TestResultController(IViewModeBuilder<TestResultViewModel> testResultViewModelBuilder)
        {
            _testResultViewModelBuilder = testResultViewModelBuilder;
        }

        [HttpGet, Route("Previous")]
        public IActionResult PreviousTestResult(string testSuite, string testMethod, string baseUrl, string testStartTime)
        {
            AddVMBuilderParameter("buildMode", "previous");
            AddVMBuilderParameter("testSuite", testSuite);
            AddVMBuilderParameter("testMethod", testMethod);
            AddVMBuilderParameter("baseUrl", baseUrl);
            AddVMBuilderParameter("testStartTime", testStartTime);
            return View(_testResultViewModelBuilder.Build(builderParameters));
        }
    }
}
