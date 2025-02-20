using QAPDashboard.Shared.Configurations;

namespace QAPDashboard
{
    public class UIRunnerConfigurationParser : IParseRunnerConfiguration
    {
        public bool GetOption(string arg, string value, int pos)
        {
            string option = arg[pos..];

            if (option is "h" or "allowedhosts")
            {
                RunnerConfiguration.AllowedHosts = value.ToLower();
            }
            else if (option is "p" or "dashboardport")
            {
                RunnerConfiguration.DashboardPort = Convert.ToInt32(value);
            }
            else if (option == "azdevopskey")
            {
                RunnerConfiguration.AZDevOpsAPIKey = value;
            }
            else
            {
                return false;
            }

            return true;
        }

        public void LoadDefaultOptions(IConfiguration configuration)
        {
            RunnerConfiguration.AllowedHosts = "localhost;enforcers1.liazon.corp;automation.grpeapp.com";
            RunnerConfiguration.StorageType = "local";
            RunnerConfiguration.FileStoragePath = "c:\\TwillioTestResults";
            RunnerConfiguration.TestInventoryFileStoragePath = string.Format("{0}TestInventory", AppDomain.CurrentDomain.BaseDirectory);
            RunnerConfiguration.DashboardPort = 5000;
            RunnerConfiguration.LoggingAPIUrl = "http://automation.grpeapp.com";
            RunnerConfiguration.ApplicationType = "local";
            //RunnerConfiguration.AZStorageConnectionString = configuration["AzureBlobStorage:ConnectionString"];
        }
    }
}
