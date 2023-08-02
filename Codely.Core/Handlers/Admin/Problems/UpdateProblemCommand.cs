using Codely.Core.Data;
using Codely.Core.Helpers;
using Codely.Core.Types;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Problems;

public sealed class UpdateProblemCommand : IRequestHandler<UpdateProblemRequest, UpdateProblemResponse>
{
    private readonly CodelyContext _context;

    public UpdateProblemCommand(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task<UpdateProblemResponse> Handle(UpdateProblemRequest request, CancellationToken cancellationToken)
    {
        Guard.Against
            .IsEmpty(request.Title, "Title can't be empty")
            .IsEmpty(request.Description, "Description can't be empty");
        
        
        var problemAlreadyExists = await _context.Problems
            .Where(x => x.Title.ToLower() == request.Title.ToLower())
            .Where(x => x.Id != request.ProblemId)
            .AnyAsync(cancellationToken);

        if (problemAlreadyExists)
        {
            throw new CodelyException("Problem with same title already exists");
        }

        var problem = await _context.Problems
            .Where(x => x.Id == request.ProblemId)
            .FirstAsync(cancellationToken);

        problem.Title = request.Title;
        problem.Description = request.Description;
        
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateProblemResponse();
    }
}

public sealed class UpdateProblemRequest : IRequest<UpdateProblemResponse>
{
    public required int ProblemId { get; init; }
    
    public required string Title { get; init; }

    public required string Description { get; init; }
}

public sealed class UpdateProblemResponse
{
}