using System.Globalization;
using Newtonsoft.Json;
using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models.Twillio;
using QAPDashboard.Shared.Utilities;

namespace QAPDashboard.Shared.Services.TestRuns
{

    public interface ILocalTestRunService
    {
        Runs GetLocalTestRuns();
        Runs GetLocalTestRuns(string testCase = "", string dateRange = "", string startDate = "", string endDate = "");
    }
    public class LocalTestRunService : ILocalTestRunService
    {
        public Runs GetLocalTestRuns()
        {
            Runs runs = new();
            List<Calls> calls = [];
            var resultDirectories = Directory.GetDirectories(RunnerConfiguration.FileStoragePath);
            foreach (var resultDirectory in resultDirectories)
            {
                var runDirectories = Directory.GetDirectories(resultDirectory);
                foreach (var runDirectory in runDirectories)
                {
                    var testStats = GetTestStats(runDirectory);
                    var audioFilePath = $"https://api.twilio.com/2010-04-01/Accounts/{RunnerConfiguration.TwilioAccountSid}/Recordings/RE32153b1d70d795ed930331e1ab0788e7.wav";
                    calls.Add(new Calls
                    {
                        CallId = Path.GetFileName(runDirectory),
                        TestName = Path.GetFileName(resultDirectory),
                        Result = GetTestStatus(runDirectory),
                        Date = testStats?.DateCreated ?? DateTime.MinValue,
                        Duration = $"{(testStats?.Duration ?? 0) / 3600}:{(testStats?.Duration ?? 0) % 3600 / 60}:{(testStats?.Duration ?? 0) % 3600 % 60}",
                        CallingNumber = testStats?.CallingNumber ?? "",
                        CalledNumber = testStats?.CalledNumber ?? "",
                        AudioFile = audioFilePath,
                        AudioFileName = Path.GetFileName(audioFilePath),
                    });
                }
            }

            runs.Calls = [.. calls.OrderByDescending(call => call.Date)];
            runs.TestCases = ["All", .. resultDirectories.Select(Path.GetFileName)];
            WorkflowData.TestCaseFilters = runs.TestCases;
            WorkflowData.SelectedTestCaseFilter = runs.SelectedTestCaseFilter;
            WorkflowData.SelectedDateRangeFilter = runs.SelectedDateRange;
            return runs;
        }

        public Runs GetLocalTestRuns(string testCase = "", string dateRange = "", string startDate = "", string endDate = "")
        {
            DateTime? selectedStartDate = null;
            DateTime? selectedEndDate = null;
            switch (dateRange)
            {
                case "Today":
                    selectedStartDate = DateTime.UtcNow.Date;
                    selectedEndDate = DateTime.UtcNow.Date.AddDays(-1).AddTicks(+1);
                    break;
                case "Last 15 Minuts":
                    selectedStartDate = DateTime.UtcNow.AddMinutes(-15);
                    selectedEndDate = DateTime.UtcNow;
                    break;
                case "Last Hour":
                    selectedStartDate = DateTime.UtcNow.AddHours(-1);
                    selectedEndDate = DateTime.UtcNow;
                    break;
                case "Last 12 Hours":
                    selectedStartDate = DateTime.UtcNow.AddHours(-12);
                    selectedEndDate = DateTime.UtcNow;
                    break;
                case "Last 24 Hours":
                    selectedStartDate = DateTime.UtcNow.AddHours(-24);
                    selectedEndDate = DateTime.UtcNow;
                    break;
                case "This Week":
                    selectedStartDate = DateTime.UtcNow.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
                    selectedEndDate = DateTime.UtcNow;
                    break;
                case "This Month":
                    selectedStartDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                    selectedEndDate = DateTime.UtcNow;
                    break;
                case "Last Calendar Month":
                    selectedStartDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(-1);
                    selectedEndDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddTicks(-1);
                    break;
                case "Custom Range":
                    selectedStartDate = string.IsNullOrEmpty(startDate)
                    ? null
                    : DateTime.ParseExact(startDate, "yyyyMMddHHmmssfffffff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).AddDays(-1).Date;

                    selectedEndDate = string.IsNullOrEmpty(endDate)
                        ? null
                        : DateTime.ParseExact(endDate, "yyyyMMddHHmmssfffffff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).AddDays(1).Date;
                    break;
                default:
                    selectedStartDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                    selectedEndDate = DateTime.Now;
                    break;
            }

            Runs runs = new();
            List<Calls> calls = [];
            string[] resultDirectories = [];
            if (testCase == "All")
            {
                resultDirectories = Directory.GetDirectories(RunnerConfiguration.FileStoragePath);
            }
            else
            {
                resultDirectories = [.. Directory.GetDirectories(RunnerConfiguration.FileStoragePath).Where(x => Path.GetFileName(x) == testCase)];
            }

            foreach (var resultDirectory in resultDirectories)
            {
                var runDirectories = Directory.GetDirectories(resultDirectory);
                foreach (var runDirectory in runDirectories)
                {
                    var testStats = GetTestStats(runDirectory);
                    var audioFilePath = $"https://api.twilio.com/2010-04-01/Accounts/{RunnerConfiguration.TwilioAccountSid}/Recordings/RE32153b1d70d795ed930331e1ab0788e7.wav";
                    if (testStats?.DateCreated > selectedStartDate && testStats.DateCreated < selectedEndDate)
                    {
                        lock (calls)
                        {
                            calls.Add(new Calls
                            {
                                CallId = Path.GetFileName(runDirectory),
                                TestName = Path.GetFileName(resultDirectory),
                                Result = GetTestStatus(runDirectory),
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
            }
            runs.Calls = [.. calls.OrderByDescending(call => call.Date)];
            runs.TestCases = WorkflowData.TestCaseFilters;
            runs.SelectedTestCaseFilter = WorkflowData.SelectedTestCaseFilter;
            runs.SelectedDateRange = WorkflowData.SelectedDateRangeFilter;
            return runs;
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
