using Microsoft.AspNetCore.Mvc;
using QAPDashboard.Areas.RunResults.ViewModels;
using QAPDashboard.Common.Bases;
using QAPDashboard.Common.Interfaces;

namespace QAPDashboard.Areas.RunResults.Controllers
{
  [Area("RunResults")]
  [Route("execution-list")]
  public class ExecutedTestController : TestRunnerControllerBase
  {
    private readonly ILogger<ExecutedTestController> _logger;
    private readonly IViewModeBuilder<ExecutedTestViewModel> _executionTestListViewModeBuilder;

    public ExecutedTestController(IViewModeBuilder<ExecutedTestViewModel> executionTestListViewModeBuilder, ILogger<ExecutedTestController> logger)
    {
      _executionTestListViewModeBuilder = executionTestListViewModeBuilder;
      _logger = logger;
    }

    [HttpGet, Route("")]
    public IActionResult? ExecutionTestList(DateTime? startDate = null, DateTime? endDate = null, string dateRangeFilter = "This Month")
    {
      try
      {
        if (dateRangeFilter != "This Month")
        {
          AddVMBuilderParameter("dateRangeFilter", dateRangeFilter);
          if (startDate.HasValue && endDate.HasValue)
          {
            AddVMBuilderParameter("startDate", startDate.Value.ToUniversalTime().AddDays(1).ToString("yyyyMMddHHmmssfffffff"));
            AddVMBuilderParameter("endDate", endDate.Value.ToUniversalTime().AddDays(1).AddTicks(-1).ToString("yyyyMMddHHmmssfffffff"));
          }
          ExecutedTestViewModel executionListListViewModel = _executionTestListViewModeBuilder.Build(builderParameters);
          return View(executionListListViewModel);
        }
        else
        {
          // Handle the case where only the "url" parameter is provided.
          // Build the view model as you did in the original code.

          ExecutedTestViewModel executionListListViewModel = _executionTestListViewModeBuilder.Build();
          return View(executionListListViewModel);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError("An error occurred while processing the RunList: {Message}", ex.Message);
        return null;
      }
    }

  }
}