using Codely.Core.Data.Entities.Base;
using Codely.Core.Types.Enums;

namespace Codely.Core.Data.Entities;

public sealed class Problem : BaseEntity
{
    public required string Title { get; set; } = string.Empty;

    public required string Description { get; set; } = string.Empty;

    public required ProblemStatus Status { get; set; }

    public List<Example> Examples { get; init; } = new();

    public List<TestCase> TestCases { get; init; } = new();
}