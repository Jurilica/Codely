using Codely.Core.Data;
using Codely.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.TestCases;

public sealed class ArchiveTestCaseCommand : IRequestHandler<ArchiveTestCaseRequest, ArchiveTestCaseResponse>
{
    private readonly CodelyContext _context;
    private readonly ISystemTime _systemTime;

    public ArchiveTestCaseCommand(CodelyContext context, ISystemTime systemTime)
    {
        _context = context;
        _systemTime = systemTime;
    }
    
    public async Task<ArchiveTestCaseResponse> Handle(ArchiveTestCaseRequest request, CancellationToken cancellationToken)
    {
        var testCase = await _context.TestCases
            .Where(x => x.Id == request.TestCaseId)
            .FirstAsync(cancellationToken);

        testCase.Archived = _systemTime.Now;
        await _context.SaveChangesAsync(cancellationToken);

        return new ArchiveTestCaseResponse();
    }
}

public sealed class ArchiveTestCaseRequest : IRequest<ArchiveTestCaseResponse>
{
    public required int TestCaseId { get; init; }
}

public sealed class ArchiveTestCaseResponse
{
    
}

