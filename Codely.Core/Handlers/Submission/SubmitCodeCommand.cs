using MediatR;

namespace Codely.Core.Handlers.Submission;

public sealed class SubmitCodeCommand : IRequestHandler<SubmitCodeRequest, SubmitCodeResponse>
{
    public Task<SubmitCodeResponse> Handle(SubmitCodeRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public sealed class SubmitCodeRequest : IRequest<SubmitCodeResponse>
{
}

public sealed class SubmitCodeResponse
{
}