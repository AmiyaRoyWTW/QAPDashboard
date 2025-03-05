using QAPDashboard.Common.Bases;
using QAPDashboard.Shared.Models.Twillio;

namespace QAPDashboard.Areas.TestCasesLibrary.ViewModels
{
  public class TestCaseManagementViewModel : ViewModelBase
  {
    public required Tests TwilioTestCase { get; set; }
  }
}