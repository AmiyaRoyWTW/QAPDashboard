using Microsoft.AspNetCore.Mvc;
using QAPDashboard.Areas.TestCasesLibrary.ViewModels;
using QAPDashboard.Common.Bases;
using QAPDashboard.Common.Interfaces;

namespace QAPDashboard.Areas.TestCasesLibrary.Controllers
{
  [Area("TestCasesLibrary")]
  [Route("test-management")]
  public class TestCaseManagementController : TestRunnerControllerBase
  {
    private readonly ILogger<TestCaseManagementController> _logger;
    private readonly IViewModeBuilder<TestCaseManagementViewModel> _testManagementViewModelModeBuilder;

    public TestCaseManagementController(IViewModeBuilder<TestCaseManagementViewModel> testManagementViewModelModeBuilder, ILogger<TestCaseManagementController> logger)
    {
      _testManagementViewModelModeBuilder = testManagementViewModelModeBuilder;
      _logger = logger;
    }

    [HttpGet, Route("{testCaseName}")]
    public IActionResult? TestCaseManagement(string testCaseName)
    {
      try
      {
        AddVMBuilderParameter("testCaseName", testCaseName);
        TestCaseManagementViewModel testCaseManagementViewModel = _testManagementViewModelModeBuilder.Build(builderParameters);
        return View(testCaseManagementViewModel);
      }
      catch (Exception ex)
      {
        _logger.LogError("An error occurred while processing the RunList: {Message}", ex.Message);
        return null;
      }
    }
  }
}