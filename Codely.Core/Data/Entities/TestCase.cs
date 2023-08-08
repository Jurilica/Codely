﻿using Codely.Core.Data.Entities.Base;

namespace Codely.Core.Data.Entities;

public sealed class TestCase : BaseEntity
{
    public required string Input { get; set; }

    public required string Output { get; set; }

    public required int ProblemId { get; set; }

    public Problem Problem { get; set; } = null!;
}