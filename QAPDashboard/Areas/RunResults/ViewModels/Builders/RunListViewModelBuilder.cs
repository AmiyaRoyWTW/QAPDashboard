using QAPDashboard.Common.Bases;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Common.DTOs;
using QAPDashboard.Shared.Services.TestRuns;
using QAPDashboard.Shared.Models.Twillio;

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
            throw new NotImplementedException();
        }

        public RunListViewModel Build(List<BuilderParameterDTO> builderParameters)
        {
            Runs testRuns;
            SetBuilderParameters(builderParameters);
            string testCaseName = GetBuilderParameterValue("testCaseName");
            string testCaseFilter = GetBuilderParameterValue("testCaseFilter");
            string dateRangeFilter = GetBuilderParameterValue("dateRangeFilter");
            string startDate = GetBuilderParameterValue("startDate");
            string endDate = GetBuilderParameterValue("endDate");
            if (testCaseFilter != null && dateRangeFilter != null && startDate != null && endDate != null)
            {
                testRuns = _localTestRunService.GetLocalTestRuns(testCaseName, dateRangeFilter, startDate, endDate);
            }
            else testRuns = _localTestRunService.GetLocalTestRuns(testCaseName);

            // Materialize the IEnumerable<TestRun> into a list
            //List<Runs> testRunsList = [.. testRuns];

            // Modify the StartTime property of each TestRun
            testRuns.Calls.ForEach(x => x.Date = x.Date.ToLocalTime());

            RunListViewModel runListVM = new()
            {
                TestCaseName = testCaseName,
                Runs = testRuns
            };

            return runListVM;
        }
    }
}
