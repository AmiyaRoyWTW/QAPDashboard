using System.Globalization;
using Newtonsoft.Json;
using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models.Twillio;
using QAPDashboard.Shared.Services.TestRuns;

namespace QAPDashboard.Shared.Services.ExecutionTestList
{
  public class LocalExecutionTestService : IExecutionTestService
  {
    public LocalTestRunService localTestRunService = new();

    public List<ExecutedTests> GetLocalExecutedTests()
    {
      var executedTests = new List<ExecutedTests>();
      var resultDirectories = Directory.GetDirectories(RunnerConfiguration.FileStoragePath);
      foreach (var resultDirectory in resultDirectories)
      {
        var runDirectories = Directory.GetDirectories(resultDirectory);
        var latestRun = GetLatestCallDetails(runDirectories);
        executedTests.Add(new ExecutedTests
        {
          TestName = Path.GetFileName(resultDirectory),
          LastExecutionStatus = GetTestStatus(latestRun.CallId)
        });
      }
      return executedTests;
    }

    public List<ExecutedTests> GetLocalExecutedTests(string dateRange = "", string startDate = "", string endDate = "")
    {
      DateTime? selectedStartDate;
      DateTime? selectedEndDate;
      switch (dateRange)
      {
        case "Today":
          selectedStartDate = DateTime.UtcNow.Date;
          selectedEndDate = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);
          break;
        case "Last 15 Minutes":
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
          selectedEndDate = DateTime.UtcNow;
          break;
      }

      var executedTests = new List<ExecutedTests>();
      var resultDirectories = Directory.GetDirectories(RunnerConfiguration.FileStoragePath);
      foreach (var resultDirectory in resultDirectories)
      {
        var runDirectories = Directory.GetDirectories(resultDirectory);
        var latestRun = GetLatestCallDetails(runDirectories);
        if (latestRun.DateCreated >= selectedStartDate && latestRun.DateCreated <= selectedEndDate)
        {
          executedTests.Add(new ExecutedTests
          {
            TestName = Path.GetFileName(resultDirectory),
            LastExecutionStatus = GetTestStatus(latestRun.CallId)
          });
        }
      }
      return executedTests;
    }

    private CallResponse GetLatestCallDetails(string[] directories)
    {
      var callResponses = new List<CallResponse>();
      foreach (var directory in directories)
      {
        var callResponse = localTestRunService.GetTestStats(directory);
        if (callResponse != null)
        {
          callResponse.CallId = directory;
          callResponses.Add(callResponse);
        }
      }
      return callResponses.OrderByDescending(call => call.DateCreated).FirstOrDefault() ?? new CallResponse();
    }

    public string GetTestStatus(string resultPath)
    {
      var status = "Failed";
      var testResults = Directory.GetFiles(resultPath).FirstOrDefault(x => x.EndsWith("callstatus.json"));
      if (testResults != null)
      {
        try
        {
          var callStatus = JsonConvert.DeserializeObject<CallStatus>(File.ReadAllText(testResults));
          if (callStatus != null)
          {
            status = callStatus.Status.ToLower() switch
            {
              "inprogress" => "In Progress",
              "passed" => "Passed",
              "failed" => "Failed",
              "error" => "Errored",
              _ => "Unknown",
            };
          }
        }
        catch (JsonException)
        {
          throw new Exception("Failed to parse call status");
        }
      }
      return status;
    }
  }
}