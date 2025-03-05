using Microsoft.AspNetCore.Mvc;
using QAPDashboard.Areas.TestCasesLibrary.ViewModels;
using QAPDashboard.Common.Bases;
using QAPDashboard.Common.Interfaces;

namespace QAPDashboard.Areas.TestCasesLibrary.Controllers
{
  [Area("TestCasesLibrary")]
  [Route("test-library")]
  public class TestCasesLibraryController : TestRunnerControllerBase
  {
    private readonly ILogger<TestCasesLibraryController> _logger;
    private readonly IViewModeBuilder<TestCasesLibraryViewModel> _testCasesLibraryViewModelModeBuilder;

    public TestCasesLibraryController(IViewModeBuilder<TestCasesLibraryViewModel> testCasesLibraryViewModelModeBuilder, ILogger<TestCasesLibraryController> logger)
    {
      _testCasesLibraryViewModelModeBuilder = testCasesLibraryViewModelModeBuilder;
      _logger = logger;
    }

    [HttpGet, Route("")]
    public IActionResult? TestCasesLibrary()
    {
      try
      {
        TestCasesLibraryViewModel executionListListViewModel = _testCasesLibraryViewModelModeBuilder.Build();
        return View(executionListListViewModel);
      }
      catch (Exception ex)
      {
        _logger.LogError("An error occurred while processing the RunList: {Message}", ex.Message);
        return null;
      }
    }
  }
}