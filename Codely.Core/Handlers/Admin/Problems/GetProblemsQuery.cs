using Codely.Core.Data;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Problems;

public sealed class GetProblemsQuery : IRequestHandler<GetProblemsRequest, GetProblemsResponse>
{
    private readonly CodelyContext _context;

    public GetProblemsQuery(CodelyContext context)
    {
        _context = context;
    }

    public async Task<GetProblemsResponse> Handle(GetProblemsRequest request, CancellationToken cancellationToken)
    {
        var problems = await _context.Problems
            .Select(x =>
                new GetProblemsData
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
                        .ToList(),
                    TestCases = x.TestCases
                        .Select(y =>
                            new TestCaseData
                            {
                                Id = y.Id,
                                Input = y.Input,
                                Output = y.Output
                            })
                        .ToList()
                })
            .ToListAsync(cancellationToken);;

        return new GetProblemsResponse
        {
            Problems = problems
        };
    }
}

public sealed class GetProblemsRequest : IRequest<GetProblemsResponse>
{
}

public sealed class GetProblemsResponse
{
    public List<GetProblemsData> Problems = new();
}

public sealed class GetProblemsData
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }
    
    public ProblemStatus ProblemStatus { get; set; }

    public List<ExampleData> Examples { get; set; } = new();

    public List<TestCaseData> TestCases { get; set; } = new();
}

public sealed class ExampleData
{
    public int Id { get; set; }

    public string Input { get; set; } = string.Empty;

    public string Output { get; set; } = string.Empty;

    public string Explanation { get; set; } = string.Empty;
}

public sealed class TestCaseData
{
    public int Id { get; set; }
    
    public string Input { get; set; } = string.Empty;

    public string Output { get; set; } = string.Empty;
}

