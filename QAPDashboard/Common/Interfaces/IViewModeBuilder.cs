using QAPDashboard.Common.DTOs;

namespace QAPDashboard.Common.Interfaces
{
    public interface IViewModeBuilder<TViewModel>
    {
        TViewModel Build();
        TViewModel Build(List<BuilderParameterDTO> builderParameters);
    }
}
