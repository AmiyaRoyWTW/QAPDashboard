using QAPDashboard.Common.DTOs;

namespace QAPDashboard.Common.Bases
{
    public class ViewModelBuilderBase
    {
        public List<BuilderParameterDTO> builderParameters;

        public ViewModelBuilderBase()
        {
            builderParameters = [];
        }

        public string? GetBuilderParameterValue(string parameterName)
        {
            if (builderParameters != null)
            {
                BuilderParameterDTO? parameter = builderParameters.FirstOrDefault(bp => bp.Name == parameterName);
                return parameter?.Value;
            }

            return null;
        }
        public void SetBuilderParameters(List<BuilderParameterDTO> parameters)
        {
            builderParameters = parameters;
        }
    }
}
