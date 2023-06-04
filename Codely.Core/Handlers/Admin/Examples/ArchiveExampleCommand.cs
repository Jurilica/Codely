using Codely.Core.Data;
using Codely.Core.Handlers.Admin.Problems;
using Codely.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Examples;

public sealed class ArchiveExampleCommand : IRequestHandler<ArchiveExampleRequest, ArchiveProblemResponse>
{
    private readonly CodelyContext _context;
    private readonly ISystemTime _systemTime;

    public ArchiveExampleCommand(CodelyContext context, ISystemTime systemTime)
    {
        _context = context;
        _systemTime = systemTime;
    }

    public async Task<ArchiveProblemResponse> Handle(ArchiveExampleRequest request, CancellationToken cancellationToken)
    {
        var example = await _context.Examples
            .Where(x => x.Id == request.ExampleId)
            .FirstAsync(cancellationToken);

        example.Archived = _systemTime.Now;
        await _context.SaveChangesAsync(cancellationToken);

        return new ArchiveProblemResponse();
    }
}

public sealed class ArchiveExampleRequest : IRequest<ArchiveProblemResponse>
{
    public int ExampleId { get; set; }
}

public sealed class ArchiveExampleResponse
{
}