using Codely.Core.Data;
using Codely.Core.Data.Entities;
using Codely.Core.Helpers;
using MediatR;

namespace Codely.Core.Handlers.Admin.TestCases;

public class CreateTestCaseCommand : IRequestHandler<CreateTestCaseRequest, CreateTestCaseResponse>
{
    private readonly CodelyContext _context;

    public CreateTestCaseCommand(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task<CreateTestCaseResponse> Handle(CreateTestCaseRequest request, CancellationToken cancellationToken)
    {
        Guard.Against
            .IsEmpty(request.Input, "Input can't be empty")
            .IsEmpty(request.Output, "Output can't be empty");

        var testCase = new TestCase
        {
            Input = request.Input,
            Output = request.Output,
            ProblemId = request.ProblemId
        };

        _context.TestCases.Add(testCase);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateTestCaseResponse
        {
            TestCaseId = testCase.Id
        };
    }
}

public sealed class CreateTestCaseRequest : IRequest<CreateTestCaseResponse>
{
    public required int ProblemId { get; init; }
    
    public required string Input { get; init; }

    public required string Output { get; init; }
}

public sealed class CreateTestCaseResponse
{
    public required int TestCaseId { get; init; }
}