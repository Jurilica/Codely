using Codely.Api.Constants;
using Codely.Core.Handlers.Admin.Examples;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codely.Api.Controllers.Admin;

[Route("admin/examples")]
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerConstants.Admin)]
public class ExamplesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExamplesController(IMediator mediator)
    {
        _mediator = mediator;
    }   
    
    [HttpPost]
    public async Task<CreateExampleResponse> CreateExample(CreateExampleRequest request)
    {
        return await _mediator.Send(request);
    }
    
    [HttpPut]
    public async Task<UpdateExampleResponse> UpdateExample(UpdateExampleRequest request)
    {
        return await _mediator.Send(request);
    }
    
    [HttpDelete("{id}")]
    public async Task<ArchiveExampleResponse> ArchiveExample(int id)
    {
        return await _mediator.Send(new ArchiveExampleRequest
        {
            ExampleId = id
        });
    }
}