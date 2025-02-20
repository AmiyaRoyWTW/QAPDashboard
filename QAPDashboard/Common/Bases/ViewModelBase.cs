using QAPDashboard.Shared.Configurations;
using QAPDashboard.Areas.Settings.Models;
using QAPDashboard.Common.DTOs;

namespace QAPDashboard.Common.Bases
{
    public class ViewModelBase
    {
        public ViewModelBase()
        {
            NavigationBar = UIRunnerConfiguration.NavigationBar;
            AppType = RunnerConfiguration.ApplicationType;
        }

        public List<NavigationBarDTO> NavigationBar { get; set; }
        public string AppType { get; set; }
    }
}
