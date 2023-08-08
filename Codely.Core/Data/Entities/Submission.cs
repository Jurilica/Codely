using Codely.Core.Data.Entities.Base;
using Codely.Core.Types.Enums;

namespace Codely.Core.Data.Entities;

public sealed class Submission : BaseEntity
{
    public required string Answer { get; set; }

    public required SubmissionStatus SubmissionStatus { get; set; }
    
    public required ProgrammingLanguage ProgrammingLanguage { get; set; }

    public required int UserId { get; set; }

    public User User { get; set; } = null!;
    
    public required int ProblemId { get; set; }

    public Problem Problem { get; set; } = null!;

    public List<SubmissionTestCase> SubmissionTestCases { get; init; } = new();
}