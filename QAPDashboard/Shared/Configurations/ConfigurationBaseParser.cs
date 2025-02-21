using Microsoft.Extensions.Configuration;

namespace QAPDashboard.Shared.Configurations
{
    public class ConfigurationBaseParser
    {
        private readonly IParseRunnerConfiguration RunnerConfigurationParser;
        public IConfiguration Configuration { get; }
        public ConfigurationBaseParser(IParseRunnerConfiguration runnerConfigurationParser, IConfiguration configuration)
        {
            RunnerConfigurationParser = runnerConfigurationParser;
            Configuration = configuration;
        }

        public void GetConfigurationArguments(string[] args)
        {
            int n = 0;

            RunnerConfigurationParser.LoadDefaultOptions(Configuration);

            while (n < args.Length)
            {
                int pos = IsOption(args[n]);
                if (pos > 0)
                {
                    string value = "";
                    if (args.GetUpperBound(0) > n && IsOption(args[n + 1]) == 0)
                    {
                        value = args[n + 1];
                    }
                    if (!GetOption(args[n], value, pos) && !RunnerConfigurationParser.GetOption(args[n], value, pos))
                    {
                        Console.WriteLine("***ERROR::Invalid argument :: {0}", args[n]);
                    }
                }
                n++;
            }
            ValidateStorageArgs();
        }

        private static bool GetOption(string arg, string value, int pos)
        {
            string option = arg[pos..];

            if (option is "st" or "storagetype")
            {
                RunnerConfiguration.StorageType = value.ToLower();
            }
            else if (option is "fsp" or "filestoragepath")
            {
                RunnerConfiguration.FileStoragePath = value.ToLower();
            }
            else if (option is "at" or "applicationtype")
            {
                RunnerConfiguration.ApplicationType = value.ToLower();
            }
            else if (option is "azsa" or "azstorageaccount")
            {
                RunnerConfiguration.AZStorageAccount = value;
            }
            else if (option is "azsk" or "azstoragekey")
            {
                RunnerConfiguration.AZStorageKey = value;
            }
            else if (option is "azsbc" or "azstoragescreenshot")
            {
                RunnerConfiguration.AZStorageScreenShotBlobContainer = value;
            }
            else if (option is "azst" or "azstoragetable")
            {
                RunnerConfiguration.AZStorageTable = value;
            }
            else if (option is "ltd" or "logtestdata")
            {
                RunnerConfiguration.LogTestData = true;
            }
            else if (option is "azstbc" or "azstoragetestdata")
            {
                RunnerConfiguration.AZStorageTestDataBlobContainer = value;
            }
            else if (option is "lta" or "logtestaction")
            {
                RunnerConfiguration.LogTestAction = true;
            }
            else if (option is "azsta" or "azstoragetestaction")
            {
                RunnerConfiguration.AZStorageTestDataBlobContainer = value;
            }
            else if (option is "lurl" or "loggingapi")
            {
                RunnerConfiguration.LoggingAPIUrl = value;
            }
            else
            {
                return false;
            }

            return true;
        }

        private static int IsOption(string opt)
        {
            char[] c;
            if (opt.Length < 2)
            {
                return 0;
            }
            else if (opt.Length > 2)
            {
                c = opt.ToCharArray(0, 3);
                if (c[0] == '-' && c[1] == '-' && IsOptionNameChar(c[2]))
                {
                    return 2;
                }
            }
            else
            {
                c = opt.ToCharArray(0, 2);
            }
            return c[0] == '-' || (c[0] == '/' && IsOptionNameChar(c[1])) ? 1 : 0;
        }

        private static bool IsOptionNameChar(char c)
        {
            return char.IsLetterOrDigit(c) || c == '?';
        }

        private static void ValidateStorageArgs()
        {
            if (RunnerConfiguration.StorageType == "local")
            {
                return;
            }
            RunnerConfiguration.AZStorageAccount = RunnerConfiguration.AZStorageConnectionString.Split(";")[1].Split("=")[1];
            RunnerConfiguration.AZStorageKey = RunnerConfiguration.AZStorageConnectionString.Split(";")[2].Split("=")[1];
            RunnerConfiguration.AZStorageRunSummaryBlobContainer = "qaprunsummaryblobcontainer";
            RunnerConfiguration.AZStorageExecutedTestsBlobContainer = "qapexecutedtestsblobcontainer";
            RunnerConfiguration.AZStorageTestDataBlobContainer = "qaprunstestdatablobcontainer";
            if (RunnerConfiguration.AZStorageAccount == null || RunnerConfiguration.AZStorageKey == null || RunnerConfiguration.AZStorageRunSummaryBlobContainer == null)
            {
                throw new ArgumentException($"Missing arguments for given storage type. Azure storage requires AZStorageAccount, AZStorageKey, AZStorageExecutedTestsBlobContainer, and AZStorageRunSummaryBlobContainer to be set.");
            }

            if (RunnerConfiguration.LogTestData == true && (RunnerConfiguration.AZStorageTestDataBlobContainer == null || RunnerConfiguration.AZStorageAccount == null))
            {
                throw new ArgumentException($"Missing arguments for given storage type and account. Azure Test Data Storage requires AZStorageTestDataBlobContainer to be set and AzStorageAccount.");
            }

            if (RunnerConfiguration.LogTestAction == true && (RunnerConfiguration.AZStorageTestActionBlobContainer == null || RunnerConfiguration.AZStorageAccount == null))
            {
                throw new ArgumentException($"Missing arguments for given storage type and account. Azure Test Action Storage requires AZStorageTestActionBlobContainer to be set and AzStorageAccount.");
            }
        }
    }
}
