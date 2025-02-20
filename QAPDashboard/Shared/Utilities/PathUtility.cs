using QAPDashboard.Shared.Configurations;
using System.Reflection;

namespace QAPDashboard.Shared.Utilities
{
    public static class PathUtility
    {
        public static string GetFileStoragePath => RunnerConfiguration.FileStoragePath;
        public static string GetTestCaseFilename => "test";
        public static string GetTestRunFilename => "run";
        public static string GetTestCaseErrorFilename => "error";
        public static string GetTestRunSummaryFilename => "summary";
        public static string GetTestActionsFilename => "actions";
        public static string GetTestDataFilename => "testData";
        public static string GetTestRunJsonFilename => $"{GetTestRunFilename}.json";
        public static string GetTestRunSummaryJsonFilename => $"{GetTestRunSummaryFilename}.json";
        public static string getConfigJsonFilename => "TestRunners/TestRunner.Shared/config.json";

        public static string GetTestRunJsonPath(Guid testRunId)
        {
            return Path.Combine(RunnerConfiguration.FileStoragePath, testRunId.ToString(), GetTestRunJsonFilename);
        }

        public static string GetTestRunDirectory(Guid testRunId)
        {
            return Path.Combine(RunnerConfiguration.FileStoragePath, testRunId.ToString());
        }

        public static string GetTestCaseJsonPath(Guid testRunId, Guid testCaseId)
        {
            return Path.Combine(RunnerConfiguration.FileStoragePath, testRunId.ToString(), $"{testCaseId}_{GetTestCaseFilename}.json");
        }

        public static string GetTestCaseErrorJsonPath(Guid testRunId, Guid testCaseId)
        {
            return Path.Combine(RunnerConfiguration.FileStoragePath, testRunId.ToString(), $"{testCaseId}_{GetTestCaseErrorFilename}.json");
        }

        public static string GetTestActionsJsonPath(Guid testRunId, Guid testCaseId)
        {
            return Path.Combine(RunnerConfiguration.FileStoragePath, testRunId.ToString(), $"{testCaseId}_{GetTestActionsFilename}.json");
        }

        public static string GetTestDataJsonPath(Guid testRunId, Guid testCaseId)
        {
            return Path.Combine(RunnerConfiguration.FileStoragePath, testRunId.ToString(), $"{testCaseId}_{GetTestDataFilename}.json");
        }

        public static List<string> GetTestFilesList(Guid testRunId)
        {
            return Directory.GetFiles(Path.Combine(RunnerConfiguration.FileStoragePath, testRunId.ToString()), $"*{GetTestCaseFilename}.json").ToList();
        }

        public static string GetSolutionPath()
        {
            string startupPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            DirectoryInfo directory = new(startupPath);

            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            return directory?.FullName;
        }

        public static List<string> GetTestAssemblyPaths(string solutionPath)
        {
            return Directory.GetFiles(solutionPath, "*.dll", SearchOption.AllDirectories)
                            .Where(path => path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") &&
                                           Path.GetFileNameWithoutExtension(path).ToLower().Contains("tests"))
                            .ToList();
        }

        public static bool IsValidPath(string path)
        {

            string fullPath = Path.GetFullPath(path);

            // Ensure that the full path is not empty or null
            if (string.IsNullOrEmpty(fullPath))
            {
                return false;
            }

            // Ensure that the full path is not just a root directory (e.g., "C:\")
            if (Path.GetPathRoot(fullPath) == fullPath)
            {
                return false;
            }

            // Check if the full path contains invalid characters
            bool containsInvalidChars = Array.Exists(Path.GetInvalidPathChars(), fullPath.Contains);
            if (containsInvalidChars)
            {
                return false;
            }

            // Check if the full path exceeds the maximum length allowed
            const int MAX_PATH_LENGTH = 260; // Max length for a path in Windows
            if (fullPath.Length > MAX_PATH_LENGTH)
            {
                return false;
            }

            // Check if the directory part of the full path exists
            string directory = Path.GetDirectoryName(fullPath);
            return Directory.Exists(directory);
        }

        public static string GetConfigDetailsPath()
        {
            return Path.Combine(GetSolutionPath(), getConfigJsonFilename);
        }
    }
}
