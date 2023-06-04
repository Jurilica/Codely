using Codely.Core.Data.Entities.Base;

namespace Codely.Core.Data.Entities;

public sealed class Example : BaseEntity
{
    public string Input { get; set; } = string.Empty;

    public string Output { get; set; } = string.Empty;

    public string Explanation { get; set; } = string.Empty;

    public int ProblemId { get; set; }

    public Problem Problem { get; set; } = null!;
}