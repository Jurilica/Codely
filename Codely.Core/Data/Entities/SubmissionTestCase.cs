using Codely.Core.Data.Entities.Base;
using Codely.Core.Types.Enums;

namespace Codely.Core.Data.Entities;

public class SubmissionTestCase : BaseEntity
{
    public string Output { get; set; } = string.Empty;
    
    public SubmissionTestCaseStatus SubmissionTestCaseStatus { get; set; }

    public int TestCaseId { get; set; }

    public TestCase TestCase { get; set; } = null!;
    
    public int SubmissionId { get; set; }

    public Submission Submission { get; set; } = null!;
}