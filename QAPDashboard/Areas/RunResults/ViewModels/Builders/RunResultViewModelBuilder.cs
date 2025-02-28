using QAPDashboard.Common.Bases;
using QAPDashboard.Common.DTOs;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models;
using QAPDashboard.Shared.Services.TestCases;
using QAPDashboard.Shared.Services.TestRuns;
using QAPDashboard.Shared.Services.TestRunSummaries;

namespace QAPDashboard.Areas.RunResults.ViewModels.Builders
{
    public class RunResultViewModelBuilder : ViewModelBuilderBase, IViewModeBuilder<RunResultViewModel>
    {
        private readonly ILocalTestCaseService _localTestCaseService;
        public RunResultViewModelBuilder(ILocalTestCaseService localTestCaseService)
        {
            _localTestCaseService = localTestCaseService;
        }
        //private readonly ILocalTestCaseService _localTestCaseService = localTestCaseService;

        public RunResultViewModel Build()
        {
            throw new NotImplementedException();
        }

        public RunResultViewModel Build(List<BuilderParameterDTO> parameters)
        {
            SetBuilderParameters(parameters);
            return CastToRunResultVM(GetBuilderParameterValue("testCaseName"), GetBuilderParameterValue("testRunId"), _localTestCaseService);
        }

        private static RunResultViewModel CastToRunResultVM(string testCaseName, string testRunId, ILocalTestCaseService localTestCaseService)
        {
            return localTestCaseService.GetLocalTestResults(testCaseName, testRunId);
        }
    }
}
