using QAPDashboard.Areas.RunResults.ViewModels.Builders;
using QAPDashboard.Areas.RunResults.ViewModels;
using QAPDashboard.Common.Interfaces;
using QAPDashboard.Shared.Configurations;
using QAPDashboard.Shared.Services.TestCaseErrors;
using QAPDashboard.Shared.Services.TestCases;
using QAPDashboard.Shared.Services.TestRunResults;
using QAPDashboard.Shared.Services.TestRuns;
using QAPDashboard.Shared.Services.TestRunSummaries;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Azure;
using Azure.Storage.Queues;
using Azure.Storage.Blobs;
using Azure.Core.Extensions;
using QAPDashboard.Shared.Services.ExecutionTestList;

namespace QAPDashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddMvc(options => options.EnableEndpointRouting = false);
            _ = services.AddSingleton<ILocalTestCaseService, LocalTestCaseService>();
            _ = services.AddSingleton<ITestCaseService, AzureTestCaseService>();
            _ = services.AddSingleton<ITestCaseErrorService, AzureTestCaseErrorService>();
            _ = services.AddSingleton<ITestRunSummaryService, AzureTestRunSummaryService>();
            _ = services.AddSingleton<ILocalTestRunService, LocalTestRunService>();
            _ = services.AddSingleton<ILocalRunResultService, LocalStorageRunResultsService>();
            _ = services.AddSingleton<IExecutionTestService, LocalExecutionTestService>();

            _ = services.AddSingleton<IViewModeBuilder<RunListViewModel>, RunListViewModelBuilder>();
            _ = services.AddSingleton<IViewModeBuilder<RunResultViewModel>, RunResultViewModelBuilder>();
            _ = services.AddSingleton<IViewModeBuilder<TestResultViewModel>, TestResultViewModelBuilder>();
            _ = services.AddSingleton<IViewModeBuilder<ExecutedTestViewModel>, ExecutedTestViewModelBuilder>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            _ = env.IsDevelopment() ? app.UseDeveloperExceptionPage() : app.UseExceptionHandler("/home/error");

            //_ = app.UseMiddleware<HostFilteringMiddleware>();

            _ = app.UseStaticFiles();
            //app.UseHttpsRedirection;
            _ = app.UseRouting();
            _ = app.UseEndpoints(endpoints =>
            {
                foreach (string areaName in new[] { "RunResults" })
                {
                    _ = endpoints.MapAreaControllerRoute(areaName, areaName, "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                }

                _ = endpoints.MapControllers();
            });

            OpenUrl($"http://localhost:{RunnerConfiguration.DashboardPort}/run-list");
        }

        public void OpenUrl(string url)
        {
            try
            {
                _ = Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    _ = Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    _ = Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    _ = Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
