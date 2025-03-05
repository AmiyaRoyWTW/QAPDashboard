using QAPDashboard.Common.Bases;
using QAPDashboard.Common.DTOs;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Shared.Services.TestCasesLibrary;

namespace QAPDashboard.Areas.TestCasesLibrary.ViewModels.Builders
{
  public class TestCaseManagementViewModelBuilder : ViewModelBuilderBase, IViewModeBuilder<TestCaseManagementViewModel>
  {
    private readonly ITestCasesLibraryService _localTestCasesLibraryService;

    public TestCaseManagementViewModelBuilder(ITestCasesLibraryService localTestCasesLibraryService)
    {
      _localTestCasesLibraryService = localTestCasesLibraryService;
    }

    public TestCaseManagementViewModel Build()
    {
      throw new NotImplementedException();
    }

    public TestCaseManagementViewModel Build(List<BuilderParameterDTO> builderParameters)
    {
      SetBuilderParameters(builderParameters);
      string testCaseName = GetBuilderParameterValue("testCaseName") ?? string.Empty;
      TestCaseManagementViewModel testCasesLibraryViewModelVM = new()
      {
        TwilioTestCase = _localTestCasesLibraryService.GetTwilioTestCase(testCaseName)
      };
      return testCasesLibraryViewModelVM;
    }
  }
}