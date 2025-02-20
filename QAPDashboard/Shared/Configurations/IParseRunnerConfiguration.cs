namespace QAPDashboard.Shared.Configurations
{
    public interface IParseRunnerConfiguration
    {
        void LoadDefaultOptions(IConfiguration configuration);

        bool GetOption(string arg, string value, int pos);
    }
}
