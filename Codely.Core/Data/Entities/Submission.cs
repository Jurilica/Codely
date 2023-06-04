using Codely.Core.Data.Entities.Base;
using Codely.Core.Types.Enums;

namespace Codely.Core.Data.Entities;

public sealed class Submission : BaseEntity
{
    public string Answer { get; set; } = string.Empty;

    public SubmissionStatus SubmissionStatus { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public int ProgrammingLanguageVersionId { get; set; }

    public ProgrammingLanguageVersion ProgrammingLanguageVersion { get; set; } = null!;

    public int ProblemId { get; set; }

    public Problem Problem { get; set; } = null!;

    public List<SubmissionTestCase> SubmissionTestCases { get; init; } = new();
}