using Codely.Core.Data;
using Codely.Core.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.TestCases;

public sealed class UpdateTestCaseCommand : IRequestHandler<UpdateTestCaseRequest, UpdateTestCaseResponse>
{
    private readonly CodelyContext _context;

    public UpdateTestCaseCommand(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task<UpdateTestCaseResponse> Handle(UpdateTestCaseRequest request, CancellationToken cancellationToken)
    {
        Guard.Against
            .IsEmpty(request.Input, "Input can't be empty")
            .IsEmpty(request.Output, "Output can't be empty");

        var testCase = await _context.TestCases
            .Where(x => x.Id == request.TestCaseId)
            .FirstAsync(cancellationToken);

        testCase.Input = request.Input;
        testCase.Output = request.Output;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateTestCaseResponse();
    }
}

public sealed class UpdateTestCaseRequest : IRequest<UpdateTestCaseResponse>
{
    public int TestCaseId { get; set; }

    public string Input { get; set; } = string.Empty;

    public string Output { get; set; } = string.Empty;
}

public sealed class UpdateTestCaseResponse 
{
    
}