using Codely.Core.Data.Entities.Base;

namespace Codely.Core.Data.Entities;

public sealed class TestCase : BaseEntity
{
    public string Input { get; set; } = string.Empty;

    public string Output { get; set; } = string.Empty;
}