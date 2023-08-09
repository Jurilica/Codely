using Codely.Core.Types.Enums;

namespace Codely.Core.Helpers;

public static class ProblemSubmissionHelper
{
    public static ProblemSubmissionStatus ToProblemSubmissionStatus(this SubmissionStatus? submissionStatus) =>
        submissionStatus switch
        {
            SubmissionStatus.Created => ProblemSubmissionStatus.Pending,
            SubmissionStatus.Failed => ProblemSubmissionStatus.Failed,
            SubmissionStatus.Succeeded => ProblemSubmissionStatus.Succeeded,
            _ => ProblemSubmissionStatus.Unsolved
        };
    
    public static ProblemSubmissionStatus ToProblemSubmissionStatus(this SubmissionStatus submissionStatus) =>
        submissionStatus switch
        {
            SubmissionStatus.Created => ProblemSubmissionStatus.Pending,
            SubmissionStatus.Failed => ProblemSubmissionStatus.Failed,
            SubmissionStatus.Succeeded => ProblemSubmissionStatus.Succeeded,
            _ => ProblemSubmissionStatus.Unsolved
        };
}