using Codely.Api.Attributes;
using Codely.Api.Constants;
using Codely.Core.Handlers.Admin.TestCases;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codely.Api.Controllers.Admin;

[Route("admin/test-cases")]
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerConstants.Admin)]
[RoleAuthorize(Role.Admin)]
public class TestCasesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestCasesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<CreateTestCaseResponse> CreateTestCase(CreateTestCaseRequest request)
    {
        return await _mediator.Send(request);
    }
    
    [HttpPut]
    public async Task<UpdateTestCaseResponse> UpdateTestCase(UpdateTestCaseRequest request)
    {
        return await _mediator.Send(request);
    }
    
    [HttpDelete("{id}")]
    public async Task<ArchiveTestCaseResponse> ArchiveTestCase(int id)
    {
        return await _mediator.Send(new ArchiveTestCaseRequest
        {
            TestCaseId = id
        });
    }
}