using Codely.Core.Data;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Users;

public sealed class GetUsersQuery : IRequestHandler<GetUsersRequest, GetUsersResponse>
{
    private readonly CodelyContext _context;

    public GetUsersQuery(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .AsNoTracking()
            .Where(x => x.Role == Role.User)
            .Select(x =>
                new GetUsersData
                {
                    Username = x.Username,
                    Email = x.Email,
                    RegistrationDate = x.Created,
                    UserStatus = x.UserStatus
                })
            .ToListAsync(cancellationToken);

        return new GetUsersResponse
        {
            Users = users
        };
    }
}

public sealed class GetUsersRequest : IRequest<GetUsersResponse>
{
}

public sealed class GetUsersResponse
{
    public required List<GetUsersData> Users { get; init; }
}

public sealed class GetUsersData
{
    public required string Username { get; init; }

    public required string Email { get; init; }
    
    public required DateTime RegistrationDate { get; init; }
    
    public required UserStatus UserStatus { get; init; }
}