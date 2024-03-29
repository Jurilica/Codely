﻿using Codely.Core.Data;
using Codely.Core.Data.Entities;
using Codely.Core.Helpers;
using Codely.Core.Types;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Problems;

public sealed class CreateProblemCommand : IRequestHandler<CreateProblemRequest, CreateProblemResponse>
{
    private readonly CodelyContext _context;

    public CreateProblemCommand(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task<CreateProblemResponse> Handle(CreateProblemRequest request, CancellationToken cancellationToken)
    {
        Guard.Against
            .IsEmpty(request.Title, "Title can't be empty")
            .IsEmpty(request.Description, "Description can't be empty");

        var problemAlreadyExists = await _context.Problems
            .Where(x => x.Title.ToLower() == request.Title.ToLower())
            .AnyAsync(cancellationToken);

        if (problemAlreadyExists)
        {
            throw new CodelyException("Problem with same title already exists");
        }

        var problem = new Problem
        {
            Title = request.Title,
            Description = request.Description,
            Difficulty = request.Difficulty,
            Status = ProblemStatus.Unpublished
        };

        _context.Problems.Add(problem);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateProblemResponse
        {
            ProblemId = problem.Id
        };
    }
}

public sealed class CreateProblemRequest : IRequest<CreateProblemResponse>
{
    public required string Title { get; init; }

    public required string Description { get; init; } 
    
    public required ProblemDifficulty Difficulty { get; init; }
}

public sealed class CreateProblemResponse
{
    public int ProblemId { get; init; }
}