using QAPDashboard;
using QAPDashboard.Areas.RunResults.ViewModels.Builders;
using QAPDashboard.Areas.RunResults.ViewModels;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Services.TestCaseErrors;
using QAPDashboard.Shared.Services.TestCases;
using QAPDashboard.Shared.Services.TestRunResults;
using QAPDashboard.Shared.Services.TestRuns;
using QAPDashboard.Shared.Services.TestRunSummaries;
using QAPDashboard.Shared.Services.ExecutionTestList;
using QAPDashboard.Settings;
using QAPDashboard.Shared.Services.TestCasesLibrary;
using QAPDashboard.Areas.TestCasesLibrary.ViewModels;
using QAPDashboard.Areas.TestCasesLibrary.ViewModels.Builders;


var builderApp = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

UIRunnerConfigurationParser configurationRunnerParser = new();
IConfiguration configuration = new ConfigurationBuilder().Build();
ConfigurationBaseParser configurationBaseParser = new(configurationRunnerParser, builderApp.Build());

configurationBaseParser.GetConfigurationArguments(args);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
// Add services to the container.
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddSingleton<ILocalTestCaseService, LocalTestCaseService>();
builder.Services.AddSingleton<ITestCaseService, AzureTestCaseService>();
builder.Services.AddSingleton<ITestCaseErrorService, AzureTestCaseErrorService>();
builder.Services.AddSingleton<ITestRunSummaryService, AzureTestRunSummaryService>();
builder.Services.AddSingleton<ILocalTestRunService, LocalTestRunService>();
builder.Services.AddSingleton<ILocalRunResultService, LocalStorageRunResultsService>();
builder.Services.AddSingleton<IExecutionTestService, LocalExecutionTestService>();
builder.Services.AddSingleton<IApplicationSettingsManager, ApplicationSettingManager>();
builder.Services.AddSingleton<ITestCasesLibraryService, LocalTestCasesLibraryService>();
builder.Services.AddSingleton<IViewModeBuilder<TestCaseManagementViewModel>, TestCaseManagementViewModelBuilder>();

builder.Services.AddSingleton<IViewModeBuilder<RunListViewModel>, RunListViewModelBuilder>();
builder.Services.AddSingleton<IViewModeBuilder<RunResultViewModel>, RunResultViewModelBuilder>();
builder.Services.AddSingleton<IViewModeBuilder<TestResultViewModel>, TestResultViewModelBuilder>();
builder.Services.AddSingleton<IViewModeBuilder<ExecutedTestViewModel>, ExecutedTestViewModelBuilder>();
builder.Services.AddSingleton<IViewModeBuilder<TestCasesLibraryViewModel>, TestCasesLibraryViewModelBuilder>();
builder.Services.AddSingleton<IViewModeBuilder<TestCaseManagementViewModel>, TestCaseManagementViewModelBuilder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    foreach (string areaName in new[] { "RunResults", "API", "Settings", "TestCasesLibrary" })
    {
        _ = endpoints.MapAreaControllerRoute(areaName, areaName, "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    }

    _ = endpoints.MapControllers();
});
app.MapGet("/", () => Results.Redirect("/execution-list"));
app.Run();
