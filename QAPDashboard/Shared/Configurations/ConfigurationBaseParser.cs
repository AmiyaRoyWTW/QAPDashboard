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
                        Console.WriteLine($"***ERROR::Invalid argument :: {args[n]}");
                    }
                }
                n++;
            }
            ValidateStorageArgs();
        }

        private static bool GetOption(string arg, string value, int pos)
        {
            string option = arg[pos..];

            switch (option.ToLower())
            {
                case "st":
                case "storagetype":
                    RunnerConfiguration.StorageType = value.ToLower();
                    break;
                case "fsp":
                case "filestoragepath":
                    RunnerConfiguration.FileStoragePath = value.ToLower();
                    break;
                case "at":
                case "applicationtype":
                    RunnerConfiguration.ApplicationType = value.ToLower();
                    break;
                case "azsa":
                case "azstorageaccount":
                    RunnerConfiguration.AZStorageAccount = value;
                    break;
                case "azsk":
                case "azstoragekey":
                    RunnerConfiguration.AZStorageKey = value;
                    break;
                case "azsbc":
                case "azstoragescreenshot":
                    RunnerConfiguration.AZStorageScreenShotBlobContainer = value;
                    break;
                case "azst":
                case "azstoragetable":
                    RunnerConfiguration.AZStorageTable = value;
                    break;
                case "ltd":
                case "logtestdata":
                    RunnerConfiguration.LogTestData = true;
                    break;
                case "azstbc":
                case "azstoragetestdata":
                    RunnerConfiguration.AZStorageTestDataBlobContainer = value;
                    break;
                case "lta":
                case "logtestaction":
                    RunnerConfiguration.LogTestAction = true;
                    break;
                case "azsta":
                case "azstoragetestaction":
                    RunnerConfiguration.AZStorageTestDataBlobContainer = value;
                    break;
                case "lurl":
                case "loggingapi":
                    RunnerConfiguration.LoggingAPIUrl = value;
                    break;
                default:
                    return false;
            }

            return true;
        }

        private static int IsOption(string opt)
        {
            if (opt.Length < 2)
            {
                return 0;
            }

            if (opt.StartsWith("--") && IsOptionNameChar(opt[2]))
            {
                return 2;
            }

            return opt[0] == '-' || (opt[0] == '/' && IsOptionNameChar(opt[1])) ? 1 : 0;
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

            if (RunnerConfiguration.AZStorageConnectionString != null)
            {
                var connectionStringParts = RunnerConfiguration.AZStorageConnectionString.Split(";");
                if (connectionStringParts.Length > 2)
                {
                    RunnerConfiguration.AZStorageAccount = connectionStringParts[1].Split("=")[1];
                    RunnerConfiguration.AZStorageKey = connectionStringParts[2].Split("=")[1];
                }
                else
                {
                    throw new ArgumentException("Invalid AZStorageConnectionString format.");
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(RunnerConfiguration.AZStorageConnectionString), "AZStorageConnectionString cannot be null.");
            }

            RunnerConfiguration.AZStorageRunSummaryBlobContainer = "qaprunsummaryblobcontainer";
            RunnerConfiguration.AZStorageExecutedTestsBlobContainer = "qapexecutedtestsblobcontainer";
            RunnerConfiguration.AZStorageTestDataBlobContainer = "qaprunstestdatablobcontainer";

            if (RunnerConfiguration.AZStorageAccount == null || RunnerConfiguration.AZStorageKey == null || RunnerConfiguration.AZStorageRunSummaryBlobContainer == null)
            {
                throw new ArgumentException("Missing arguments for given storage type. Azure storage requires AZStorageAccount, AZStorageKey, AZStorageExecutedTestsBlobContainer, and AZStorageRunSummaryBlobContainer to be set.");
            }

            if (RunnerConfiguration.LogTestData && (RunnerConfiguration.AZStorageTestDataBlobContainer == null || RunnerConfiguration.AZStorageAccount == null))
            {
                throw new ArgumentException("Missing arguments for given storage type and account. Azure Test Data Storage requires AZStorageTestDataBlobContainer to be set and AzStorageAccount.");
            }

            if (RunnerConfiguration.LogTestAction && (RunnerConfiguration.AZStorageTestActionBlobContainer == null || RunnerConfiguration.AZStorageAccount == null))
            {
                throw new ArgumentException("Missing arguments for given storage type and account. Azure Test Action Storage requires AZStorageTestActionBlobContainer to be set and AzStorageAccount.");
            }
        }
    }
}
