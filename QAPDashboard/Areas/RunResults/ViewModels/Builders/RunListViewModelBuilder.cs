using QAPDashboard.Common.Bases;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Common.DTOs;
using QAPDashboard.Shared.Services.TestRuns;
using QAPDashboard.Shared.Models.Twillio;
using QAPDashboard.Shared.Utilities;

namespace QAPDashboard.Areas.RunResults.ViewModels.Builders
{
    public class RunListViewModelBuilder : ViewModelBuilderBase, IViewModeBuilder<RunListViewModel>
    {
        private readonly ILocalTestRunService _localTestRunService;

        public RunListViewModelBuilder(ILocalTestRunService localTestRunService)
        {
            _localTestRunService = localTestRunService;
        }

        public RunListViewModel Build()
        {
            RunListViewModel runListVM = new()
            {
                Runs = _localTestRunService.GetLocalTestRuns(),
            };
            return runListVM;
        }

        public RunListViewModel Build(List<BuilderParameterDTO> builderParameters)
        {
            SetBuilderParameters(builderParameters);
            string url = GetBuilderParameterValue("url");
            string testCaseFilter = GetBuilderParameterValue("testCaseFilter");
            string dateRangeFilter = GetBuilderParameterValue("dateRangeFilter");
            string startDate = GetBuilderParameterValue("startDate");
            string endDate = GetBuilderParameterValue("endDate");
            //WorkflowData.SelectedTestCaseFilter = testCaseFilter;
            //WorkflowData.SelectedDateRangeFilter = dateRangeFilter;
            Runs testRuns = _localTestRunService.GetLocalTestRuns(testCaseFilter, dateRangeFilter, startDate, endDate);

            // Materialize the IEnumerable<TestRun> into a list
            //List<Runs> testRunsList = [.. testRuns];

            // Modify the StartTime property of each TestRun
            testRuns.Calls.ForEach(x => x.Date = x.Date.ToLocalTime());

            RunListViewModel runListVM = new()
            {
                Runs = testRuns
            };

            return runListVM;
        }
    }
}
