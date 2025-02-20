using Newtonsoft.Json;
using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models.Twillio;

namespace QAPDashboard.Shared.Services.TestCases
{

    public interface ILocalTestCaseService
    {
        List<TestStepStats> GetLocalTestSteps(string testId);
    }
    public class LocalTestCaseService : ILocalTestCaseService
    {
        public List<TestStepStats> GetLocalTestSteps(string testId)
        {
            int stepCount = 1;
            List<TestStepStats> testStepStats = [];
            var stepFiles = Directory.GetFiles(Path.Combine(RunnerConfiguration.FileStoragePath, testId)).Where(x => !(x.EndsWith("call.json") || x.EndsWith("callstatus.json") || x.EndsWith("callresponse.json")));
            foreach (var stepFile in stepFiles)
            {
                var stepTranscript = JsonConvert.DeserializeObject<TwillioTranscript>(File.ReadAllText(stepFile));
                if (stepTranscript != null)
                {
                    testStepStats.Add(new TestStepStats
                    {
                        StepId = stepCount++,
                        StepName = Path.GetFileNameWithoutExtension(stepFile),
                        ExpectToHear = GetStepExpectaion(stepTranscript),
                        Status = stepTranscript.Status,
                        Confidence = GetStepConfidence(stepTranscript),
                    });
                }
            }
            return testStepStats;
        }

        private static string GetStepExpectaion(TwillioTranscript stepTranscript)
        {
            var expectToHear = String.Empty;
            foreach( var transcript in stepTranscript.Transcription)
            {
                expectToHear += transcript.Transcript + " ";
            }
            return expectToHear;
        }
        private static double GetStepConfidence(TwillioTranscript stepTranscript)
        {
            var confidences = stepTranscript.Transcription.Select(x => x.Confidence).ToList();
            return Math.Round(Queryable.Average(confidences.AsQueryable())*100, 2);
        }
    }
}
