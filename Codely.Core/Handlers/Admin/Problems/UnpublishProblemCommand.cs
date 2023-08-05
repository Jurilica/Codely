using Codely.Core.Data;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Problems;

public sealed class UnpublishProblemCommand : IRequestHandler<UnpublishProblemRequest, UnpublishProblemResponse>
{
    private readonly CodelyContext _context;

    public UnpublishProblemCommand(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task<UnpublishProblemResponse> Handle(UnpublishProblemRequest request, CancellationToken cancellationToken)
    {
        var problem = await _context.Problems
            .Where(x => x.Id == request.ProblemId)
            .FirstAsync(cancellationToken);

        problem.Status = ProblemStatus.Unpublished;
        await _context.SaveChangesAsync(cancellationToken);

        return new UnpublishProblemResponse();
    }
}

public sealed class UnpublishProblemRequest : IRequest<UnpublishProblemResponse>
{
    public required int ProblemId { get; init; }
}

public sealed class UnpublishProblemResponse
{
}