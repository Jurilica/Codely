using Codely.Core.Data;
using MediatR;

namespace Codely.Core.Handlers.Admin.Problems;

public sealed class GetProblemsQuery: IRequestHandler<GetProblemsRequest, GetProblemsResponse>
{
    private readonly CodelyContext _context;

    public GetProblemsQuery(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task<GetProblemsResponse> Handle(GetProblemsRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public sealed class GetProblemsRequest : IRequest<GetProblemsResponse>
{
}

public sealed class GetProblemsResponse
{
}