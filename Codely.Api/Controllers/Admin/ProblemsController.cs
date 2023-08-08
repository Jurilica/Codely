using Codely.Api.Attributes;
using Codely.Api.Constants;
using Codely.Core.Handlers.Admin.Problems;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codely.Api.Controllers.Admin;

[Route("admin/problems")]
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerConstants.Admin)]
[RoleAuthorize(Role.Admin)]
public class ProblemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProblemsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<GetProblemsResponse> GetProblems()
    {
        return await _mediator.Send(new GetProblemsRequest());
    }

    [HttpGet("{id}")]
    public async Task<GetProblemResponse> GetProblems(int id)
    {
        return await _mediator.Send(new GetProblemRequest
        {
            ProblemId = id
        });
    }

    [HttpPost]
    public async Task<CreateProblemResponse> CreateProblem(CreateProblemRequest request)
    {
        return await _mediator.Send(request);
    }
    
    [HttpPut]
    public async Task<UpdateProblemResponse> UpdateProblem(UpdateProblemRequest request)
    {
        return await _mediator.Send(request);
    }
    
    [HttpPut("{id}/publish")]
    public async Task<PublishProblemResponse> PublishProblems(int id)
    {
        return await _mediator.Send(new PublishProblemRequest
        {
            ProblemId = id
        });
    }
    
    [HttpPut("{id}/unpublish")]
    public async Task<UnpublishProblemResponse> UnpublishProblems(int id)
    {
        return await _mediator.Send(new UnpublishProblemRequest
        {
            ProblemId = id
        });
    }
    
    [HttpDelete("{id}")]
    public async Task<ArchiveProblemResponse> ArchiveProblems(int id)
    {
        return await _mediator.Send(new ArchiveProblemRequest
        {
            ProblemId = id
        });
    }
}