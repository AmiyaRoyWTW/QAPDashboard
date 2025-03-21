using Microsoft.AspNetCore.Mvc;
using QAPDashboard.Areas.TestCasesLibrary.Models;
using QAPDashboard.Areas.TestCasesLibrary.ViewModels;
using QAPDashboard.Common.Bases;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Shared.Services.TestCasesLibrary;

namespace QAPDashboard.Areas.TestCasesLibrary.Controllers
{
  [Area("TestCasesLibrary")]
  [Route("test-management")]
  public class TestCaseManagementController : TestRunnerControllerBase
  {
    private readonly ILogger<TestCaseManagementController> _logger;
    private readonly IViewModeBuilder<TestCaseManagementViewModel> _testManagementViewModelModeBuilder;
    private readonly ITestCasesLibraryService _testCasesLibraryService;

    public TestCaseManagementController(IViewModeBuilder<TestCaseManagementViewModel> testManagementViewModelModeBuilder, ITestCasesLibraryService testCasesLibraryService, ILogger<TestCaseManagementController> logger)
    {
      _testManagementViewModelModeBuilder = testManagementViewModelModeBuilder;
      _testCasesLibraryService = testCasesLibraryService;
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

    [HttpPost, Route("test-submit"), RequestFormLimits(ValueCountLimit = 5000)]
    public IActionResult? TestCaseUpdate(TwilioTestCaseForm twilioTestCaseForm)
    {
      try
      {
        _testCasesLibraryService.UpdateTwilioTestCase(twilioTestCaseForm);
        Thread.Sleep(500);
        return LocalRedirect("/test-management/" + twilioTestCaseForm.TestName);
      }
      catch (Exception ex)
      {
        _logger.LogError("An error occurred while processing the RunList: {Message}", ex.Message);
        return null;
      }
    }
  }
}