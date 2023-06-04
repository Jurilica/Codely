using Codely.Core.Data;
using Codely.Core.Data.Entities;
using Codely.Core.Helpers;
using MediatR;

namespace Codely.Core.Handlers.Admin.Examples;

public sealed class CreateExampleCommand : IRequestHandler<CreateExampleRequest, CreateExampleResponse>
{
    private readonly CodelyContext _context;

    public CreateExampleCommand(CodelyContext context)
    {
        _context = context;
    }

    public async Task<CreateExampleResponse> Handle(CreateExampleRequest request, CancellationToken cancellationToken)
    {
        Guard.Against
            .IsEmpty(request.Input, "Input can't be empty")
            .IsEmpty(request.Output, "Output can't be empty")
            .IsEmpty(request.Explanation, "Explanation can't be empty");

        var example = new Example
        {
            Input = request.Input,
            Output = request.Output,
            Explanation = request.Explanation,
            ProblemId = request.ProblemId
        };

        _context.Examples.Add(example);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateExampleResponse
        {
            ExampleId = example.Id
        };
    }
}

public sealed class CreateExampleRequest : IRequest<CreateExampleResponse>
{
    public int ProblemId { get; set; }

    public string Input { get; set; } = string.Empty;

    public string Output { get; set; } = string.Empty;

    public string Explanation { get; set; } = string.Empty;
}

public sealed class CreateExampleResponse
{
    public int ExampleId { get; set; }
}