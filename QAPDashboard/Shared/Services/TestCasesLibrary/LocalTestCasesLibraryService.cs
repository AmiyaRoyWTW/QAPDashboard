using Newtonsoft.Json;
using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Models.Twillio;
namespace QAPDashboard.Shared.Services.TestCasesLibrary
{
  public class LocalTestCasesLibraryService : ITestCasesLibraryService
  {
    public string[] GetLocalTestNames()
    {
      var twilioTestManagementFile = RunnerConfiguration.TestInventoryFileStoragePath != null
                ? Directory.GetFiles(RunnerConfiguration.TestInventoryFileStoragePath)
                : [];
      var twilioTests = JsonConvert.DeserializeObject<TwilioTestCase>(File.ReadAllText(twilioTestManagementFile.First()));
      return twilioTests?.Tests?.Select(x => x.TestName).ToArray() ?? [];
    }

    public List<TwilioTestCase> GetTwilioTestCases()
    {
      throw new NotImplementedException();
    }

    public Tests GetTwilioTestCase(string testName)
    {
      var twilioTestManagementFile = RunnerConfiguration.TestInventoryFileStoragePath != null
                ? Directory.GetFiles(RunnerConfiguration.TestInventoryFileStoragePath)
                : [];
      var twilioTests = JsonConvert.DeserializeObject<TwilioTestCase>(File.ReadAllText(twilioTestManagementFile.First()));
      return twilioTests?.Tests?.Where(x => x.TestName.Equals(testName)).FirstOrDefault() ?? new Tests();
    }
  }
}