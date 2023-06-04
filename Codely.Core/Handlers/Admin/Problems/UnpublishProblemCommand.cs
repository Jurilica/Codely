using Codely.Core.Data;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Problems;

public sealed class UnpublishProblemCommand : IRequestHandler<UnpublishProblemRequest, UnpublishedProblemResponse>
{
    private readonly CodelyContext _context;

    public UnpublishProblemCommand(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task<UnpublishedProblemResponse> Handle(UnpublishProblemRequest request, CancellationToken cancellationToken)
    {
        var problem = await _context.Problems
            .Where(x => x.Id == request.ProblemId)
            .FirstAsync(cancellationToken);

        problem.Status = ProblemStatus.Unpublished;
        await _context.SaveChangesAsync(cancellationToken);

        return new UnpublishedProblemResponse();
    }
}

public sealed class UnpublishProblemRequest : IRequest<UnpublishedProblemResponse>
{
    public int ProblemId { get; set; }
}

public sealed class UnpublishedProblemResponse
{
}