using Codely.Core.Data.Entities.Base;
using Codely.Core.Types.Enums;

namespace Codely.Core.Data.Entities;

public sealed class Submission : BaseEntity
{
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string Answer { get; set; } = string.Empty;
    
    public SubmissionStatus SubmissionStatus { get; set; }
}