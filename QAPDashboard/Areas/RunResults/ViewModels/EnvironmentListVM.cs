using QAPDashboard.Common.Bases;
using QAPDashboard.Shared.Models;

namespace QAPDashboard.Areas.RunResults.ViewModels
{
    public class EnvironmentListVM : ViewModelBase
    {
        public List<AzureTableEntry> Environments { get; set; }
    }
}
