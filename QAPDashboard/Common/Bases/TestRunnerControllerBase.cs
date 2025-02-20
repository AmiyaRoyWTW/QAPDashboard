using Microsoft.AspNetCore.Mvc;
using QAPDashboard.Common.DTOs;

namespace QAPDashboard.Common.Bases
{
    public class TestRunnerControllerBase : Controller
    {
        public List<BuilderParameterDTO> builderParameters;

        public TestRunnerControllerBase()
        {
            builderParameters = [];
        }

        [NonAction]
        public void AddVMBuilderParameter(string name, string _value)
        {
            BuilderParameterDTO builderParameterDTO = new()
            {
                Name = name,
                Value = _value
            };
            builderParameters.Add(builderParameterDTO);
        }

        /*public ITestSuiteCacheService SelectTestSuiteManagerByFramework(IEnumerable<ITestSuiteCacheService> testSuiteManagerServices, string framework)
        {
            try
            {
                ITestSuiteCacheService testSuiteManagerService = testSuiteManagerServices.FirstOrDefault(testSuiteManagerService => testSuiteManagerService.GetType().Name.Contains(framework, StringComparison.OrdinalIgnoreCase));
                return testSuiteManagerService;
            }
            catch (Exception) { return null; }


        }*/
    }
}
