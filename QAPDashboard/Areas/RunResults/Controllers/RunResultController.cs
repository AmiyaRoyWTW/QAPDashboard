using Microsoft.AspNetCore.Mvc;
using QAPDashboard.Areas.RunResults.ViewModels;
using QAPDashboard.Common.Bases;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Shared.Services.TestRunResults;
using QAPDashboard.Shared.Utilities;
using System.ComponentModel.DataAnnotations;

namespace QAPDashboard.Areas.RunResults.Controllers
{
    [Area("RunResults")]
    [Route("run-result")]
    public class RunResultController : TestRunnerControllerBase
    {
        private readonly IViewModeBuilder<RunResultViewModel> _runResultViewModeBuilder;
        //private readonly IViewModeBuilder<TestResultViewModel> _testResultViewModelBuilder;
        //private readonly ITestRunResultService _testRunResultService;
        private readonly ILogger<RunListController> _logger;

        public RunResultController(IViewModeBuilder<RunResultViewModel> runResultViewModeBuilder, /*IViewModeBuilder<TestResultViewModel> testResultViewModelBuilder, ITestRunResultService testRunResultService,*/ ILogger<RunListController> logger)
        {
            _runResultViewModeBuilder = runResultViewModeBuilder;
            //_testResultViewModelBuilder = testResultViewModelBuilder;
            //_testRunResultService = testRunResultService;
            _logger = logger;
        }

        [HttpGet, Route("{testCaseName}/{testRunId}")]
        public IActionResult? RunResult(string testCaseName, string testRunId)
        {
            try
            {
                AddVMBuilderParameter("testCaseName", testCaseName);
                AddVMBuilderParameter("testRunId", testRunId);
                _logger.LogInformation("Coming inside RunResult");
                return View(_runResultViewModeBuilder.Build(builderParameters));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing the RunList: {Message}", ex.Message);
                return null;
            }
        }

        /*[HttpGet, Route("{testRunId}/Json")]
        public IActionResult? RunResultJson(Guid testRunId)
        {
            try
            {
                _logger.LogInformation("Coming inside RunResult Json");
                dynamic jsonRunResults = _testRunResultService.GetTestRunResult(testRunId);
                return Json(jsonRunResults);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing the RunList: {Message}", ex.Message);
                return null;
            }
        }*/
        [HttpGet, Route("{testRunId}/submit")]
        public IActionResult PushRunResult(Guid testRunId)
        {
            if (!IdValidator.IsValidTestRunId(testRunId))
            {
                return RedirectToAction("Error", "Home");
            }
            //RESTDashboardTestRunExecutionStorageService testOrchestrationService = new();
            //testOrchestrationService.SendFileSystemResultsToServer(testRunId, RunnerConfiguration.FileStoragePath);
            string? redirectUrl = Url.Action("RunResult", new { testRunId });
            if (redirectUrl == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return Redirect(redirectUrl);
        }


        /*[HttpGet, Route("{testRunId}/test-result/{testId}")]
        public IActionResult TestResult(string testRunId, string testId)
        {
            AddVMBuilderParameter("buildMode", "byId");
            AddVMBuilderParameter("testRunId", testRunId);
            AddVMBuilderParameter("testId", testId);
            return View(_testResultViewModelBuilder.Build(builderParameters));
        }*/
    }
}
