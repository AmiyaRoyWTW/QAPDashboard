using QAPDashboard.Common.DTOs;

namespace QAPDashboard.Settings
{
    public interface IApplicationSettingsManager
    {
        NavigationSettingsDTO GetNavigationBarData();
    }
}