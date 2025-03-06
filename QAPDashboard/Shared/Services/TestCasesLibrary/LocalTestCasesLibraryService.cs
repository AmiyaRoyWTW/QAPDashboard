using Newtonsoft.Json;
using QAPDashboard.Areas.TestCasesLibrary.Models;
using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models.Twillio;
namespace QAPDashboard.Shared.Services.TestCasesLibrary
{
  public class LocalTestCasesLibraryService : ITestCasesLibraryService
  {
    public string[] GetLocalTestNames()
    {
      var twilioTestManagementFiles = RunnerConfiguration.TestInventoryFileStoragePath != null
                ? Directory.GetFiles(RunnerConfiguration.TestInventoryFileStoragePath)
                : [];
      var twilioTestManagementFile = twilioTestManagementFiles.Where(x => Path.GetFileName(x).Equals("TwilioTests.json"));
      var twilioTests = JsonConvert.DeserializeObject<TwilioTestCase>(File.ReadAllText(twilioTestManagementFile.First()));
      return twilioTests?.Tests?.Select(x => x.TestName).ToArray() ?? [];
    }

    public List<TwilioTestCase> GetTwilioTestCases()
    {
      throw new NotImplementedException();
    }

    public Tests GetTwilioTestCase(string testName)
    {
      var twilioTestManagementFiles = RunnerConfiguration.TestInventoryFileStoragePath != null
                ? Directory.GetFiles(RunnerConfiguration.TestInventoryFileStoragePath)
                : [];
      var twilioTestManagementFile = twilioTestManagementFiles.Where(x => Path.GetFileName(x).Equals("TwilioTests.json"));
      var twilioTests = JsonConvert.DeserializeObject<TwilioTestCase>(File.ReadAllText(twilioTestManagementFile.First()));
      return twilioTests?.Tests?.Where(x => x.TestName.Equals(testName)).FirstOrDefault() ?? new Tests();
    }

    public void UpdateTwilioTestCase(TwilioTestCaseForm testCaseForm)
    {
      var id = 1;
      var twilioTestManagementFiles = RunnerConfiguration.TestInventoryFileStoragePath != null
                ? Directory.GetFiles(RunnerConfiguration.TestInventoryFileStoragePath)
                : [];
      var twilioTestManagementFile = twilioTestManagementFiles.Where(x => Path.GetFileName(x).Equals("TwilioTests.json"));
      var twilioTests = JsonConvert.DeserializeObject<TwilioTestCase>(File.ReadAllText(twilioTestManagementFile.First())) ?? throw new Exception("Test case library find not found");
      var test = twilioTests.Tests.Where(x => x.TestName.Equals(testCaseForm.TestName)).FirstOrDefault();
      var oldTest = twilioTests.Tests.Where(x => x.TestName.Equals(testCaseForm.OldTestName)).FirstOrDefault();
      if (test == null && oldTest == null)
      {
        twilioTests.Tests.Add(new Tests
        {
          TestName = testCaseForm.TestName,
          TestDescription = testCaseForm.TestDescription,
          TestSteps = [.. testCaseForm.TestSteps.Select(x => new Models.Twillio.TestSteps
          {
            Id = id++,
            StepName = x.StepName,
            ExpectedResult = x.ExpectedResult,
            ReplyWith = x.ReplyWith
          })]
        });
      }
      else if (test == null && oldTest != null)
      {
        oldTest.TestName = testCaseForm.TestName;
        oldTest.TestDescription = testCaseForm.TestDescription;
        oldTest.TestSteps = [.. testCaseForm.TestSteps.Select(x => new Models.Twillio.TestSteps
        {
          Id = id++,
          StepName = x.StepName,
          ExpectedResult = x.ExpectedResult,
          ReplyWith = x.ReplyWith
        })];
      }
      else if (test != null && oldTest != null)
      {
        test.TestDescription = testCaseForm.TestDescription;
        test.TestSteps = [.. testCaseForm.TestSteps.Select(x => new Models.Twillio.TestSteps
        {
          Id = id++,
          StepName = x.StepName,
          ExpectedResult = x.ExpectedResult,
          ReplyWith = x.ReplyWith
        })];
      }
      File.WriteAllText(twilioTestManagementFile.First(), JsonConvert.SerializeObject(twilioTests));
    }
  }
}