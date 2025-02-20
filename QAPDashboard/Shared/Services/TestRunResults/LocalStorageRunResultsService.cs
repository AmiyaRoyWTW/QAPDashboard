using QAPDashboard.Shared.Configurations;

namespace QAPDashboard.Shared.Services.TestRunResults
{
    public class LocalStorageRunResultsService : ILocalRunResultService
    {
        public List<string> GetTwillioRuns()
        {
            List<string> runs;
            runs = [.. Directory.GetDirectories(RunnerConfiguration.FileStoragePath)];
            return runs;
        }
    }
}
