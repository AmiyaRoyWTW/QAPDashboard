namespace QAPDashboard.Shared.Utilities
{
    public static class IdValidator
    {
        public static bool IsValidTestRunId(Guid testRunId)
        {
            return testRunId != Guid.Empty;
        }

        public static bool IsValidTestCaseId(Guid testCaseId)
        {
            return testCaseId != Guid.Empty;
        }
    }
}
