using System.Globalization;
using Newtonsoft.Json;
using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models.Twillio;

namespace QAPDashboard.Shared.Services.TestRuns
{

    public interface ILocalTestRunService
    {
        List<Runs> GetLocalTestRuns();
        List<Runs> GetLocalTestRuns(string startDate = "", string endDate = "");
    }
    public class LocalTestRunService : ILocalTestRunService
    {
        public List<Runs> GetLocalTestRuns()
        {
            List<Runs> runs = [];
            var resultDirectories = Directory.GetDirectories(RunnerConfiguration.FileStoragePath);
            foreach (var resultDirectory in resultDirectories)
            {
                var testStats = GetTestStats(resultDirectory);
                var audioFilePath = $"https://api.twilio.com/2010-04-01/Accounts/{RunnerConfiguration.TwilioAccountSid}/Recordings/RE32153b1d70d795ed930331e1ab0788e7.wav";
                runs.Add(new Runs
                {
                    RunName = resultDirectory.Split("\\").ToList().Last(),
                    Result = GetTestStatus(resultDirectory),
                    Date = testStats?.DateCreated ?? DateTime.MinValue,
                    Duration = $"{(testStats?.Duration ?? 0) / 3600}:{(testStats?.Duration ?? 0) % 3600 / 60}:{(testStats?.Duration ?? 0) % 3600 % 60}",
                    CallingNumber = testStats?.CallingNumber ?? "",
                    CalledNumber = testStats?.CalledNumber ?? "",
                    AudioFile = audioFilePath,
                    AudioFileName = Path.GetFileName(audioFilePath),
                });
            }
            return runs;
        }

        public List<Runs> GetLocalTestRuns(string startDate = "", string endDate = "")
        {
            DateTime? parsedStartDate = string.IsNullOrEmpty(startDate)
                ? null
                : DateTime.ParseExact(startDate, "yyyyMMddHHmmssfffffff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).AddDays(-1).Date;

            DateTime? parsedEndDate = string.IsNullOrEmpty(endDate)
                ? null
                : DateTime.ParseExact(endDate, "yyyyMMddHHmmssfffffff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).AddDays(1).Date;
            List<Runs> runs = [];
            var resultDirectories = Directory.GetDirectories(RunnerConfiguration.FileStoragePath);
            foreach (var resultDirectory in resultDirectories)
            {
                var testStats = GetTestStats(resultDirectory);
                var audioFilePath = $"https://api.twilio.com/2010-04-01/Accounts/{RunnerConfiguration.TwilioAccountSid}/Recordings/RE32153b1d70d795ed930331e1ab0788e7.wav";
                if (testStats?.DateCreated > parsedStartDate && testStats.DateCreated < parsedEndDate)
                {
                    lock (runs)
                    {
                        runs.Add(new Runs
                        {
                            RunName = resultDirectory.Split("\\").ToList().Last(),
                            Result = GetTestStatus(resultDirectory),
                            Date = testStats?.DateCreated ?? DateTime.MinValue,
                            Duration = $"{(testStats?.Duration ?? 0) / 3600}:{(testStats?.Duration ?? 0) % 3600 / 60}:{(testStats?.Duration ?? 0) % 3600 % 60}",
                            CallingNumber = testStats?.CallingNumber ?? "",
                            CalledNumber = testStats?.CalledNumber ?? "",
                            AudioFile = audioFilePath,
                            AudioFileName = Path.GetFileName(audioFilePath),
                        });
                    }
                }
            }
            return [.. runs.OrderByDescending(run => run.Date)];
        }

        public string GetTestStatus(string resultPath)
        {
            string status = "Failed";
            var testResults = Directory.GetFiles(resultPath).Where(x => x.EndsWith("callstatus.json")).FirstOrDefault();
            if (testResults != null)
            {
                var callStatus = JsonConvert.DeserializeObject<CallStatus>(File.ReadAllText(testResults));
                if (callStatus != null)
                {
                    status = callStatus.Status.ToLower() switch
                    {
                        "inprogress" => "In Progress",
                        "passed" => "Success",
                        "failed" => "Failed",
                        "error" => "Errored",
                        _ => "Unknown",
                    };
                }
            }
            return status;
        }

        public CallResponse? GetTestStats(string resultPath)
        {
            CallResponse? callResponseStats = null;
            var callResponse = Directory.GetFiles(resultPath).Where(x => x.EndsWith("callresponse.json")).FirstOrDefault();
            if (callResponse != null)
            {
                callResponseStats = JsonConvert.DeserializeObject<CallResponse>(File.ReadAllText(callResponse));
            }
            return callResponseStats;
        }
    }
}
