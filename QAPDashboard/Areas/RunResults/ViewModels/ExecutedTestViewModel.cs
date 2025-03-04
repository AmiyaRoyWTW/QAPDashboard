using QAPDashboard.Common.Bases;
using QAPDashboard.Shared.Models.Twillio;

namespace QAPDashboard.Areas.RunResults.ViewModels
{
  public class ExecutedTestViewModel : ViewModelBase
  {
    public List<string> DateRanges { get; set; } = ["Last 15 Minutes", "Last Hour", "Last 12 Hours", "Last 24 Hours", "This Week", "This Month", "Last Calendar Month", "Custom Range"];
    public string SelectedDateRange { get; set; } = "This Month";
    public string? CustomStartDate { get; set; }
    public string? CustomEndDate { get; set; }
    public required List<ExecutedTests> ExecutedTests { get; set; }
  }
}