using Hangfire;

namespace Codely.Core.Services;

public interface ITestCaseJobs
{
    void ExecuteTestCases(int submissionId);
}

public class TestCaseJobs : ITestCaseJobs
{
    public void ExecuteTestCases(int submissionId)
    {
        BackgroundJob.Enqueue<ITestCaseService>(x => x.ExecuteTestCases(submissionId));
    }
}