using QAPDashboard.Common.Bases;

namespace QAPDashboard.Areas.TestCasesLibrary.ViewModels
{
  public class TestCasesLibraryViewModel : ViewModelBase
  {
    public required string[] TwilioTests { get; set; }
  }
}