using QAPDashboard.Common.Bases;
using QAPDashboard.Shared.Models;

namespace QAPDashboard.Areas.RunResults.ViewModels
{
    public class OverviewTableReportVM : ViewModelBase
    {
        public List<OverviewTableReportDTO>? RunReports { get; set; }
    }
}
