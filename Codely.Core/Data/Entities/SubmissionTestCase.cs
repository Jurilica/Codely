using Codely.Core.Data.Entities.Base;
using Codely.Core.Types.Enums;

namespace Codely.Core.Data.Entities;

public class SubmissionTestCase : BaseEntity
{
    public required string Output { get; set; }

    public required SubmissionTestCaseStatus SubmissionTestCaseStatus { get; set; }

    public required int TestCaseId { get; set; }

    public TestCase TestCase { get; set; } = null!;

    public required int SubmissionId { get; set; }

    public Submission Submission { get; set; } = null!;
}