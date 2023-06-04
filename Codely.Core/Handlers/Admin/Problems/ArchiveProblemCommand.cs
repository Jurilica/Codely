using Codely.Core.Data;
using Codely.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Problems;

public sealed class ArchiveProblemCommand : IRequestHandler<ArchiveProblemRequest, ArchiveProblemResponse>
{
    private readonly CodelyContext _context;
    private readonly ISystemTime _systemTime;

    public ArchiveProblemCommand(CodelyContext context, ISystemTime systemTime)
    {
        _context = context;
        _systemTime = systemTime;
    }

    public async Task<ArchiveProblemResponse> Handle(ArchiveProblemRequest request, CancellationToken cancellationToken)
    {
        var problem = await _context.Problems
            .Where(x => x.Id == request.ProblemId)
            .FirstAsync(cancellationToken);

        problem.Archived = _systemTime.Now;
        await _context.SaveChangesAsync(cancellationToken);

        return new ArchiveProblemResponse();
    }
}

public sealed class ArchiveProblemRequest : IRequest<ArchiveProblemResponse>
{
    public int ProblemId { get; set; }
}

public sealed class ArchiveProblemResponse
{
}