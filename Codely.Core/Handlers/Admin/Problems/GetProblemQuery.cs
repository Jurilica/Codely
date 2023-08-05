using Codely.Core.Data;
using Codely.Core.Types;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Problems;

public sealed class GetProblemQuery : IRequestHandler<GetProblemRequest, GetProblemResponse>
{
    private readonly CodelyContext _context;

    public GetProblemQuery(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task<GetProblemResponse> Handle(GetProblemRequest request, CancellationToken cancellationToken)
    {
        var problemData = await _context.Problems
            .Where(x => x.Id == request.ProblemId)
            .Select(x =>
                new GetProblemData
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ProblemStatus = x.Status,
                    Examples = x.Examples
                        .Select(y =>
                            new ExampleData
                            {
                                Id = y.Id,
                                Explanation = y.Explanation,
                                Input = y.Input,
                                Output = y.Output
                            })
                        .OrderBy(y => y.Id)
                        .ToList(),
                    TestCases = x.TestCases
                        .Select(y =>
                            new TestCaseData
                            {
                                Id = y.Id,
                                Input = y.Input,
                                Output = y.Output
                            })
                        .OrderBy(y => y.Id)
                        .ToList()
                })
            .FirstOrDefaultAsync(cancellationToken);

        if (problemData is null)
        {
            throw new CodelyException("Problem not found");
        }

        return new GetProblemResponse
        {
            Problem = problemData
        };
    }
}

public sealed class GetProblemRequest : IRequest<GetProblemResponse>
{
    public required int ProblemId { get; init; }    
}

public sealed class GetProblemResponse
{
    public required GetProblemData Problem { get; init; }
}

public sealed class GetProblemData
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    public required string Description { get; init; }
    
    public required ProblemStatus ProblemStatus { get; init; }

    public required List<ExampleData> Examples { get; init; }

    public required List<TestCaseData> TestCases { get; init; }
}

public sealed class ExampleData
{
    public required int Id { get; init; }

    public required string Input { get; init; }

    public required string Output { get; init; } 

    public required string Explanation { get; init; } 
}

public sealed class TestCaseData
{
    public required int Id { get; init; }
    
    public required string Input { get; init; } 

    public required string Output { get; init; } 
}

