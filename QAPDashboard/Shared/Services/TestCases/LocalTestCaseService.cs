using Newtonsoft.Json;
using QAPDashboard.Areas.RunResults.ViewModels;
using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models.Twillio;
using QAPDashboard.Shared.Services.TestRuns;

namespace QAPDashboard.Shared.Services.TestCases
{

    public interface ILocalTestCaseService
    {
        RunResultViewModel GetLocalTestResults(string testName, string testId);
    }
    public class LocalTestCaseService : ILocalTestCaseService
    {
        public RunResultViewModel GetLocalTestResults(string testName, string testId)
        {
            List<StepViewModel> stepViewModels = [];
            LocalTestRunService localTestRunService = new();
            RunResultViewModel runResultViewModel = new();
            var twilioTests = GetTestDetails(testName);
            var twilioSteps = twilioTests?.TestSteps.OrderBy(x => x.Id).ToList();
            var stepFiles = Directory.GetFiles(Path.Combine(RunnerConfiguration.FileStoragePath, testName, testId)).Where(x => !(x.EndsWith("call.json") || x.EndsWith("callstatus.json") || x.EndsWith("callresponse.json")));
            if (twilioSteps == null)
            {
                throw new Exception("Test steps not found");
            }
            foreach (var step in twilioSteps)
            {
                var stepFile = stepFiles.FirstOrDefault(x => Path.GetFileName(x).ToLower(System.Globalization.CultureInfo.CurrentCulture).Equals(step.StepName.ToLower() + ".json"));
                if (stepFile != null)
                {
                    var stepTranscript = JsonConvert.DeserializeObject<TwillioTranscript>(File.ReadAllText(stepFile)) ?? throw new Exception("Step transcript not found");
                    var transcrition = GetStepExpectation(stepTranscript);
                    var stepStatus = step.ExpectedResult.Trim().Equals(transcrition.Trim(),
                            StringComparison.OrdinalIgnoreCase)
                            ? "Passed"
                            : (string.IsNullOrEmpty(transcrition) ? "DidNotExecuted" : "Failed");
                    if (stepTranscript != null)
                    {
                        stepViewModels.Add(new StepViewModel
                        {
                            StepId = step.Id,
                            StepName = step.StepName,
                            ExpectToHear = step.ExpectedResult,
                            Transcription = GetStepExpectation(stepTranscript),
                            ReplyWith = step.ReplyWith,
                            Status = stepStatus,
                            Confidence = GetStepConfidence(stepTranscript),
                        });
                    }
                }
                else
                {
                    stepViewModels.Add(new StepViewModel
                    {
                        StepId = step.Id,
                        StepName = step.StepName,
                        ExpectToHear = step.ExpectedResult,
                        Transcription = "",
                        ReplyWith = step.ReplyWith,
                        Status = "DidNotExecuted",
                        Confidence = 0
                    });
                }
                var testStats = localTestRunService.GetTestStats(Path.Combine(RunnerConfiguration.FileStoragePath, testName, testId));
                string runDuration = $"{(testStats?.Duration ?? 0) / 3600}:{(testStats?.Duration ?? 0) % 3600 / 60}:{(testStats?.Duration ?? 0) % 3600 % 60}";
                runResultViewModel = new RunResultViewModel
                {
                    RunId = testId,
                    TestName = testName,
                    TestDescription = twilioTests?.TestDescription,
                    RunStatus = localTestRunService.GetTestStatus(Path.Combine(RunnerConfiguration.FileStoragePath, testName, testId)),
                    RunDate = testStats?.DateCreated ?? DateTime.MinValue,
                    DialResult = "Answered",
                    RunDuration = runDuration,
                    Steps = stepViewModels
                };
            }
            return runResultViewModel ?? new RunResultViewModel();
        }

        private static Tests? GetTestDetails(string testCaseName)
        {
            var twilioTestManagementFiles = RunnerConfiguration.TestInventoryFileStoragePath != null
                ? Directory.GetFiles(RunnerConfiguration.TestInventoryFileStoragePath)
                : [];
            var twilioTestManagementFile = twilioTestManagementFiles.Where(x => Path.GetFileName(x).Equals("TwilioTests.json"));
            var twilioTests = JsonConvert.DeserializeObject<TwilioTestCase>(File.ReadAllText(twilioTestManagementFile.First()));
            return twilioTests?.Tests?.FirstOrDefault(x => x.TestName == testCaseName);
        }

        private static string GetStepExpectation(TwillioTranscript stepTranscript)
        {
            return string.Join(" ", stepTranscript.Transcription
                .Select(transcript => transcript.Transcript.Trim()));
        }
        private static double GetStepConfidence(TwillioTranscript stepTranscript)
        {
            var confidences = stepTranscript.Transcription.Select(x => x.Confidence).ToList();
            return Math.Round(Queryable.Average(confidences.AsQueryable()) * 100, 2);
        }
    }
}
