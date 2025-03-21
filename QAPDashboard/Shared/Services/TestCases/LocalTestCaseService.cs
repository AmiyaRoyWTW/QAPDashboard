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
            var stepViewModels = new List<StepViewModel>();
            var localTestRunService = new LocalTestRunService();
            var runResultViewModel = new RunResultViewModel();
            var twilioTests = GetTestDetails(testName);
            var twilioSteps = twilioTests?.TestSteps.OrderBy(x => x.Id).ToList();
            var stepFiles = Directory.GetFiles(Path.Combine(RunnerConfiguration.FileStoragePath, testName, testId))
                .Where(x => !(x.EndsWith("call.json") || x.EndsWith("callstatus.json") || x.EndsWith("callresponse.json")));

            if (twilioSteps == null)
            {
                throw new Exception("Test steps not found");
            }

            foreach (var step in twilioSteps)
            {
                var stepFile = stepFiles.FirstOrDefault(x => Path.GetFileName(x).Equals($"{step.StepName.ToLower()}.json", StringComparison.CurrentCultureIgnoreCase));
                if (stepFile != null)
                {
                    try
                    {
                        var stepTranscript = JsonConvert.DeserializeObject<TwillioTranscript>(File.ReadAllText(stepFile)) ?? throw new Exception("Step transcript not found");
                        var transcription = GetStepExpectation(stepTranscript);
                        var stepStatus = step.ExpectedResult.Trim().Equals(transcription.Trim(), StringComparison.OrdinalIgnoreCase)
                            ? "Passed"
                            : (string.IsNullOrEmpty(transcription) ? "DidNotExecuted" : "Failed");

                        stepViewModels.Add(new StepViewModel
                        {
                            StepId = step.Id,
                            StepName = step.StepName,
                            ExpectToHear = step.ExpectedResult,
                            Transcription = transcription,
                            ReplyWith = step.ReplyWith,
                            Status = stepStatus,
                            Confidence = GetStepConfidence(stepTranscript),
                        });
                    }
                    catch (JsonException)
                    {
                        // Handle the exception as needed, e.g., log the error or return a default status
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
            }

            var testStats = localTestRunService.GetTestStats(Path.Combine(RunnerConfiguration.FileStoragePath, testName, testId));
            var runDuration = $"{(testStats?.Duration ?? 0) / 3600}:{(testStats?.Duration ?? 0) % 3600 / 60}:{(testStats?.Duration ?? 0) % 3600 % 60}";

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

            return runResultViewModel ?? new RunResultViewModel();
        }

        private static Tests? GetTestDetails(string testCaseName)
        {
            var twilioTestManagementFiles = RunnerConfiguration.TestInventoryFileStoragePath != null
                ? Directory.GetFiles(RunnerConfiguration.TestInventoryFileStoragePath)
                : [];
            var twilioTestManagementFile = twilioTestManagementFiles.FirstOrDefault(x => Path.GetFileName(x).Equals("TwilioTests.json")) ?? throw new Exception("TwilioTests.json file not found");
            var twilioTests = JsonConvert.DeserializeObject<TwilioTestCase>(File.ReadAllText(twilioTestManagementFile));
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