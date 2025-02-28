using Microsoft.AspNetCore.Mvc;
using QAPDashboard.Common.Bases;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Areas.RunResults.ViewModels;

namespace QAPDashboard.Areas.RunResults.Controllers
{
    [Area("RunResults")]
    //[Route("run-list")]
    [Route("")]
    public class RunListController : TestRunnerControllerBase
    {
        private readonly ILogger<RunListController> _logger;
        private readonly IViewModeBuilder<RunListViewModel> _runListViewModeBuilder;

        public RunListController(IViewModeBuilder<RunListViewModel> runListViewModeBuilder, ILogger<RunListController> logger)
        {
            _runListViewModeBuilder = runListViewModeBuilder;
            _logger = logger;
        }
        [HttpGet, Route("")]
        public IActionResult? RunList(string url, DateTime? startDate = null, DateTime? endDate = null, string testCaseFilter = "All", string dateRangeFilter = "This Month")
        {
            try
            {
                if (testCaseFilter != "All" || dateRangeFilter != "This Month")
                {
                    AddVMBuilderParameter("testCaseFilter", testCaseFilter);
                    AddVMBuilderParameter("dateRangeFilter", dateRangeFilter);
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        AddVMBuilderParameter("startDate", startDate.Value.ToUniversalTime().ToString("yyyyMMddHHmmssfffffff"));
                        AddVMBuilderParameter("endDate", endDate.Value.ToUniversalTime().AddDays(1).AddTicks(-1).ToString("yyyyMMddHHmmssfffffff"));
                    }
                    RunListViewModel runListViewModel = _runListViewModeBuilder.Build(builderParameters);
                    return View(runListViewModel);
                }
                else
                {
                    // Handle the case where only the "url" parameter is provided.
                    // Build the view model as you did in the original code.

                    RunListViewModel runListViewModel = _runListViewModeBuilder.Build();
                    return View(runListViewModel);
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
