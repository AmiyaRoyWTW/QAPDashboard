using QAPDashboard.Common.Bases;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Common.DTOs;
using QAPDashboard.Shared.Models;
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
            string startDate = GetBuilderParameterValue("startDate");
            string endDate = GetBuilderParameterValue("endDate");
            //IEnumerable<string> testRuns = _localTestRunService.GetLocalTestRuns(url, startDate, endDate);
            IEnumerable<Runs> testRuns = _localTestRunService.GetLocalTestRuns(startDate, endDate);

            // Materialize the IEnumerable<TestRun> into a list
            List<Runs> testRunsList = [.. testRuns];

            // Modify the StartTime property of each TestRun
            testRunsList.ForEach(x => x.Date = x.Date.ToLocalTime());

            RunListViewModel runListVM = new()
            {
                Runs = testRunsList
            };

            return runListVM;
        }
    }
}
