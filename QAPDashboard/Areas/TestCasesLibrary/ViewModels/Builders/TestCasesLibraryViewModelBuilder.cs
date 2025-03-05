using QAPDashboard.Common.Bases;
using QAPDashboard.Common.DTOs;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Shared.Services.TestCasesLibrary;

namespace QAPDashboard.Areas.TestCasesLibrary.ViewModels.Builders
{
  public class TestCasesLibraryViewModelBuilder : ViewModelBuilderBase, IViewModeBuilder<TestCasesLibraryViewModel>
  {
    private readonly ITestCasesLibraryService _localTestCasesLibraryService;

    public TestCasesLibraryViewModelBuilder(ITestCasesLibraryService localTestCasesLibraryService)
    {
      _localTestCasesLibraryService = localTestCasesLibraryService;
    }

    public TestCasesLibraryViewModel Build()
    {
      TestCasesLibraryViewModel testCasesLibraryViewModelVM = new()
      {
        TwilioTests = _localTestCasesLibraryService.GetLocalTestNames()
      };
      return testCasesLibraryViewModelVM;
    }

    public TestCasesLibraryViewModel Build(List<BuilderParameterDTO> builderParameters)
    {
      throw new NotImplementedException();
    }

  }
}