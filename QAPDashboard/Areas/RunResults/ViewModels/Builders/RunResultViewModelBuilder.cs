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
            RunResultViewModel runResultViewModel;
            LocalTestRunService localTestRunService = new();
            List<StepViewModel> stepViewModels = [];
            var steps = localTestCaseService.GetLocalTestSteps(testCaseName, testRunId);
            foreach (var step in steps)
            {
                stepViewModels.Add(new StepViewModel
                {
                    StepId = step.StepId,
                    StepName = step.StepName,
                    ExpectToHear = step.ExpectToHear,
                    ReplyWith = step.ReplyWith,
                    DetailedResult = step.DetailedResult,
                    Status = step.Status,
                    Confidence = step.Confidence,
                });
            }
            var testStats = localTestRunService.GetTestStats(Path.Combine(RunnerConfiguration.FileStoragePath, testCaseName, testRunId));
            string runDuration = $"{(testStats?.Duration ?? 0) / 3600}:{(testStats?.Duration ?? 0) % 3600 / 60}:{(testStats?.Duration ?? 0) % 3600 % 60}";
            runResultViewModel = new RunResultViewModel
            {
                RunId = testRunId,
                RunStatus = localTestRunService.GetTestStatus(Path.Combine(RunnerConfiguration.FileStoragePath, testCaseName, testRunId)),
                RunDate = testStats?.DateCreated ?? DateTime.MinValue,
                DialResult = "Answered",
                RunDuration = runDuration,
                Steps = stepViewModels
            };
            return runResultViewModel;
        }
    }
}
