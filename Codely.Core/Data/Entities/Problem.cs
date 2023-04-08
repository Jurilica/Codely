 using Codely.Core.Data.Entities.Base;

namespace Codely.Core.Data.Entities;

public sealed class Problem : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<Example> Examples { get; init; } = new();

    public List<TestCase> TestCases { get; set; } = new();
}
    