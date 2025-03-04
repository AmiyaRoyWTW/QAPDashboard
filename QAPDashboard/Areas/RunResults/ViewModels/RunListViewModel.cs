using QAPDashboard.Common.Bases;
using QAPDashboard.Shared.Models.Twillio;

namespace QAPDashboard.Areas.RunResults.ViewModels
{
    public class RunListViewModel : ViewModelBase
    {
        public required string TestCaseName { get; set; }
        public required Runs Runs { get; set; }
    }
}
