using Hangfire;

namespace Codely.Core.Services;

public interface ITestCaseJob
{
    void ExecuteTestCases(int submissionId);
}

public class TestCaseJob : ITestCaseJob
{
    public void ExecuteTestCases(int submissionId)
    {
        BackgroundJob.Enqueue<ITestCaseJob>(x => x.ExecuteTestCases(submissionId));
    }
}