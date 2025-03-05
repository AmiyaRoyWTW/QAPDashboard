using System.Globalization;
using QAPDashboard.Common.Bases;
using QAPDashboard.Common.DTOs;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Settings;
using QAPDashboard.Shared.Models.Twillio;
using QAPDashboard.Shared.Services.ExecutionTestList;

namespace QAPDashboard.Areas.RunResults.ViewModels.Builders
{
  public class ExecutedTestViewModelBuilder : ViewModelBuilderBase, IViewModeBuilder<ExecutedTestViewModel>
  {
    private readonly IExecutionTestService _executionTestRunService;
    private readonly IApplicationSettingsManager _applicationSettingsManager;

    public ExecutedTestViewModelBuilder(IExecutionTestService executionTestRunService, IApplicationSettingsManager applicationSettingsManager)
    {
      _executionTestRunService = executionTestRunService;
      _applicationSettingsManager = applicationSettingsManager;
    }

    public ExecutedTestViewModel Build()
    {
      ExecutedTestViewModel executionListVM = new()
      {
        ExecutedTests = _executionTestRunService.GetLocalExecutedTests()
      };
      return executionListVM;
    }

    public ExecutedTestViewModel Build(List<BuilderParameterDTO> builderParameters)
    {
      SetBuilderParameters(builderParameters);
      string dateRangeFilter = GetBuilderParameterValue("dateRangeFilter");
      string startDate = GetBuilderParameterValue("startDate");
      string endDate = GetBuilderParameterValue("endDate");
      List<ExecutedTests> executedTests = _executionTestRunService.GetLocalExecutedTests(dateRangeFilter, startDate, endDate);

      ExecutedTestViewModel executionListVM = new()
      {
        ExecutedTests = executedTests,
        SelectedDateRange = dateRangeFilter,
        CustomStartDate = string.IsNullOrEmpty(startDate) ? null : DateTime.ParseExact(startDate, "yyyyMMddHHmmssfffffff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime().ToString("yyyy-MM-dd"),
        CustomEndDate = string.IsNullOrEmpty(endDate) ? null : DateTime.ParseExact(endDate, "yyyyMMddHHmmssfffffff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime().ToString("yyyy-MM-dd")
      };

      return executionListVM;
    }

  }
}