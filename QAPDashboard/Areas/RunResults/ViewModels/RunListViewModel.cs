using QAPDashboard.Common.Bases;
using QAPDashboard.Shared.Models;
using QAPDashboard.Shared.Models.Twillio;

namespace QAPDashboard.Areas.RunResults.ViewModels
{
    public class RunListViewModel : ViewModelBase
    {
        //public IEnumerable<TestRun> Runs { get; set; }
        public required Runs Runs { get; set; }
    }
}
