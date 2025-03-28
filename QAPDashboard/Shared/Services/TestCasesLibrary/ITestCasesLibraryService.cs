using QAPDashboard.Areas.TestCasesLibrary.Models;
using QAPDashboard.Shared.Models.Twillio;

namespace QAPDashboard.Shared.Services.TestCasesLibrary
{
  public interface ITestCasesLibraryService
  {
    string[] GetLocalTestNames();
    List<TwilioTestCase> GetTwilioTestCases();
    Tests GetTwilioTestCase(string testName);
    void UpdateTwilioTestCase(TwilioTestCaseForm testCaseForm);
  }
}
