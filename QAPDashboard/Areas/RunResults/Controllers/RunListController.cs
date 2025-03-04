using Microsoft.AspNetCore.Mvc;
using QAPDashboard.Common.Bases;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Areas.RunResults.ViewModels;

namespace QAPDashboard.Areas.RunResults.Controllers
{
    [Area("RunResults")]
    [Route("run-list")]
    public class RunListController : TestRunnerControllerBase
    {
        private readonly ILogger<RunListController> _logger;
        private readonly IViewModeBuilder<RunListViewModel> _runListViewModeBuilder;

        public RunListController(IViewModeBuilder<RunListViewModel> runListViewModeBuilder, ILogger<RunListController> logger)
        {
            _runListViewModeBuilder = runListViewModeBuilder;
            _logger = logger;
        }
        [HttpGet, Route("{testCaseName}")]
        public IActionResult? RunList(string testCaseName, DateTime? startDate = null, DateTime? endDate = null, string testCaseFilter = "All", string dateRangeFilter = "This Month")
        {
            try
            {
                AddVMBuilderParameter("testCaseName", testCaseName);
                if (testCaseFilter != "All" || dateRangeFilter != "This Month")
                {
                    AddVMBuilderParameter("testCaseFilter", testCaseFilter);
                    AddVMBuilderParameter("dateRangeFilter", dateRangeFilter);
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        AddVMBuilderParameter("startDate", startDate.Value.ToUniversalTime().AddDays(1).ToString("yyyyMMddHHmmssfffffff"));
                        AddVMBuilderParameter("endDate", endDate.Value.ToUniversalTime().AddDays(1).AddTicks(-1).ToString("yyyyMMddHHmmssfffffff"));
                    }

                }
                RunListViewModel runListViewModel = _runListViewModeBuilder.Build(builderParameters);
                return View(runListViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing the RunList: {Message}", ex.Message);
                return null;
            }
        }
    }
}
