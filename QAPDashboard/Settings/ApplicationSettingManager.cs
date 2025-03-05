using Newtonsoft.Json;
using System.Reflection;
using QAPDashboard.Areas.Settings.Models;
using QAPDashboard.Common.DTOs;

namespace QAPDashboard.Settings
{
    public class ApplicationSettingManager : IApplicationSettingsManager
    {
        private readonly ApplicationSettingsDTO _applicationSettingsDTO;

        private const string ApplicationConfigFileName = "applicationConfig";

        public ApplicationSettingManager()
        {
            string configText = LoadConfigFile(ApplicationConfigFileName);

            _applicationSettingsDTO = JsonConvert.DeserializeObject<ApplicationSettingsDTO>(configText) ?? new ApplicationSettingsDTO();
            UIRunnerConfiguration.NavigationBar = GetNavigationBarData().NavigationBar;
        }

        private static string LoadConfigFile(string fileName)
        {
            string configText = "No File Found";
            string? applicationResult = Assembly.GetEntryAssembly()?.Location != null
                ? Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)
                : string.Empty;
            string filePath = Path.Combine(applicationResult ?? string.Empty, $"{fileName}.json");

            if (File.Exists(filePath))
            {
                configText = File.ReadAllText(filePath);
            }

            return configText;
        }

        public NavigationSettingsDTO GetNavigationBarData()
        {
            return _applicationSettingsDTO.NavigationSettings ?? new NavigationSettingsDTO();
        }
    }
}
