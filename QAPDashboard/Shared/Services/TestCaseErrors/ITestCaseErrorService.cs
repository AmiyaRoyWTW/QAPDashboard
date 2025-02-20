using QAPDashboard.Shared.Models;

namespace QAPDashboard.Shared.Services.TestCaseErrors
{
    public interface ITestCaseErrorService
    {
        TestCaseError GetTestCaseError(Guid testRunId, Guid testCaseId);
        void Update(TestCaseError testCaseError);
    }
}
