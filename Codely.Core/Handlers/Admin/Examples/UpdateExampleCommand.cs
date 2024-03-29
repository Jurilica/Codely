﻿using Codely.Core.Data;
using Codely.Core.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Examples;

public sealed class UpdateExampleCommand : IRequestHandler<UpdateExampleRequest, UpdateExampleResponse>
{
    private readonly CodelyContext _context;

    public UpdateExampleCommand(CodelyContext context)
    {
        _context = context;
    }

    public async Task<UpdateExampleResponse> Handle(UpdateExampleRequest request, CancellationToken cancellationToken)
    {
        Guard.Against
            .IsEmpty(request.Input, "Input can't be empty")
            .IsEmpty(request.Output, "Output can't be empty")
            .IsEmpty(request.Explanation, "Explanation can't be empty");

        var example = await _context.Examples
            .Where(x => x.Id == request.ExampleId)
            .FirstAsync(cancellationToken);

        example.Input = request.Input;
        example.Output = request.Output;
        example.Explanation = request.Explanation;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateExampleResponse();
    }
}

public sealed class UpdateExampleRequest : IRequest<UpdateExampleResponse>
{
    public required int ExampleId { get; init; }

    public required string Input { get; init; }

    public required string Output { get; init; }

    public required string Explanation { get; init; }
}

public sealed class UpdateExampleResponse
{
}