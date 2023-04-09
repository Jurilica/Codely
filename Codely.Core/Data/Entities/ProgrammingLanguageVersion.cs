using Codely.Core.Data.Entities.Base;
using Codely.Core.Types.Enums;

namespace Codely.Core.Data.Entities;

public sealed class ProgrammingLanguageVersion : BaseEntity
{
    public ProgrammingLanguage ProgrammingLanguage { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;
}